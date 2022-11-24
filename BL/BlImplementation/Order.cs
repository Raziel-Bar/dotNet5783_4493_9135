using BlApi;
using Dal;

namespace BlImplementation;
internal class Order : IOrder
{
    private DalApi.IDal dal = new DalList();
    /// <summary>
    /// Adminstator action: gets a list of all orders from dal, presented as OrderForList Type.
    /// </summary>
    /// <returns>List of OrderForList objects : type IEnumerable</returns>
    public IEnumerable<BO.OrderForList> RequestOrdersListAdmin()
    {
        IEnumerable<DO.Order> doOrders = dal.Order.GetList(); // getting the DO orders
        IEnumerable<BO.OrderForList> boOrdersList = new List<BO.OrderForList>();
        BO.Order boOrder;
        int amountSum;
        foreach (var item in doOrders) // transformin all DO orders into BO orders so that we can have the Status, amount of items and the total price
        {
            amountSum = 0;
            boOrder = RequestOrderDetails(item.ID);  // transforming DO order into BO order
            foreach (var orderItem in boOrder.ListOfItems) // counting items...
            {
                amountSum += orderItem.Amount;
            }
            boOrdersList.Append(new BO.OrderForList // adding a BO orderToList based on the info we now have
            {
                ID = boOrder.ID,
                CustomerName = boOrder.CustomerName,
                Status = boOrder.Status,
                AmountOfItems = amountSum,
                TotalPrice = boOrder.TotalPrice,
            });
        }
        return boOrdersList;
    }
    /// <summary>
    /// gets a BO order. Meaning, gets the full details of an existing DO Order, including the missing info
    /// </summary>
    /// <param name="orderID">The ID of the order</param>
    /// <returns>a BO order</returns>
    /// <exception cref="BO.InvalidDataException">in case the orderID is negative or 0</exception>
    /// <exception cref="BO.NotFoundInDalException">in case there is no order with such ID</exception>
    /// <exception cref="BO.UnexpectedException">FOR DEVELOPERS: not supposed to happen! but just in case there will be any inner error between the Dal and the Bl.</exception>
    public BO.Order RequestOrderDetails(int orderID)
    {
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            DO.Order doOrder = dal.Order.Get(orderID); // order exists in Dal check
            IEnumerable<DO.OrderItem> doOrderItems = dal.OrderItem.GetItemsInOrder(orderID); // getting a DO orderItems list for the price and amount info
            BO.Order boOrder = new BO.Order // we now sets the "easy" values
            {
                ID = orderID,
                CustomerName = doOrder.CustomerName,
                CustomerAdress = doOrder.CustomerAdress,
                CustomerEmail = doOrder.CustomerEmail,
                OrderDate = doOrder.OrderDate,
                ShipDate = doOrder.ShipDate,
                DeliveryDate = doOrder.DeliveryDate,
            };
            double totalPriceOrder = 0; // sum for the total price
            foreach (var item in doOrderItems) // running over the order items
            {
                double totalPriceUnit = item.Price * item.Amount; // we calc the total product price for the boOrder.ListOfItems.TotalPrice
                boOrder.ListOfItems.Add(new BO.OrderItem // we fill in the ListOfItems
                {
                    ProductID = item.ProductID,
                    OrderItemID = item.OrderItemID,
                    PricePerUnit = item.Price,
                    Amount = item.Amount,
                    ProductName = dal.Product.Get(item.ProductID).Name,
                    TotalPrice = totalPriceUnit
                });
                totalPriceOrder += totalPriceUnit;
            }
            boOrder.TotalPrice = totalPriceOrder; // sets the total price field

            // only 1 value left, the status
            if (boOrder.DeliveryDate != null) boOrder.Status = BO.ORDER_STATUS.DELIVERED;
            else if (boOrder.ShipDate != null) boOrder.Status = BO.ORDER_STATUS.SHIPPED;
            else boOrder.Status = BO.ORDER_STATUS.PENDING;

            return boOrder; // BO order is calculated and ready
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Order", ex);
        }
        catch (Exception) // for developers!. that exception is NOT supposed to happen
        {
            throw new BO.UnexpectedException();
        }
    }
    /// <summary>
    /// sets a shipping date for an existing pending order
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <returns>an Updated BO order</returns>
    /// <exception cref="BO.InvalidDataException">in case the orderID is negative or 0</exception>
    /// <exception cref="BO.DateException">in case the order has already been shipped</exception>
    /// <exception cref="BO.NotFoundInDalException">in case there is no order with such ID in the Dal</exception>
    public BO.Order UpdateOrderShipDateAdmin(int orderID)
    {
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            DO.Order doOrder = dal.Order.Get(orderID); // order exists in Dal check
            if (doOrder.ShipDate != null) throw new BO.DateException(); // order's status check
            doOrder.ShipDate = DateTime.Now;
            dal.Order.Update(doOrder);
            return RequestOrderDetails(orderID);
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Order", ex);
        }
    }
    /// <summary>
    /// sets a delivery date for an existing already shipped order
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns></returns>
    /// <exception cref="BO.InvalidDataException">in case the orderID is negative or 0</exception>
    /// <exception cref="BO.DateException">in case the order has never been shipped or if it's already been delivered</exception>
    /// <exception cref="BO.NotFoundInDalException">in case there is no order with such ID in the Dal</exception>
    public BO.Order UpdateOrderDeliveryDateAdmin(int orderID)
    {
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            DO.Order doOrder = dal.Order.Get(orderID); // order exists in Dal check
            if (doOrder.ShipDate == null || doOrder.DeliveryDate != null) throw new BO.DateException(); // order's status check
            doOrder.DeliveryDate = DateTime.Now;
            dal.Order.Update(doOrder);
            return RequestOrderDetails(orderID);
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Order", ex);
        }
    }
    /// <summary>
    /// makes an orderTracking for an existing order.
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <returns>a tracking list of the order, incuding dates of creation and shipment/delivery (if exist) : type OrderTracking</returns>
    /// <exception cref="BO.InvalidDataException">in case the orderID is negative or 0</exception>
    /// <exception cref="BO.NotFoundInDalException">in case there is no order in dal with such ID</exception>
    public BO.OrderTracking OrderTrackingAdmin(int orderID)
    {
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            BO.Order boOrder = RequestOrderDetails(orderID); // order exists in Dal check
            BO.OrderTracking boOrderTrack = new BO.OrderTracking
            {
                ID = orderID,
                Status = boOrder.Status,
                Tracker = new List<(DateTime? date, string? description)>()
            };
            boOrderTrack.Tracker.Add((boOrder.OrderDate, "Order created"));
            if (boOrder.ShipDate != null) boOrderTrack.Tracker.Add((boOrder.ShipDate, "Order shipped"));
            if (boOrder.DeliveryDate != null) boOrderTrack.Tracker.Add((boOrder.DeliveryDate, "Order Delivered"));
            return boOrderTrack;
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Order", ex);
        }
    }
}

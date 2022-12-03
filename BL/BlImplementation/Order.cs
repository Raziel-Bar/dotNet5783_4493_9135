using BlApi;
using Dal;
using System;

namespace BlImplementation;
internal class Order : IOrder
{
    private DalApi.IDal dal = new DalList();

    /// <summary>
    /// Adminstator action: gets a list of all orders from dal, presented as OrderForList Type.
    /// </summary>
    /// <returns>List of OrderForList objects : type IEnumerable</returns>
    public IEnumerable<BO.OrderForList?> RequestOrdersListAdmin()
    {
        IEnumerable<DO.Order?> doOrders = dal.Order.GetList(); // getting the DO orders

        List<BO.OrderForList?> boOrdersList = new List<BO.OrderForList?>();

        foreach (var item in doOrders) // transforming all DO orders into BO orders so that we can have the Status, amount of items and the total price
        {
            if (item is not null)
            {
                BO.Order boOrder = RequestOrderDetails(item.Value.ID);  // transforming DO order into BO order

                boOrdersList.Add(new BO.OrderForList // adding a BO orderToList based on the info we now have
                {
                    ID = boOrder.ID,
                    CustomerName = boOrder.CustomerName,
                    Status = boOrder.Status,
                    AmountOfItems = boOrder.ListOfItems!.Count(),
                    TotalPrice = boOrder.TotalPrice,
                });
            }
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

            // getting a DO orderItems list for the price and amount info
            IEnumerable<DO.OrderItem?> doOrderItems = dal.OrderItem.GetList(orderItem => orderItem!.Value.OrderID == orderID);

            BO.Order boOrder = new BO.Order // we now sets the "easy" values
            {
                ID = orderID,
                CustomerName = doOrder.CustomerName,
                CustomerAddress = doOrder.CustomerAddress,
                CustomerEmail = doOrder.CustomerEmail,
                OrderDate = doOrder.OrderDate,
                ShipDate = doOrder.ShipDate,
                DeliveryDate = doOrder.DeliveryDate,
                ListOfItems = new List<BO.OrderItem?>()
            };


            double totalPriceOrder = 0; // sum for the total price

            foreach (var item in doOrderItems) // running over the order items
            {
                if (item is DO.OrderItem orderItem)
                {
                    double totalPriceUnit = orderItem.Price * orderItem.Amount; // we calc the total product price for the boOrder.ListOfItems.TotalPrice

                    boOrder.ListOfItems.Add(new BO.OrderItem // we fill in the ListOfItems
                    {
                        ProductID = orderItem.ProductID,
                        OrderItemID = orderItem.OrderItemID,
                        PricePerUnit = orderItem.Price,
                        Amount = orderItem.Amount,
                        ProductName = dal.Product.Get(orderItem.ProductID).Name,
                        TotalPrice = totalPriceUnit
                    });

                    totalPriceOrder += totalPriceUnit;
                }
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
            if (doOrder.ShipDate != null) throw new BO.DateException("Order has already been shipped away!"); // order's status check

            doOrder.ShipDate = DateTime.Now;

            dal.Order.Update(doOrder);

            return RequestOrderDetails(orderID);

            //DateTime dateTime;
            //Console.Write("Enter shipping date: ");//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            //while (!(DateTime.TryParse(Console.ReadLine(), out dateTime)) || dateTime <= doOrder.OrderDate || dateTime > DateTime.Now)
            //{
            //    Console.WriteLine("Error. please enter a valid input");/// @#@#@#@#@#@#@#@#$$$$$$$$$$$$$$$$$$$$$$$$$
            //}
            //doOrder.ShipDate = dateTime;
            //dal.Order.Update(doOrder);
            //return RequestOrderDetails(orderID);
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

            if (doOrder.ShipDate == null) throw new BO.DateException("Order has'nt been shipped yet!"); // order's status check

            if (doOrder.DeliveryDate != null) throw new BO.DateException("Order has already been delivered!"); // order's status check


            doOrder.ShipDate = DateTime.Now;
            dal.Order.Update(doOrder);

            return RequestOrderDetails(orderID); // after we update the dal the BO.order will be update too  


            //Console.Write("Enter Delivery date: ");//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //while (!(DateTime.TryParse(Console.ReadLine(), out dateTime)) || dateTime <= doOrder.ShipDate || dateTime > DateTime.Now)
            //{
            //    // לשנות לחריגות!!!!!!!!!!!!!!!!
            //    Console.WriteLine("Error. please enter a valid input");//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
            //}
            //doOrder.ShipDate = dateTime;

            //dal.Order.Update(doOrder);

            //return RequestOrderDetails(orderID);
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

            boOrderTrack.Tracker.Add((boOrder.OrderDate, " Order created"));

            if (boOrder.ShipDate != null) boOrderTrack.Tracker.Add((boOrder.ShipDate, " Order shipped"));

            if (boOrder.DeliveryDate != null) boOrderTrack.Tracker.Add((boOrder.DeliveryDate, " Order Delivered"));
            return boOrderTrack;
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Order", ex);
        }
    }


    /// <summary>
    /// updates an already confirmed order's orderItem's details
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <param name="orderItemID">The orderItem's ID</param>
    /// <param name="newAmount">The orderItem's new amount</param>
    /// <exception cref="BO.InvalidDataException">any ID is invalid or the amount is negative</exception>
    /// <exception cref="BO.NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the order</exception>
    /// <exception cref="BO.DateException">in case the order has already been shipped and we can't change it anymore</exception>
    /// <BONUS_METHOD_explanation>
    /// We designed this method based on OUR understanding of it.
    /// We noticed that there is no USER method to update an order ONCE YOU CONFIRMED IT.
    /// Thus, in cases where a customer will want to change an exisiting confirmed order, he Will need the help of the Administrator
    /// The method will only take care of one item at a time!!
    /// Special cases: 
    /// In case it is the only item in the order and the newAmount is 0, the order will be deleted from the Dal
    /// In case The Order has already been shipped or delivered, the method will throw an appropriate exception stating the update failed 
    /// NOTE: FOR THE TIME BEING, this method will be void. In the future it might turn into either DO or BO order...
    /// </BONUS_METHOD_explanation>
    public void UpdateOrderAdmin(int orderID, int productID, int orderItemID, int newAmount)
    {
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check

        if (productID < 100000) throw new BO.InvalidDataException("Product"); // productID validity check

        if (newAmount < 0) throw new BO.InvalidDataException("amount"); // amount validity check
        try
        {
            BO.Order boOrder = RequestOrderDetails(orderID); // order exists in Dal check

            boOrder.ListOfItems ??= new List<BO.OrderItem?>();

            DO.Order doOrder = dal.Order.Get(orderID);

            DO.Product dataProduct = dal.Product.Get(productID);

            if (orderItemID != 0)
            {
                DO.OrderItem orderItem = dal.OrderItem.Get(orderItemID);

                if (orderItem.ProductID != productID || orderItem.OrderID != orderID) throw new BO.InvalidDataException("ID");
            }
            if (dataProduct.InStock < newAmount) throw new BO.StockNotEnoughtOrEmptyException();// stock amount check

            if (boOrder.ShipDate != null) throw new BO.DateException("Order has already been shipped away!"); // checks if the order has already been shipped 


            if (boOrder.ListOfItems.Find(item => item!.OrderItemID == orderItemID) == null) // new add
            {
                boOrder.ListOfItems.Add(new BO.OrderItem
                {
                    ProductID = dataProduct.ID,
                    PricePerUnit = dataProduct.Price,
                    ProductName = dataProduct.Name,
                    Amount = newAmount,
                    TotalPrice = dataProduct.Price * newAmount
                });
                dataProduct.InStock -= newAmount;
                dal.OrderItem.Add(new DO.OrderItem
                {
                    Amount = newAmount,
                    OrderID = orderID,
                    OrderItemID = 0, // will be added in the dalOrderItem method
                    Price = dataProduct.Price,
                    ProductID = dataProduct.ID
                });
            }
            else // caretake of an existing item
            {
                BO.OrderItem? _orderItem = boOrder.ListOfItems.First(item => item!.OrderItemID == orderItemID); // copying old item

                boOrder.ListOfItems.Remove(boOrder.ListOfItems.First(item => item!.OrderItemID == orderItemID)); // removing old item

                if (boOrder.ListOfItems.Count == 0 && newAmount == 0)//delete an intire order
                {
                    dataProduct.InStock += _orderItem.Amount;
                    dal.Order.Delete(orderID);
                    dal.OrderItem.Delete(_orderItem.OrderItemID);
                }
                else if (newAmount == 0)// removing the item entirely
                {
                    boOrder.TotalPrice -= _orderItem.TotalPrice;
                    dataProduct.InStock += _orderItem.Amount;
                    dal.OrderItem.Delete(_orderItem.OrderItemID);
                }
                else // item simple update
                {
                    dataProduct.InStock += _orderItem.Amount; // adding the old amount
                    boOrder.TotalPrice -= _orderItem.TotalPrice; // erasing old total item price
                    _orderItem.Amount = newAmount; //setting new amount
                    _orderItem.PricePerUnit = dataProduct.Price; // new price per unit
                    _orderItem.TotalPrice = newAmount * _orderItem.PricePerUnit; // new total item price
                    boOrder.TotalPrice += _orderItem.TotalPrice; // adding to cart total price
                    boOrder.ListOfItems.Add(_orderItem);
                    dataProduct.InStock -= newAmount; // taking back the new amount
                    dal.OrderItem.Update(new DO.OrderItem
                    {
                        Amount = newAmount,
                        OrderID = orderID,
                        OrderItemID = orderItemID,
                        Price = dataProduct.Price,
                        ProductID = dataProduct.ID
                    }); // orderItem update
                }
            }
            dal.Product.Update(dataProduct); // product dal update
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Order/OrderItem", ex);
        }
    }
}

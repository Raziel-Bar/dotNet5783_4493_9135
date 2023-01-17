using BlApi;
using Dal;
namespace BlImplementation;
internal class Order : IOrder
{
    private readonly DalApi.IDal? dal = DalApi.Factory.Get();

    /// <summary>
    /// Adminstator action: gets a list of all orders from dal, presented as OrderForList Type.
    /// </summary>
    /// <returns>List of OrderForList objects : type IEnumerable</returns>
    public IEnumerable<BO.OrderForList?> RequestOrdersListAdmin()
    {
        IEnumerable<DO.Order?> doOrders = dal?.Order.GetList() ?? throw new BO.UnexpectedException(); ; // getting the DO orders      

        IEnumerable<BO.OrderForList?> boList = from order in doOrders
                                               let boOrder = RequestOrderDetails((int)order?.ID!)
                                               orderby order?.ID
                                               select boOrder.CopyPropTo(new BO.OrderForList
                                               {
                                                   TotalPrice = boOrder.ListOfItems!.Sum(orderItem => orderItem!.Price * orderItem.Amount),
                                                   Amount = boOrder.ListOfItems!.Count
                                               });

        return boList;
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
        // ID validity check
        IDCheck(orderID);
        try
        {
            DO.Order doOrder = dal?.Order.Get(orderID) ?? throw new BO.UnexpectedException(); // order exists in Dal

            BO.Order boOrder = doOrder.CopyPropTo(new BO.Order()); // bonus // we now sets the "easy" values - the identical props


            // Now for the ListOfitems, TotalPrice and Status //

            // getting a DO orderItems list for the price and amount info
            IEnumerable<DO.OrderItem?> doOrderItems = dal.OrderItem.GetList(orderItem => orderItem?.OrderID == orderID);

            /*
             double totalPriceOrder = 0; // sum for the total price

             boOrder.ListOfItems = new List<BO.OrderItem?>();

             foreach (var item in doOrderItems) // running over the order items
             {
                 if (item is DO.OrderItem orderItem)
                 {
                     BO.OrderItem boOrderItem = orderItem.CopyPropTo(new BO.OrderItem()); // Bonus

                     boOrderItem.ProductName = dal.Product.Get(orderItem.ProductID)?.Name; // unique prop

                     boOrderItem.TotalPrice = orderItem.Price * orderItem.Amount; // unique prop

                     boOrder.ListOfItems.Add(boOrderItem);

                     totalPriceOrder += boOrderItem.TotalPrice;
                 }
             }                                          &*&*&*&*&*&*&*&*&*&*&**/

            boOrder.ListOfItems = (from doOrderItem in doOrderItems
                                   orderby dal.Product.Get((int)doOrderItem?.ProductID!)?.Name // we sort the items by their names and not their OrderItemID
                                   select doOrderItem.CopyPropTo(new BO.OrderItem
                                   {
                                       ProductName = dal.Product.Get((int)doOrderItem?.ProductID!)?.Name,
                                       TotalPrice = (double)doOrderItem?.Price! * (int)doOrderItem?.Amount!
                                   })).ToList(); // casting our temp : Ienumrable<T> into List<T>

            boOrder.TotalPrice = boOrder.ListOfItems!.Sum(boOrderItem => boOrderItem!.TotalPrice); // sum for the total price

            // boOrder.TotalPrice = totalPriceOrder; // sets the total price field &*&*&*&*&*&*&*&*&*&**&*

            boOrder.Status = GetStatus(doOrder); // only 1 value left, the status

            return boOrder; // BO order is calculated and ready
        }

        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order", ex); }
        catch (Exception) { throw new BO.UnexpectedException(); } // for developers!. that exception is NOT supposed to happen
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
        IDCheck(orderID);
        try
        {
            DO.Order doOrder = dal?.Order.Get(orderID) ?? throw new BO.UnexpectedException(); // order exists in Dal check

            if (doOrder.ShipDate is not null) throw new BO.DateException("Order has already been shipped away!"); // order's status check

            doOrder.ShipDate = DateTime.Now;

            dal.Order.Update(doOrder);

            return RequestOrderDetails(orderID);
        }
        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order", ex); }
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
        IDCheck(orderID);
        try
        {
            DO.Order doOrder = dal?.Order.Get(orderID) ?? throw new BO.UnexpectedException(); // order exists in Dal check

            if (doOrder.ShipDate is null) throw new BO.DateException("Order has'nt been shipped yet!"); // order's status check

            if (doOrder.DeliveryDate is not null) throw new BO.DateException("Order has already been delivered!"); // order's status check

            doOrder.DeliveryDate = DateTime.Now; // update

            dal.Order.Update(doOrder);

            return RequestOrderDetails(orderID); // after we update the dal the BO.order will be update too  

        }
        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order", ex); }
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
        // ID validity check
        IDCheck(orderID);
        try
        {
            BO.Order boOrder = RequestOrderDetails(orderID); // order exists in Dal check

            BO.OrderTracking boOrderTrack = boOrder.CopyPropTo(new BO.OrderTracking());

            boOrderTrack.Tracker = new List<(DateTime? date, string? description)>();

            boOrderTrack.Tracker.Add((boOrder.OrderDate, " Order created"));

            if (boOrder.ShipDate is not null) boOrderTrack.Tracker.Add((boOrder.ShipDate, " Order shipped"));

            if (boOrder.DeliveryDate is not null) boOrderTrack.Tracker.Add((boOrder.DeliveryDate, " Order Delivered"));

            return boOrderTrack;
        }
        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order", ex); }
    }

    /// <summary>
    /// BONUS updates an already confirmed order's orderItem's details
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

        IDCheck(orderID);// Order ID validity check

        if (productID < 100000) throw new BO.InvalidDataException("Product"); // productID validity check

        if (newAmount < 0) throw new BO.InvalidDataException("amount"); // amount validity check   
        try
        {
            if (orderItemID > 0) // OrderItem ID check
            {
                DO.OrderItem orderItem = dal?.OrderItem.Get(orderItemID) ?? throw new BO.UnexpectedException();

                if (orderItem.ProductID != productID || orderItem.OrderID != orderID) throw new BO.InvalidDataException("ID"); // ID's match check
            }
            else if (orderItemID < 0) throw new BO.InvalidDataException("ID"); // else => orderItemID == 0 => new add

            // preparing all data for update and some more checks

            BO.Order boOrder = RequestOrderDetails(orderID); // order exists in Dal check

            if (dal?.Product.Get(productID) is DO.Product dataProduct)
            {
                if (dataProduct.InStock < newAmount) throw new BO.StockNotEnoughtOrEmptyException();// stock amount check

                if (boOrder.ShipDate is not null) throw new BO.DateException("Order has already been shipped away!"); // checks if the order has already been shipped 

                boOrder.ListOfItems ??= new List<BO.OrderItem?>(); // insurance for not having null list

                int newDataProductInStock = 0; // since we update Dal.Product database only at the end of the code, we made this variable to store the exact value for the InStock prop

                if (boOrder.ListOfItems.Find(item => item!.OrderItemID == orderItemID) == null) // new add
                {
                    boOrder.ListOfItems.Add(new BO.OrderItem // bonus here is inefficient since we have many props with different names. and we need special assignments
                    {
                        ProductID = dataProduct.ID,
                        Price = dataProduct.Price,
                        ProductName = dataProduct.Name,
                        Amount = newAmount,
                        TotalPrice = dataProduct.Price * newAmount
                    });

                    newDataProductInStock = dataProduct.InStock - newAmount;
                    // updating dal.OrderItem database
                    dal.OrderItem.Add(new DO.OrderItem // bonus here is inefficient since we have many props with different names. and we need special assignments
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

                    if (boOrder.ListOfItems.Count == 0 && newAmount == 0) // delete an intire order
                    {
                        newDataProductInStock = dataProduct.InStock + _orderItem!.Amount;
                        dal.Order.Delete(orderID); // updating Dal.Order
                        dal.OrderItem.Delete(_orderItem.OrderItemID); // updating Dal.OrderItem
                    }
                    else if (newAmount == 0)// removing the item entirely
                    {
                        boOrder.TotalPrice -= _orderItem!.TotalPrice;
                        newDataProductInStock = dataProduct.InStock + _orderItem.Amount;
                        dal.OrderItem.Delete(_orderItem.OrderItemID); // updating Dal.OrderItem
                    }
                    else // item simple amount update
                    {
                        newDataProductInStock = dataProduct.InStock + _orderItem!.Amount; // adding the old amount
                        boOrder.TotalPrice -= _orderItem.TotalPrice; // erasing old total item price
                        _orderItem.Amount = newAmount; //setting new amount
                        _orderItem.Price = dataProduct.Price; // new price per unit
                        _orderItem.TotalPrice = newAmount * _orderItem.Price; // new total item price
                        boOrder.TotalPrice += _orderItem.TotalPrice; // adding to cart total price
                        boOrder.ListOfItems.Add(_orderItem);
                        newDataProductInStock -= newAmount; // taking back the new amount
                        dal.OrderItem.Update(new DO.OrderItem // bonus here is inefficient since we need special assignments
                        {
                            Amount = newAmount,
                            OrderID = orderID,
                            OrderItemID = orderItemID,
                            Price = dataProduct.Price,
                            ProductID = dataProduct.ID
                        }); // orderItem update
                    }
                }
                dataProduct.InStock = newDataProductInStock;
                dal.Product.Update(dataProduct); // Dal.Product Update
            }
            else throw new BO.UnexpectedException(); // not should happens 
        }
        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order/OrderItem", ex); }
    }

    /// <summary>
    /// finds the order (from either the PENDING or SHIPPED orders) that has the oldest status change.
    /// used by the Simulator for taking care of orders
    /// </summary>
    /// <returns>the order that fits the conditions mentioned above</returns>
    public BO.Order? NextOrderInLine()
    {
        IEnumerable<BO.Order> ordersInLine = from order in RequestOrdersListAdmin()
                                             where order.Status != BO.ORDER_STATUS.DELIVERED
                                             select RequestOrderDetails(order.ID);
        return ordersInLine.Where(order => GetLatestDate(order) == ordersInLine.Min(_order => GetLatestDate(_order))).FirstOrDefault();
    }


    #region private utility methods
    /// <summary>
    /// order ID validity general checker
    /// </summary>
    /// <param name="id">given order id number</param>
    /// <exception cref="BO.InvalidDataException">in case the number is negative</exception>
    private static void IDCheck(int id)
    {
        if (id < 0)
            throw new BO.InvalidDataException("Order");
    }

    /// <summary>
    /// order's status calculator. 
    /// </summary>
    /// <param name="order">given order</param>
    /// <returns>the current status of the order</returns>
    private static BO.ORDER_STATUS? GetStatus(DO.Order order) =>
    order.DeliveryDate is not null ? BO.ORDER_STATUS.DELIVERED : order.ShipDate is not null ? BO.ORDER_STATUS.SHIPPED : BO.ORDER_STATUS.PENDING;

    /// <summary>
    /// order's Latest date calculator. 
    /// </summary>
    /// <param name="order">given order</param>
    /// <returns>the latest date in the order's statuc</returns>
    private static DateTime? GetLatestDate(BO.Order order) =>
    order.DeliveryDate is not null ? order.DeliveryDate : order.ShipDate is not null ? order.ShipDate : order.OrderDate;

    #endregion
}

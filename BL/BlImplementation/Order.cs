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
        IEnumerable<DO.Order?> doOrders = dal.Order.GetList(null); // getting the DO orders

        List<BO.OrderForList?> boOrdersList = new List<BO.OrderForList?>();

        foreach (var item in doOrders) // transforming all DO orders into BO orders so that we can have the Status, amount of items and the total price
        {
            if (item is not null)
            {
                BO.Order boOrder = RequestOrderDetails(item.Value.ID);  // transforming DO order into BO order
                BO.OrderForList boOrderForList = new BO.OrderForList();
                PropertyCopier<BO.Order, BO.OrderForList>.Copy(boOrder, boOrderForList); // bonus
                boOrderForList.Amount = boOrder.ListOfItems!.Count(); // unique prop
                boOrdersList.Add(boOrderForList);
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
            DO.Order? doOrder = dal.Order.Get(orderID); // order exists in Dal check
            BO.Order boOrder = new BO.Order(); // we now sets the "easy" values - the identical props
            PropertyCopier<DO.Order?, BO.Order>.Copy(doOrder, boOrder);
            boOrder.ListOfItems = new List<BO.OrderItem?>();

            // Now for the ListOfitems, TotalPrice and Status //

            // getting a DO orderItems list for the price and amount info
            IEnumerable<DO.OrderItem?> doOrderItems = dal.OrderItem.GetList(orderItem => orderItem!.Value.OrderID == orderID);
            double totalPriceOrder = 0; // sum for the total price

            foreach (var item in doOrderItems) // running over the order items
            {
                if (item is DO.OrderItem orderItem)
                {
                    BO.OrderItem boOrderItem = new BO.OrderItem();
                    PropertyCopier<DO.OrderItem, BO.OrderItem>.Copy(orderItem, boOrderItem); // Bonus
                    boOrderItem.ProductName = dal.Product.Get(orderItem.ProductID)!.Value.Name; // unique prop
                    boOrderItem.TotalPrice = orderItem.Price * orderItem.Amount; // unique prop
                    boOrder.ListOfItems.Add(boOrderItem);
                    totalPriceOrder += boOrderItem.TotalPrice;
                }
            }

            boOrder.TotalPrice = totalPriceOrder; // sets the total price field

            // only 1 value left, the status
            if (boOrder.DeliveryDate != null) boOrder.Status = BO.ORDER_STATUS.DELIVERED;

            else if (boOrder.ShipDate != null) boOrder.Status = BO.ORDER_STATUS.SHIPPED;

            else boOrder.Status = BO.ORDER_STATUS.PENDING;

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
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            DO.Order? doOrder = dal.Order.Get(orderID); // order exists in Dal check
            if (doOrder!.Value.ShipDate != null) throw new BO.DateException("Order has already been shipped away!"); // order's status check
            DO.Order retDoOrder = new DO.Order();
            PropertyCopier<DO.Order?, DO.Order>.Copy(doOrder, retDoOrder); // Bonus
            retDoOrder.ShipDate = DateTime.Now; // update. CHECK WITH DAN!
            dal.Order.Update(retDoOrder);
            return RequestOrderDetails(orderID);

            // TO CHECK WITH DAN!
            //DateTime dateTime; 
            //Console.Write("Enter shipping date: ");
            //while (!(DateTime.TryParse(Console.ReadLine(), out dateTime)) || dateTime <= doOrder.OrderDate || dateTime > DateTime.Now)
            //{
            //    Console.WriteLine("Error. please enter a valid input");
            //}
            //doOrder.ShipDate = dateTime;
            //dal.Order.Update(doOrder);
            //return RequestOrderDetails(orderID);
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
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            DO.Order? doOrder = dal.Order.Get(orderID); // order exists in Dal check

            if (doOrder!.Value.ShipDate == null) throw new BO.DateException("Order has'nt been shipped yet!"); // order's status check

            if (doOrder.Value.DeliveryDate != null) throw new BO.DateException("Order has already been delivered!"); // order's status check

            DO.Order retDoOrder = new DO.Order();
            PropertyCopier<DO.Order?, DO.Order>.Copy(doOrder, retDoOrder); // Bonus
            retDoOrder.DeliveryDate = DateTime.Now; // update. CHECK WITH DAN!
            dal.Order.Update(retDoOrder);
            return RequestOrderDetails(orderID); // after we update the dal the BO.order will be update too  

            //Console.Write("Enter Delivery date: ");
            //while (!(DateTime.TryParse(Console.ReadLine(), out dateTime)) || dateTime <= doOrder.ShipDate || dateTime > DateTime.Now)
            //{
            //    Console.WriteLine("Error. please enter a valid input");
            //}
            //doOrder.DeliveryDate = dateTime;
            //dal.Order.Update(doOrder);
            //return RequestOrderDetails(orderID);
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
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // ID validity check
        try
        {
            BO.Order boOrder = RequestOrderDetails(orderID); // order exists in Dal check
            BO.OrderTracking boOrderTrack = new BO.OrderTracking // could've use bonus here but in this case, this code is shorter
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
        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order", ex); }
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
        if (orderID < 0) throw new BO.InvalidDataException("Order"); // Order ID validity check
        if (productID < 100000) throw new BO.InvalidDataException("Product"); // productID validity check
        if (newAmount < 0) throw new BO.InvalidDataException("amount"); // amount validity check
        try
        {
            if (orderItemID >= 0) // OrderItem ID check
            {
                DO.OrderItem? orderItem = dal.OrderItem.Get(orderItemID);
                if (orderItem!.Value.ProductID != productID || orderItem.Value.OrderID != orderID) throw new BO.InvalidDataException("ID"); // ID's match check
            }
            else throw new BO.InvalidDataException("ID");

            // preparing all data for update and some more checks
            BO.Order boOrder = RequestOrderDetails(orderID); // order exists in Dal check
            DO.Order? doOrder = dal.Order.Get(orderID); 
            DO.Product? dataProduct = dal.Product.Get(productID);
           
            if (dataProduct!.Value.InStock < newAmount) throw new BO.StockNotEnoughtOrEmptyException();// stock amount check
            if (boOrder.ShipDate != null) throw new BO.DateException("Order has already been shipped away!"); // checks if the order has already been shipped 
            
            boOrder.ListOfItems ??= new List<BO.OrderItem?>(); // insurance for not having null list
            int newDataProductInStock = 0; // since we update Dal.Product database only at the end of the code, we made this variable to store the exact value for the InStock prop

            if (boOrder.ListOfItems.Find(item => item!.OrderItemID == orderItemID) == null) // new add
            {                
                boOrder.ListOfItems.Add(new BO.OrderItem // bonus here is inefficient since we have many props with different names. and we need special assignments
                {
                    ProductID = dataProduct.Value.ID,
                    Price = dataProduct.Value.Price,
                    ProductName = dataProduct.Value.Name,
                    Amount = newAmount,
                    TotalPrice = dataProduct.Value.Price * newAmount
                });
                newDataProductInStock = dataProduct.Value.InStock - newAmount;
                // updating dal.OrderItem database
                dal.OrderItem.Add(new DO.OrderItem // bonus here is inefficient since we have many props with different names. and we need special assignments
                {
                    Amount = newAmount,
                    OrderID = orderID,
                    OrderItemID = 0, // will be added in the dalOrderItem method
                    Price = dataProduct.Value.Price,
                    ProductID = dataProduct.Value.ID
                });
            }
            else // caretake of an existing item
            {
                BO.OrderItem? _orderItem = boOrder.ListOfItems.First(item => item!.OrderItemID == orderItemID); // copying old item
                boOrder.ListOfItems.Remove(boOrder.ListOfItems.First(item => item!.OrderItemID == orderItemID)); // removing old item

                if (boOrder.ListOfItems.Count == 0 && newAmount == 0) // delete an intire order
                {
                    newDataProductInStock = dataProduct.Value.InStock + _orderItem!.Amount;
                    dal.Order.Delete(orderID); // updating Dal.Order
                    dal.OrderItem.Delete(_orderItem.OrderItemID); // updating Dal.OrderItem
                }
                else if (newAmount == 0)// removing the item entirely
                {
                    boOrder.TotalPrice -= _orderItem!.TotalPrice;
                    newDataProductInStock = dataProduct.Value.InStock + _orderItem.Amount;
                    dal.OrderItem.Delete(_orderItem.OrderItemID); // updating Dal.OrderItem
                }
                else // item simple amount update
                {
                    newDataProductInStock = dataProduct.Value.InStock + _orderItem!.Amount; // adding the old amount
                    boOrder.TotalPrice -= _orderItem.TotalPrice; // erasing old total item price
                    _orderItem.Amount = newAmount; //setting new amount
                    _orderItem.Price = dataProduct.Value.Price; // new price per unit
                    _orderItem.TotalPrice = newAmount * _orderItem.Price; // new total item price
                    boOrder.TotalPrice += _orderItem.TotalPrice; // adding to cart total price
                    boOrder.ListOfItems.Add(_orderItem);
                    newDataProductInStock -= newAmount; // taking back the new amount
                    dal.OrderItem.Update(new DO.OrderItem // bonus here is inefficient since we need special assignments
                    {
                        Amount = newAmount,
                        OrderID = orderID,
                        OrderItemID = orderItemID,
                        Price = dataProduct.Value.Price,
                        ProductID = dataProduct.Value.ID
                    }); // orderItem update
                }
            }
            DO.Product doProduct = new DO.Product();
            PropertyCopier<DO.Product?, DO.Product>.Copy(dataProduct, doProduct); // Bonus
            doProduct.InStock = newDataProductInStock;
            dal.Product.Update(doProduct); // Dal.Product Update
        }
        catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Order/OrderItem", ex); }
    }
}

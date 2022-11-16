using DO;
using DalApi;
using static Dal.DataSource;

namespace Dal;


public class DalOrderItem : IOrderItem
{
    /// <summary>
    /// Adds a new order item to the order item's list
    /// </summary>
    /// <param name="newOrderItem">
    /// The new added order item
    /// </param>
    /// <returns>
    /// The new added order item's ID
    /// </returns>
    /// <exception cref="Exception">
    /// In case the order item already exists in the list
    /// </exception>
    public int Add(OrderItem newOrderItem)
    {
        newOrderItem.OrderItemID = getRunNumberOrderItemID; // ID is given here
        _orderItems.Add(newOrderItem);
        return newOrderItem.OrderItemID;
    }

    /// <summary>
    /// Search for an order item based on the orderID and the productID
    /// </summary>
    /// <param name="orderId">
    /// The ID of the order that should include the given item
    /// </param>
    /// <param name="productId">
    /// The ID of the product that we search for in the list
    /// </param>
    /// <returns>
    /// The order item's details
    /// </returns>
    /// <exception cref="Exception">
    /// In case the given item does not exist in the given order, or, of course, if the whole order doesn't exist
    /// </exception>
    public OrderItem GetByOrderAndProcuctIDs(int orderId, int productId)
    {
        OrderItem? orderItem = _orderItems.FirstOrDefault(orderItem => orderItem.OrderID == orderId && orderItem.ProductID == productId);

        if (orderItem == null)
        {
            ExceptionFunctionThrow.NotFoundException("product");
        }

        return orderItem.Value;
    }

    /// <summary>
    /// Global search if the given item exists in any order at all
    /// </summary>
    /// <param name="orderItemId">
    /// ID of the item in the order
    /// </param>
    /// <returns>
    /// The order item's details
    /// </returns>
    /// <exception cref="Exception">
    /// In case the given item does not exist in the list
    /// </exception>
    public OrderItem Get(int orderItemId)
    {
        OrderItem? orderItem = _orderItems.FirstOrDefault(orderItem => orderItem.OrderItemID == orderItemId);
        if (orderItem == null)
            ExceptionFunctionThrow.NotFoundException("order item"); // לבדוק הודעות מתאימות

        return orderItem.Value;

        //   throw new Exception("The order item you search for does not exist");
    }

    /// <summary>
    /// makes a list of all order items that are included in a specific order 
    /// </summary>
    /// <param name="orderId">
    /// The ID of the order we copy all of its items
    /// </param>
    /// <returns>
    /// The made list
    /// </returns>
    public IEnumerable<OrderItem> GetItemsInOrder(int orderId) => _orderItems.Where(orderItem => orderItem.OrderID == orderId);

        //OrderItem[] newlist = Array.FindAll(DataSource._orderItems, p => p.OrderID == orderId);
        //return newlist;
    

    /// <summary>
    /// copies the order item's list into a new array
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public IEnumerable<OrderItem> GetList() => _orderItems.Select(orderItem => orderItem);
        //    OrderItem[] newOrderItemlist = new OrderItem[DataSource._orderItemCounter];
        //    for (int i = 0; i < newOrderItemlist.Length; ++i)
        //        newOrderItemlist[i] = DataSource._orderItems[i];
        //    return newOrderItemlist;
    

    /// <summary>
    /// deletes an order item from the list
    /// </summary>
    /// <param name="orderItemId">
    /// The ID of the to be deleted order item
    /// </param>
    /// <exception cref="Exception">
    /// In case the order item does not exist in the list
    /// </exception>
    public void Delete(int orderItemId) => _orderItems.Remove(Get(orderItemId));  // אם לא מצאנו גט זורק חריגה 

        //int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == orderItemId);

        //if (index == -1)
        //    throw new Exception("The order item you wish to delete does not exist");

        //int last = (--DataSource._orderItemCounter);

        //DataSource._orderItems[index] = DataSource._orderItems[last]; // moving last orderItem's details into the deleted orderItem's cell, running over it

        //Array.Clear(DataSource._orderItems, last, last); // last cell is no longer needed. cleaning...

    

    /// <summary>
    /// Updates a specific order item's details
    /// </summary>
    /// <param name="updateOrderItem">
    /// The updated order item's details
    /// </param>
    /// <exception cref="Exception">
    /// In case the order item does not exist
    /// </exception>
    public void Update(OrderItem updateOrderItem)
    {
        Delete(updateOrderItem.OrderItemID);
        _orderItems.Add(updateOrderItem);  // stay with the same key

        //int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == updateOrderItem.OrderItemID);

        //if (index == -1)
        //    throw new Exception("The order item you wish to update does not exist");

        //DataSource._orderItems[index] = updateOrderItem;
    }
}
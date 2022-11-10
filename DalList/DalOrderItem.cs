using DO;

namespace Dal;
public class DalOrderItem
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
    public int AddNewOrderItem(OrderItem newOrderItem)
    {
        newOrderItem.OrderItemID = DataSource.getRunNumberOrderItemID; // ID is given here

        DataSource._orderItems[DataSource._orderItemCounter++] = newOrderItem;

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
    public OrderItem SearchProductItem(int orderId, int productId)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderID == orderId && p.ProductID == productId);
        if (index == -1)
            throw new Exception("The item product you search for either does not exist or the order does not.");

        return DataSource._orderItems[index];
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
    public OrderItem SearchOrderItem(int orderItemId)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == orderItemId);
        if (index == -1)
            throw new Exception("The order item you want is not exist");

        return DataSource._orderItems[index];
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
    public OrderItem[] OrdersList(int orderId) 
    {
        OrderItem[] newlist = Array.FindAll(DataSource._orderItems, p => p.OrderID == orderId);
        return newlist;
    }

    /// <summary>
    /// copies the order item's list into a new array
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public OrderItem[] OrderItemsList()
    {
        OrderItem[] newOrderItemlist = new OrderItem[DataSource._orderItemCounter];
        for (int i = 0; i < newOrderItemlist.Length; ++i)
            newOrderItemlist[i] = DataSource._orderItems[i];
        return newOrderItemlist;
    }

    /// <summary>
    /// deletes an order item from the list
    /// </summary>
    /// <param name="orderItemId">
    /// The ID of the to be deleted order item
    /// </param>
    /// <exception cref="Exception">
    /// In case the order item does not exist in the list
    /// </exception>
    public void DeleteOrderItem(int orderItemId)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == orderItemId);

        if (index == -1)
            throw new Exception("The order item you want to delete is not exist");

        int last = (--DataSource._orderItemCounter);

        DataSource._orderItems[index] = DataSource._orderItems[last]; // moving last orderItem's details into the deleted orderItem's cell, running over it

        Array.Clear(DataSource._orderItems, last, last); // last cell is no longer needed. cleaning...

    }

    /// <summary>
    /// Updates a specific order item's details
    /// </summary>
    /// <param name="updateOrderItem">
    /// The updated order item's details
    /// </param>
    /// <exception cref="Exception">
    /// In case the order item does not exist
    /// </exception>
    public void UpdateOrderItem(OrderItem updateOrderItem)
    {
        int index = Array.FindIndex(DataSource._orderItems, p => p.OrderItemID == updateOrderItem.OrderItemID);

        if (index == -1)
            throw new Exception("The order item you wish to update does not exist");

        DataSource._orderItems[index] = updateOrderItem;
    }
}
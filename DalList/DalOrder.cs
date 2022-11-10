using DO;

namespace Dal;
public class DalOrder
{
    /// <summary>
    /// Adds a new order to the _orders array
    /// </summary>
    /// <param name="newOrder">
    /// A given new order from the user
    /// </param>
    /// <returns>
    /// the new order ID
    /// </returns>
    /// <exception cref="Exception">
    /// in case the order already exists in the _orders array
    /// </exception>
    public int AddNewOrder(Order newOrder)
    {
        newOrder.ID = DataSource.getRunNumberOrderID; // ID is given here

        DataSource._orders[DataSource._orderCounter++] = newOrder;

        return newOrder.ID;
    }

    /// <summary>
    /// search for a specific order based on its ID
    /// </summary>
    /// <param name="orderId">
    /// A given order ID
    /// </param>
    /// <returns>
    /// The order's details
    /// </returns>
    /// <exception cref="Exception">
    /// In case the order does not exist
    /// </exception>
    public Order SearchOrder(int orderId)
    {
        int index = Array.FindIndex(DataSource._orders, p => p.ID == orderId);

        if (index == -1)
            throw new Exception("The order you search for does not exist");

        return DataSource._orders[index];
    }

    /// <summary>
    /// copies all _orders' RELEVANT cells into a new array
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public Order[] ListOfOrders()
    {
        Order[] newOrderlist = new Order[DataSource._orderCounter];
        for (int i = 0; i < newOrderlist.Length; ++i)
            newOrderlist[i] = DataSource._orders[i];
        return newOrderlist;
    }
    
    /// <summary>
    /// deletes an order from the _orders array
    /// </summary>
    /// <param name="orderId">
    /// the ID of the to be deleted order
    /// </param>
    /// <exception cref="Exception">
    /// In case the order does not exist in the array
    /// </exception>
    public void DeleteOrder(int orderId)
    {
        int index = Array.FindIndex(DataSource._orders, p => p.ID == orderId);

        if (index == -1)
            throw new Exception("The order you wish to delete does not exist");

        int last = (--DataSource._orderCounter);

        DataSource._orders[index] = DataSource._orders[last]; // moving last order's details into the deleted order's cell, running over it

        Array.Clear(DataSource._orders, last, last); // last cell is no longer needed. cleaning...
    }

    /// <summary>
    /// updates an existing order
    /// </summary>
    /// <param name="orderUpdate">
    /// the order's new details
    /// </param>
    /// <exception cref="Exception">
    /// In case the order does not exist
    /// </exception>
    public void UpdateOrder(Order orderUpdate)
    {
        int index = Array.FindIndex(DataSource._orders, p => p.ID == orderUpdate.ID);

        if (index == -1)
            throw new Exception("The order you wish to update does not exist");

        DataSource._orders[index] = orderUpdate;
    }
}
using DO;
using DalApi;
using static Dal.DataSource;
namespace Dal;


//public @@@
internal class DalOrder : IOrder
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
    public int Add(Order newOrder)
    {
        newOrder.ID = getRunNumberOrderID; // ID is given here
        _orders.Add(newOrder);
        return newOrder.ID;

        //@@
        //newOrder.ID = DataSource.getRunNumberOrderID; // ID is given here

        //DataSource._orders[DataSource._orderCounter++] = newOrder;

        //return newOrder.ID;
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
    public Order Get(int orderId)
    {
        Order? order = _orders.FirstOrDefault(order => order.ID == orderId);
        if (order == null)
            throw new NotFoundException("order","search");// לבדוק הודעות מתאימות
        return order.Value;

        //int index = Array.FindIndex(DataSource._orders, p => p.ID == orderId);

        //if (index == -1)
        //    throw new Exception("The order you search for does not exist");

        //return DataSource._orders[index];
    }

    /// <summary>
    /// copies all _orders' RELEVANT cells into a new array
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public IEnumerable<Order> GetList() => _orders.Select(order => order);
    //{
        
        //Order[] newOrderlist = new Order[DataSource._orderCounter];
        //for (int i = 0; i < newOrderlist.Length; ++i)
        //    newOrderlist[i] = DataSource._orders[i];
        //return newOrderlist;
  //  }

    /// <summary>
    /// deletes an order from the _orders array
    /// </summary>
    /// <param name="orderId">
    /// the ID of the to be deleted order
    /// </param>
    /// <exception cref="Exception">
    /// In case the order does not exist in the array
    /// </exception>
    public void Delete(int orderId) => _orders.Remove(Get(orderId));  // אם לא מצאנו גט זורק חריגה 
    //{
       

        //int index = Array.FindIndex(DataSource._orders, p => p.ID == orderId);

        //if (index == -1)
        //    throw new Exception("The order you wish to delete does not exist");

        //int last = (--DataSource._orderCounter);

        //DataSource._orders[index] = DataSource._orders[last]; // moving last order's details into the deleted order's cell, running over it

        //Array.Clear(DataSource._orders, last, last); // last cell is no longer needed. cleaning...
    //}

    /// <summary>
    /// updates an existing order
    /// </summary>
    /// <param name="orderUpdate">
    /// the order's new details
    /// </param>
    /// <exception cref="Exception">
    /// In case the order does not exist
    /// </exception>
    public void Update(Order orderUpdate)
    {
   
        Delete(orderUpdate.ID);
        _orders.Add(orderUpdate);  // stay with the same key

        //int index = Array.FindIndex(DataSource._orders, p => p.ID == orderUpdate.ID);

        //if (index == -1)
        //    throw new Exception("The order you wish to update does not exist");

        //DataSource._orders[index] = orderUpdate;
    }
}
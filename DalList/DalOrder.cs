using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

/// <summary>
/// Implementation for DalApi.Iorder
/// </summary>
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
    public Order Get(int orderId) => Get(order => order!.Value.ID == orderId);
    



    /// <summary>
    /// deletes an order from the _orders array
    /// </summary>
    /// <param name="orderId">
    /// the ID of the to be deleted order
    /// </param>
    /// <exception cref="Exception">
    /// In case the order does not exist in the array
    /// </exception>
    public void Delete(int orderId) => _orders.Remove(Get(orderId));



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
        _orders.Add(orderUpdate);
    }

    /// <summary>
    /// לשנות את ההערה 
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>

    /// <summary>
    /// copies all _orders' RELEVANT cells into a new array
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? func = null) =>
        func is null ? _orders.Select(order => order) : _orders.Where(func);

    public Order Get(Func<Order?, bool>? func)
    {
        if (_orders.FirstOrDefault(func!) is Order order)
            return order;

        throw new NotFoundException("order");
    }
}
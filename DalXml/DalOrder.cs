using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

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
        /*newOrder.ID = getRunNumberOrderID; // ID is given here
        _orders.Add(newOrder);
        return newOrder.ID;*/
        return 0;
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
    public Order? Get(int orderId)// => Get(order => order?.ID == orderId);
    {
        return null;
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
    public void Delete(int orderId)// => _orders.Remove(Get(orderId));
    {
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
    public void Update(Order orderUpdate)
    {
        /*Delete(orderUpdate.ID);
        _orders.Add(orderUpdate);*/
    }

    /// <summary>
    /// returns a list of all orders that fits a given condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the full list of all existing orders</param>
    /// <returns>the list of all orders that got true in the condition</returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? func = null)/* =>
        func is null ? _orders.Select(order => order) : _orders.Where(func);*/
    {
        return null;
    }

    /// <summary>
    /// gets an order that fits a condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the order</param>
    /// <returns>the order if the condition was true</returns>
    /// <exception cref="NotFoundException">In case we didn't find an order that fits the condition</exception>
    public Order? Get(Func<Order?, bool>? func)
    {
        /*if (_orders.FirstOrDefault(func!) is Order order)
            return order;

        throw new NotFoundException("order");*/
        return null;
    }

}

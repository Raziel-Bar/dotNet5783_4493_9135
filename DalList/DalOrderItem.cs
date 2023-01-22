using DalApi;
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;
namespace Dal;

/// <summary>
/// Implementation for DalApi.IOrderItem
/// </summary>
internal class DalOrderItem : IOrderItem
{
    [MethodImpl(MethodImplOptions.Synchronized)]
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

    [MethodImpl(MethodImplOptions.Synchronized)]
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
    public OrderItem? Get(int orderItemId) => Get(item => item?.OrderItemID == orderItemId);

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// deletes an order item from the list
    /// </summary>
    /// <param name="orderItemId">
    /// The ID of the to be deleted order item
    /// </param>
    /// <exception cref="Exception">
    /// In case the order item does not exist in the list
    /// </exception>
    public void Delete(int orderItemId) => _orderItems.Remove(Get(orderItemId));

    [MethodImpl(MethodImplOptions.Synchronized)]
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
        _orderItems.Add(updateOrderItem);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// gets an orderItem that fits a condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives product. in case func == null - we simply get the orderItem</param>
    /// <returns>the product if the condition was true</returns>
    /// <exception cref="NotFoundException">In case we didn't find an orderItem that fits the condition</exception>
    public OrderItem? Get(Func<OrderItem?, bool>? func)
    {
        if (_orderItems.FirstOrDefault(func!) is OrderItem orderItem)
            return orderItem;
        throw new NotFoundException("Order item");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// returns a list of all orderItems that fits a given condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the full list of all existing orderItems</param>
    /// <returns>the list of all orderItems that got true in the condition</returns>
    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? func) =>
        func is null ? _orderItems.Select(orderItem => orderItem) : _orderItems.Where(func);
}
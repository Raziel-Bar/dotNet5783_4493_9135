﻿using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

/// <summary>
/// Implementation for DalApi.IOrderItem
/// </summary>
internal class DalOrderItem : IOrderItem
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
    public OrderItem Get(int orderItemId) => Get(item => item!.Value.OrderID == orderItemId);


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


    public OrderItem Get(Func<OrderItem?, bool>? func)
    {
        if (_orderItems.FirstOrDefault(func!) is OrderItem orderItem)
            return orderItem;
        throw new NotFoundException("Order item");
    }

    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? func = null) =>
        func is null ? _orderItems.Select(orderItem => orderItem) : _orderItems.Where(func);
}
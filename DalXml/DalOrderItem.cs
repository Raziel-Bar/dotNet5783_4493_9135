using DalApi;
using DO;
using System.Xml.Linq;
using static Dal.XmlTools;
namespace Dal;

internal class DalOrderItem : IOrderItem
{
    const string s_entityName = "OrderItems";
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
    public int Add(OrderItem newOrderItem) // to check throw
    {

        XElement orderItemElment = LoadListFromXMLElment(s_entityName);

        int id = getRunNumber(s_entityName);
        newOrderItem.OrderItemID = id;

        XElement newXElement = itemToXelement(newOrderItem, s_entityName);

        orderItemElment.Add(newXElement);

        saveListToXMLElment(orderItemElment, s_entityName);

        saveRunNumber(s_entityName, id);
        return id;
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
    public OrderItem? Get(int orderItemId) => Get(item => item?.OrderItemID == orderItemId);


    /// <summary>
    /// deletes an order item from the list
    /// </summary>
    /// <param name="orderItemId">
    /// The ID of the to be deleted order item
    /// </param>
    /// <exception cref="Exception">
    /// In case the order item does not exist in the list
    /// </exception>
    public void Delete(int orderItemId)
    {

        XElement xElement = LoadListFromXMLElment(s_entityName);

        XElement xElementToDelete = xElement.Elements().FirstOrDefault(e => e.Element(s_entityName).Value == orderItemId.ToString()) ?? throw new NotFoundException("Order item");

        xElementToDelete.Remove();

        saveListToXMLElment(xElement, s_entityName);
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
    public void Update(OrderItem updateOrderItem)
    {
        Delete(updateOrderItem.OrderItemID);
        Add(updateOrderItem);
    }

    /// <summary>
    /// gets an orderItem that fits a condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives product. in case func == null - we simply get the orderItem</param>
    /// <returns>the product if the condition was true</returns>
    /// <exception cref="NotFoundException">In case we didn't find an orderItem that fits the condition</exception>
    public OrderItem? Get(Func<OrderItem?, bool>? func)
    {
        return GetList(func).FirstOrDefault() ?? throw new NotFoundException("Order item");
    }

    /// <summary>
    /// returns a list of all orderItems that fits a given condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the full list of all existing orderItems</param>
    /// <returns>the list of all orderItems that got true in the condition</returns>
    public IEnumerable<OrderItem?> GetList(Func<OrderItem?, bool>? func)// to check throw
    {
        XElement xElement = LoadListFromXMLElment(s_entityName);
        IEnumerable<OrderItem?> items = xelementToItems<OrderItem>(xElement).Select(o => (OrderItem?)o);
        return func is null ? items.Select(o => o) : items.Where(func);
    }

}

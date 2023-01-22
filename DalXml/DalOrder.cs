using DalApi;
using DO;
namespace Dal;
using System.Runtime.CompilerServices;

internal class DalOrder : IOrder
{

    const string s_orders = @"Orders";
    const string s_entityName = "Orders";
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
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order newOrder)
    {
        
        var listOrders = XmlTools.LoadListFromXMLSerializer<Order?>(s_entityName);
        int id = XmlTools.getRunNumber(s_entityName);
        newOrder.ID = id;
        listOrders.Add(newOrder);
        
        XmlTools.SaveListToXMLSerializer(listOrders, s_orders, s_entityName);
        XmlTools.saveRunNumber(s_entityName, id);
        return newOrder.ID;


    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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
    public Order? Get(int orderId) => Get(order => order?.ID == orderId);

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// deletes an order from the _orders array
    /// </summary>
    /// <param name="orderId">
    /// the ID of the to be deleted order
    /// </param>
    /// <exception cref="Exception">
    /// In case the order does not exist in the array
    /// </exception>
    public void Delete(int orderId)
    {
        var listOrders = XmlTools.LoadListFromXMLSerializer<Order?>(s_entityName);
        
        listOrders.Remove(Get(orderId));

        XmlTools.SaveListToXMLSerializer(listOrders, s_orders, s_entityName);

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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
        var listOrders = XmlTools.LoadListFromXMLSerializer<Order?>(s_entityName);
        listOrders.Add(orderUpdate);
        XmlTools.SaveListToXMLSerializer(listOrders, s_orders, s_entityName); // לבדוק 

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// returns a list of all orders that fits a given condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the full list of all existing orders</param>
    /// <returns>the list of all orders that got true in the condition</returns>
    public IEnumerable<Order?> GetList(Func<Order?, bool>? func = null)
    {

        var listOrders = XmlTools.LoadListFromXMLSerializer<Order?>(s_entityName);
        if (func is null)

            return listOrders.Select(ord => ord).OrderBy(ord => ord?.ID);
        else

            return listOrders.Where(func).OrderBy(ord => ord?.ID);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// gets an order that fits a condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the order</param>
    /// <returns>the order if the condition was true</returns>
    /// <exception cref="NotFoundException">In case we didn't find an order that fits the condition</exception>
    public Order? Get(Func<Order?, bool>? func)
    {
        
       var listOrders = XmlTools.LoadListFromXMLSerializer<Order?>(s_entityName);
        if (func is null)
            throw new NotFoundException("order");

        else

            return listOrders.FirstOrDefault(func);
    }



}

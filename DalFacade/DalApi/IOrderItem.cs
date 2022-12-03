using DO;
namespace DalApi;

/// <summary>
/// OrderItem's interface, Implements all Icrud method's accordingly to OrderItem + 2 new personal methods
/// </summary>
public interface IOrderItem : ICrud<OrderItem>
{
    ///// <summary>
    ///// Gets an orderItem's details when given the order's and the specific product's IDs
    ///// </summary>
    ///// <param name="orderID">
    ///// The order's ID
    ///// </param>
    ///// <param name="productID">
    ///// The product's ID
    ///// </param>
    ///// <returns>
    ///// The orderItem obj
    ///// </returns>
    //OrderItem GetByOrderAndProcuctIDs(int orderID, int productID);
    ///// <summary>
    ///// Gets a list :type IEnumerable of all orderItems in a given order
    ///// </summary>
    ///// <param name="orderID">
    ///// The order's ID
    ///// </param>
    ///// <returns>
    ///// a list of all OrderItems in the order
    ///// </returns>
    //IEnumerable<OrderItem> GetItemsInOrder(int orderID);
}

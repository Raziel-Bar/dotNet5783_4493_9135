using BO;
namespace BlApi;

/// <summary>
/// Order's interface
/// </summary>
public interface IOrder
{
    IEnumerable<OrderForList> RequestOrdersListAdmin();
    Order RequestOrderDetails(int orderID);
    Order UpdateOrderShipDateAdmin(int orderID);
    Order UpdateOrderDeliveryDateAdmin(int orderID);
    OrderTracking OrderTrackingAdmin(int orderID);

    /// <summary>
    /// updates an already confirmed order's orderItem's details
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <param name="orderItemID">The orderItem's ID</param>
    /// <param name="newAmount">The orderItem's new amount</param>
    /// <BONUS_METHOD_explanation>
    /// We designed this method based on OUR understanding of it.
    /// We noticed that there is no USER method to update an order ONCE YOU CONFIRMED IT.
    /// Thus, in cases where a customer will want to change an exisiting confirmed order, he Will need the help of the Administrator
    /// The method will only take care of one item at a time!!
    /// Special cases: 
    /// In case it is the only item in the order and the newAmount is 0, the order will be deleted from the Dal
    /// In case The Order has already been shipped or delivered, the method will throw an appropriate exception stating the update failed 
    /// </BONUS_METHOD_explanation>
    //void UpdateOrderAdmin(int orderID, int orderItemID, int newAmount);
}

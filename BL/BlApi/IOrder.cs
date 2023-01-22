using BO;
namespace BlApi;

/// <summary>
/// Order's interface
/// </summary>
public interface IOrder
{
    /// <summary>
    /// Adminstator action: gets a list of all orders from dal, presented as OrderForList Type.
    /// </summary>
    /// <returns></returns>
    IEnumerable<OrderForList?> RequestOrdersListAdmin();

    /// <summary>
    /// gets a BO order. Meaning, gets the full details of an existing DO Order, including the missing info
    /// </summary>
    /// <param name="orderID">The ID of the order</param>
    /// <returns></returns>
    Order RequestOrderDetails(int orderID);

    /// <summary>
    /// sets a shipping date for an existing pending order
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <returns>an Updated BO order</returns>
    /// <returns></returns>
    Order UpdateOrderShipDateAdmin(int orderID);

    /// <summary>
    /// sets a delivery date for an existing already shipped order
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns></returns>
    Order UpdateOrderDeliveryDateAdmin(int orderID);

    /// <summary>
    /// makes an orderTracking for an existing order.
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <returns>a tracking list of the order, incuding dates of creation and shipment/delivery (if exist) : type OrderTracking
    /// <returns></returns>
    /// 
    OrderTracking OrderTrackingAdmin(int orderID);

    /// <summary>
    /// updates an already confirmed order's orderItem's details
    /// </summary>
    /// <param name="orderID">The order's ID</param>
    /// <param name="productID">The product ID</param>
    /// <param name="orderItemID">The orderItem's ID</param>
    /// <param name="newAmount">The orderItem's new amount</param>
    /// <exception cref="BO.InvalidDataException">any ID is invalid or the amount is negative</exception>
    /// <exception cref="BO.NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the order</exception>
    /// <exception cref="BO.DateException">in case the order has already been shipped and we can't change it anymore</exception>
    /// <BONUS_METHOD_explanation>
    /// We designed this method based on OUR understanding of it.
    /// We noticed that there is no USER method to update an order ONCE YOU CONFIRMED IT.
    /// Thus, in cases where a customer will want to change an exisiting confirmed order, he Will need the help of the Administrator
    /// The method will only take care of one item at a time!!
    /// Special cases: 
    /// In case it is the only item in the order and the newAmount is 0, the order will be deleted from the Dal
    /// In case The Order has already been shipped or delivered, the method will throw an appropriate exception stating the update failed 
    /// NOTE: FOR THE TIME BEING, this method will be void. In the future it might turn into either DO or BO order...
    /// </BONUS_METHOD_explanation>
    void UpdateOrderAdmin(int orderID, int productID, int orderItemID, int newAmount);

    /// <summary>
    /// finds the order (from either the PENDING or SHIPPED orders) that has the oldest status change.
    /// used by the Simulator for taking care of orders
    /// </summary>
    /// <returns>the order that fits the conditions mentioned above</returns>
    Order? NextOrderInLine();
}
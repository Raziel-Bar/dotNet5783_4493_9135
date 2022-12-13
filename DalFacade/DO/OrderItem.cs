using DalApi;

namespace DO;

/// <summary>
/// Structure for a given item-in-order in database.
/// </summary>
public struct OrderItem
{
    /// <summary>
    /// The orderItem's ID (== ID attribute from the orderItem entity)
    /// </summary>
    public int OrderItemID { get; set; }
    /// <summary>
    /// The product's ID (== ID attribute from the Product entity)
    /// </summary>
    public int ProductID { get; set; }
    /// <summary>
    /// The order's ID (== ID attribute from the Order entity)
    /// </summary>
    public int OrderID { get; set; }
    /// <summary>
    /// The Price of 1 product unit  (== Price attribute from the Product entity)
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// The amount of product units in the order
    /// </summary>
    public int Amount { get; set; }

    public override string ToString() => this.ToStringProperty();
    //public override string ToString() => $@"
    //   Order item ID: {OrderItemID}
    //   Order ID: {OrderID}
    //   Product ID: {ProductID}
    //   Price: {Price}
    //   Amount of product: {Amount}
    //   ";
}




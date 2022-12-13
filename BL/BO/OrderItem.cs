using DalApi;

namespace BO;

/// <summary>
/// Represents an order item's details for both Order and Cart objects
/// </summary>
public class OrderItem
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
    /// The product's name
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// The Price of 1 product unit  (== Price attribute from the Product entity)
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The amount of product units in the order
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The total price for the product : Amount * Price
    /// </summary>
    public double TotalPrice { get; set; }
    public override string ToString() => this.ToStringProperty();
    //public override string ToString() => $@"
    //    Order item ID: {OrderItemID}
    //    Product ID: {ProductID}
    //    Product name: {ProductName}
    //    Price per unit: {Price}
    //    Amount of product: {Amount}
    //    Total: {TotalPrice}";
}

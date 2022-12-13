using DalApi;

namespace BO;

/// <summary>
/// Presents an order's details for LIST
/// </summary>
public class OrderForList
{
    /// <summary>
    /// The order's unique ID (like "barcode")
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The name of the customer who placed the order.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// The current status of an existing order
    /// </summary>
    public ORDER_STATUS? Status { get; set; }

    /// <summary>
    /// The amount of items in the order
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The final price to pay for the whole order
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString() => this.ToStringProperty();
    //$@"
    //Order ID: {ID}
    //Customer name: {CustomerName} 
    //Status: {Status}
    //Amount of items: {Amount}
    //Total: {TotalPrice}";
}

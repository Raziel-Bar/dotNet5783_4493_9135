namespace DO;
/// <summary>
/// Structure for a given order
/// </summary>
public struct Order
{
    /// <summary>
    /// The order's unique ID (like "barcode")
    /// </summary>
    public int ID { get; set; } 
    /// <summary>
    /// The name of the customer who placed the order.
    /// </summary>
    public string CustomerName { get; set; }
    /// <summary>
    /// His Email address (for communication)
    /// </summary>
    public string CustomerEmail { get; set; }   
    /// <summary>
    /// His real address (for delivery)
    /// </summary>
    public int CustomerAdress { get; set; }
    /// <summary>
    /// The date on which the order was placed
    /// </summary>
    public DateTime OrderDate { get; set; }
    /// <summary>
    /// The date on which the order is to be shipped to the customer
    /// </summary>
    public DateTime ShipDate { get; set; }
    /// <summary>
    /// The date on which the order is to be delivered to the customer
    /// </summary>
    public DateTime DelveryrDate { get; set; }
}

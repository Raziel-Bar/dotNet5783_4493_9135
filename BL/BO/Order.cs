namespace BO;

public class Order //@@@@@@@@@@@@@@@@@@@@@@@
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
    /// His Email address (for communication)
    /// </summary>
    public string? CustomerEmail { get; set; }
    /// <summary>
    /// His real address (for delivery)
    /// </summary>
    public string? CustomerAdress { get; set; }
    /// <summary>
    /// The current status of an existing order
    /// </summary>
    public ORDER_STATUS Status { get; set; }
    /// <summary>
    /// The date on which the order was placed
    /// </summary>
    public DateTime? OrderDate { get; set; }
    /// <summary>
    /// The date on which the order is to be shipped to the customer
    /// </summary>
    public DateTime? ShipDate { get; set; }
    /// <summary>
    /// The date on which the order is to be delivered to the customer
    /// </summary>
    public DateTime? DeliveryDate { get; set; }
    /// <summary>
    /// List of all the items in the order
    /// </summary>
    public List<OrderItem> ListOfItems { get; set; } 

    /// <summary>
    /// The final price to pay for the whole order
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
       Order ID: {ID}
       Customer name: {CustomerName} 
       Customer email: {CustomerEmail}
       Customer address: {CustomerAdress}
       Time of the order: {OrderDate} 
       Time of ship: {ShipDate}
       Time of delivering: {DeliveryDate}
       ";
}

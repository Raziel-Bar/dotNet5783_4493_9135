namespace BO;
/// <summary>
/// Represents a customer's cart for user
/// </summary>
public class Cart
{
    /// <summary>
    /// Customer details:
    /// </summary>
    // Name 
    public string? CustomerName { get; set; }
    // Email
    public string? CustomerEmail { get; set; }
    // Address
    public string? CustomerAddress { get; set; }
    /// <summary>
    /// The list of items in order
    /// </summary>
    public List<OrderItem>? ListOfItems { get; set; }
    /// <summary>
    /// The final price to pay for the order
    /// </summary>
    public double TotalPrice { get; set; }
    public override string ToString() => $@"
           --Your Cart--
        Customer details:
        Name: {CustomerName}
        Email: {CustomerEmail}
    	Address: {CustomerAddress}
    	Items:
            {string.Join(Environment.NewLine, ListOfItems)}
        
        --Price in total--: {TotalPrice}
        ";
}

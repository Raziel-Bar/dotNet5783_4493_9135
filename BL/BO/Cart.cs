using System.Diagnostics;
using System.Xml.Linq;

namespace BO;
public class Cart
{
    /// <summary>
    /// Customer details:
    /// </summary>

    // Name
    public string? CustomerName { get; set; }

    // Email
    public string? CustomerEmail { get; set; }

    // Adress
    public string? CustomerAdress { get; set; }

    /// <summary>
    /// The list of items in order
    /// </summary>
    public List<OrderItem> ListOfItems { get; set; }

    /// <summary>
    /// The final price to pay for the order
    /// </summary>
    public double TotalPrice { get; set; }

    public override string ToString() => $@"
           --Your Cart--
        Customer details:
        Name: {CustomerName}
        Email: {CustomerEmail}
    	Address: {CustomerAdress}
    	Order details:
        {string.Join(Environment.NewLine, ListOfItems)}
        Total: {TotalPrice}
        ";
}

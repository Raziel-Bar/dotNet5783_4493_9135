using DO;

namespace BO;
public class ProductItem
{
    /// <summary>
    /// The product's ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The product's name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The product's price per unit
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The product's category
    /// </summary>
    public WINERYS Category { get; set; }

    /// <summary>
    /// checker to know whether the product's amount in stock is empty or not
    /// </summary>
    public Available Available { get; set; }// שיננתי מ BOOL

    /// <summary>
    /// The amount of product units currently in the cart
    /// </summary>
    public int Amount { get; set; }

    public override string ToString() => $@"
        Product ID: {ID} : {Name}
        Category: {Category}
        Price per unit: {Price}
        Available: {(Available == Available.NotAvailable ? "No" : "Yes")}
        Amount in cart: {Amount}";
}

using DO;

namespace BO;
internal class ProductItem
{
    /// <summary>
    /// The product's ID
    /// </summary>
    public int? ID { get; set; }

    /// <summary>
    /// The product's name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The product's price per unit
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// The product's category
    /// </summary>
    public WINERYS Category { get; set; }

    /// <summary>
    /// checker to know whether the product's amount in stock is empty or not
    /// </summary>
    public bool? Available { get; set; }

    /// <summary>
    /// The amount of product units currently in the cart
    /// </summary>
    public int? Amount { get; set; }

    public override string ToString()
    {
        string toString = $@"
        Product ID: {ID} : {Name}
        Category: {Category}
        Price per unit: {Price}";
        if (Available != null)
        {
            toString += $@"
        Available: {(Available == false ? "No" : "Yes")}";
        }
        toString += $@"
        Amount in cart: {Amount}";
        return toString;
    }
}

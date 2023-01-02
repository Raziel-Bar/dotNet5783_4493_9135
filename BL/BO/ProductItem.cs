using DalApi;

namespace BO;

/// <summary>
/// Presents a Product's details for the catalog display
/// </summary>
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
    public WINERIES? Category { get; set; }

    /// <summary>
    /// checker to know whether the product's amount in stock is empty or not
    /// </summary>
    public Available? Available { get; set; }

    /// <summary>
    /// The amount of product units currently in the cart
    /// </summary>
    public int Amount { get; set; }
    public override string ToString() => this.ToStringProperty();
    //$@"
    //Product ID: {ID} : {Name}
    //Category: {Category}
    //Price per unit: {Price}
    //Available: {(Available == BO.Available.Available ? "No" : "Yes")}
    //Amount in cart: {Amount}";
}

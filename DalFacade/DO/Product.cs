using DalApi;

namespace DO;
/// <summary>
/// Structure for a given product in database. In our case, wine!
/// We are running a wine store that sells 4 different types of wine from 5 different wineries
/// We also sell some features for wine lovers, such as pourers, special corks and more
/// </summary>
public struct Product
{
    /// <summary>
    /// The product's unique ID (like "barcode")
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The product's name - should include the winery, vintage year and the grape species included in the bottle.
    /// This is, of course, only in case of wine bottles products...
    /// Features will carry their name only :D
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The price of the product (X1 unit)
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The catergory that shows which winery is the product from (or either we discuss a feature product. category: ELSE)
    /// </summary>
    public WINERIES? Category { get; set; }

    /// <summary>
    /// The product's quantity in the stock
    /// </summary>
    public int InStock { get; set; }

    public override string ToString() => this.ToStringProperty();
    //public override string ToString() => $@"
    //    Product ID: {ID}: {Name}
    //    category: {Category}
    //	Price: {Price}
    //	Amount in stock: {InStock}
    //    ";
}

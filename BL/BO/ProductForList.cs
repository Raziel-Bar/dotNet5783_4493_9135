namespace BO;
public class ProductForList
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
    /// The product's price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The product's category
    /// </summary>
    public WINERYS Category { get; set; }
    public override string ToString() => $@"
        Product ID: {ID}: {Name}
        category: {Category}
    	Price: {Price}
        ";

}

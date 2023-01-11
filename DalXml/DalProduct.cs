using DalApi;
using DO;
namespace Dal;

internal class DalProduct : IProduct
{
    const string s_products = @"Product";

    /// <summary>
    /// adds a new product to the products list
    /// </summary>
    /// <param name="newProduct">
    /// The new product's details
    /// </param>
    /// <returns>
    /// The new product's ID
    /// </returns>
    /// <exception cref="Exception">
    /// In case the product already exists in the list
    /// </exception> 
    public int Add(Product newProduct)
    {
        var listProducts = XmlTools.LoadListFromXMLSerializer<Product?>(s_products);
        if (listProducts.FirstOrDefault(o => o?.ID == newProduct.ID) is not null)
            throw new AlreadyExistException("product");

        listProducts.Add(newProduct);

        XmlTools.SaveListToXMLSerializer(listProducts, s_products, "Product");

        return newProduct.ID;
    }

    /// <summary>
    /// searches for a specific product according to its ID
    /// </summary>
    /// <param name="productId">
    /// The ID of the product we are looking for
    /// </param>
    /// <returns>
    /// The product's details
    /// </returns>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public Product? Get(int productId) => Get(product => product?.ID == productId);


    /// <summary>
    /// deletes a product from the list
    /// </summary>
    /// <param name="productId">
    /// The ID of the product we wish to delete
    /// </param>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public void Delete(int productId) => XmlTools.LoadListFromXMLSerializer<Product?>(s_products).Remove(Get(productId));

    /// <summary>
    /// updates a product's details
    /// </summary>
    /// <param name="uppdateProduct">
    /// The product's new details
    /// </param>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public void Update(Product updateProduct)
    {
        Delete(updateProduct.ID);
        var products = XmlTools.LoadListFromXMLSerializer<Product?>(s_products);
        products.Add(updateProduct);
        XmlTools.SaveListToXMLSerializer(products, s_products, "Product");
    }

    /// <summary>
    /// gets a product that fits a condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives product. in case func == null - we simply get the product</param>
    /// <returns>the product if the condition was true</returns>
    /// <exception cref="NotFoundException">In case we didn't find a product that fits the condition</exception>
    public Product? Get(Func<Product?, bool>? func)
    {
        if (XmlTools.LoadListFromXMLSerializer<Product?>(s_products).FirstOrDefault(func!) is Product product)
            return product;
        throw new NotFoundException("Product");
    }

    /// <summary>
    /// returns a list of all products that fits a given condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the full list of all existing products</param>
    /// <returns>the list of all orders that got true in the condition</returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? func)
    {
        var products = XmlTools.LoadListFromXMLSerializer<Product?>(s_products);
        return func is null ? products.Select(product => product) : products.Where(func);
    }
       

}

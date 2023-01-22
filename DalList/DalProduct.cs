using DalApi;
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;
namespace Dal;



/// <summary>
/// Implementation for DalApi.IProduct
/// </summary>
internal class DalProduct : IProduct
{
    [MethodImpl(MethodImplOptions.Synchronized)]
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

        Product? product = _products.FirstOrDefault(product => product?.ID == newProduct.ID);

        if (product is not null)
            throw new AlreadyExistException("product");

        _products.Add(newProduct);

        return newProduct.ID;

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
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

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// deletes a product from the list
    /// </summary>
    /// <param name="productId">
    /// The ID of the product we wish to delete
    /// </param>
    /// <exception cref="Exception">
    /// In case the product does not exist in the list
    /// </exception>
    public void Delete(int productId) => _products.Remove(Get(productId));

    [MethodImpl(MethodImplOptions.Synchronized)]
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
        _products.Add(updateProduct);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// gets a product that fits a condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives product. in case func == null - we simply get the product</param>
    /// <returns>the product if the condition was true</returns>
    /// <exception cref="NotFoundException">In case we didn't find a product that fits the condition</exception>
    public Product? Get(Func<Product?, bool>? func)
    {
        if (_products.FirstOrDefault(func!) is Product product)
            return product;
        throw new NotFoundException("Product");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    /// <summary>
    /// returns a list of all products that fits a given condition
    /// </summary>
    /// <param name="func">the condition. a function that returns bool and receives order. in case func == null - we simply get the full list of all existing products</param>
    /// <returns>the list of all orders that got true in the condition</returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? func) =>
        func is null ? _products.Select(product => product) : _products.Where(func);
}
using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

/// <summary>
/// Implementation for DalApi.IProduct
/// </summary>
internal class DalProduct : IProduct
{
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
        Product product = _products.FirstOrDefault(product => product.ID == newProduct.ID);

        if (product.ID != 0)
            throw new AlreadyExistException("product");

        _products.Add(newProduct);
        return product.ID;

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
    public Product Get(int productId)
    {
        Product product = _products.FirstOrDefault(product => product.ID == productId);

        if (product.ID == 0)
            throw new NotFoundException("product");

        return product;
    }
    /// <summary>
    /// copies all products from the list into a NEW List
    /// </summary>
    /// <returns>
    /// The new array
    /// </returns>
    public IEnumerable<Product> GetList() => _products.Select(product => product);
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
}
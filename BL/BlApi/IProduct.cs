using BO;
namespace BlApi;

/// <summary>
/// BO.Product's interface
/// </summary>
public interface IProduct
{
    /// <summary>
    /// makes a product's list based on the requested Dal data
    /// </summary>
    /// <returns>
    /// list of products : type Ienumerable
    /// </returns>
    public IEnumerable<ProductForList> RequestProducts();
    /// <summary>
    /// Makes a request to Dal for getting a product's details for administrative use
    /// </summary>
    /// <param name="productID">
    /// The Product's ID
    /// </param>
    /// <returns>
    /// the product (if exists)
    /// </returns>
    /// <exception cref="BO.NotFoundInDalException">The Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public BO.Product RequestProductDetailsAdmin(int productID);
    /// <summary>
    /// Makes a request to Dal for getting a product's details for customer's use (cart)
    /// </summary>
    /// <param name="productID">
    /// The Product's ID
    /// </param>
    /// <returns>
    /// the product (if exists)
    /// </returns>
    /// <exception cref="BO.NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public ProductItem RequestProductDetailsCart(int productID, BO.Cart cart);
    /// <summary>
    /// Adds a Product to the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="product">
    /// The Product that is to be added
    /// </param>
    /// <exception cref="BO.InvalidDataException">If the product's details are invalid</exception>
    /// <exception cref="BO.AlreadyExistInDalException">If the Product already exists in the Dal</exception>
    public void AddProductAdmin(BO.Product product);
    /// <summary>
    /// Removes a Product from the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="productID">
    /// The Product that is to be removed's ID
    /// </param>
    /// <exception cref="BO.RemoveProductThatIsInOrdersException">If product exists in any order/s requests</exception>
    /// <exception cref="BO.NotFoundInDalException">If the product doesn't exist in the Dal</exception>
    /// <exception cref="BO.InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public void RemoveProductAdmin(int productID);
    /// <summary>
    /// Updates a Product to the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="product">
    /// The Product that is to be Updated
    /// </param>
    /// <exception cref="BO.NotFoundInDalException">If the product doesn't exist in the Dal</exception>
    /// <exception cref="BO.InvalidDataException">If the product's details are invalid</exception>
    public void UpdateProductAdmin(BO.Product product);
}

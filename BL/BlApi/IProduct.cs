using BO;
namespace BlApi;

/// <summary>
/// Product's interface
/// </summary>
public interface IProduct
{
    /// <summary>
    /// makes a product's list based on the requested Dal data
    /// </summary>
    /// <returns>
    /// list of products : type Ienumerable
    /// </returns>
   // public IEnumerable<ProductForList?> RequestProducts();
    public IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>> RequestProducts(/*Func<ProductForList?, bool>? func*/);

    //public IEnumerable<BO.ProductForList?> RequestProductsByCondition(IEnumerable<BO.ProductForList?> productForLists, Func<ProductForList?, bool>? func);

    /// <summary>
    /// Makes a request to Dal for getting a product's details for administrative use
    /// </summary>
    /// <param name="productID">
    /// The Product's ID
    /// </param>
    /// <returns>
    /// the product (if exists)
    /// </returns>
    /// <exception cref="NotFoundInDalException">The Product doesn't exist in the Dal</exception>
    /// <exception cref="InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public Product RequestProductDetailsAdmin(int productID);

    /// <summary>
    /// Makes a request to Dal for getting a product's details for customer's use (cart)
    /// </summary>
    /// <param name="productID">
    /// The Product's ID
    /// </param>
    /// <returns>
    /// the product (if exists)
    /// </returns>
    /// <exception cref="NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public ProductItem RequestProductDetailsUser(int productID, Cart cart);

    /// <summary>
    /// Adds a Product to the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="product">
    /// The Product that is to be added
    /// </param>
    /// <exception cref="InvalidDataException">If the product's details are invalid</exception>
    /// <exception cref="AlreadyExistInDalException">If the Product already exists in the Dal</exception>
    public void AddProductAdmin(Product product);
    /// <summary>
    /// Removes a Product from the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="productID">
    /// The Product that is to be removed's ID
    /// </param>
    /// <exception cref="RemoveProductThatIsInOrdersException">If product exists in any order/s requests</exception>
    /// <exception cref="NotFoundInDalException">If the product doesn't exist in the Dal</exception>
    /// <exception cref="InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public void RemoveProductAdmin(int productID);
    /// <summary>
    /// Updates a Product to the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="product">
    /// The Product that is to be Updated
    /// </param>
    /// <exception cref="NotFoundInDalException">If the product doesn't exist in the Dal</exception>
    /// <exception cref="InvalidDataException">If the product's details are invalid</exception>
    public void UpdateProductAdmin(Product product);
}

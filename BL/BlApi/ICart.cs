using BO;
namespace BlApi;

/// <summary>
/// BO.Cart's interface
/// </summary>
public interface ICart
{
    /// <summary>
    /// Adds a product to the cart if All conditions are met
    /// </summary>
    /// <param name="productID">
    /// The product that is to be added's ID
    /// </param>
    /// <param name="cart">
    /// The user's current cart
    /// </param>
    /// <exception cref="NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the cart</exception>
    void AddProductToCart(int productID, Cart cart);
    /// <summary>
    /// Updates the amount of a product in the cart
    /// </summary>
    /// <param name="productID">
    /// The product that is to be added's ID
    /// </param>
    /// <param name="cart">
    /// The user's current cart
    /// </param>
    /// <param name="newAmount">
    /// The new product's amount in the cart
    /// </param>
    /// <exception cref="NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the cart</exception>
    /// <exception cref="ProductNotFoundInCartException">If the product is not in the cart at all</exception>
    void UpdateProductInCart(int productID, Cart cart, int newAmount);
    /// <summary>
    /// Makes an order once a cart is finished and updates all related data in the Dal
    /// </summary>
    /// <param name="cart">
    /// The user's cart
    /// </param>
    /// <param name="name">
    /// The user's name
    /// </param>
    /// <param name="email">
    /// The user's email address
    /// </param>
    /// <param name="address">
    /// The user's address
    /// </param>
    /// <exception cref="NotFoundInDalException">If one of the Product doesn't exist in the Dal</exception>
    /// <exception cref="StockNotEnoughtOrEmptyException">If one of the product's stock is empty so we can't add it to the cart</exception>
    /// <exception cref="InvalidDataException">If one of the customer's or product's details is invalid</exception>
    void ConfirmOrder(Cart cart, string name, string email, string address);
}

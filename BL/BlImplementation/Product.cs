using BlApi;
using Dal;
namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.IProduct
/// </summary>
internal class Product : IProduct
{
    private readonly DalApi.IDal? dal = DalApi.Factory.Get(); //new DalList();

    /// <summary>
    /// makes a product's GROUPS list based on the requested Dal data
    /// </summary>
    /// <returns>
    /// list of products : type IEnumerable<IGrouping<BO.WINERIES? ,BO.ProductForList?>> 
    /// </returns>
    ///
    public IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>> RequestProducts() => dal?.Product.GetList().CopyPropToList<DO.Product?, BO.ProductForList>().GroupBy(_product => _product.Category) ?? throw new BO.UnexpectedException();

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
    public BO.Product RequestProductDetailsAdmin(int productID)
    {
        if (productID >= 100000)
        {
            try
            {
                DO.Product product = dal?.Product.Get(productID) ?? throw new BO.UnexpectedException();
                return product.CopyPropTo(new BO.Product());
            }
            catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    }

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
    public BO.ProductItem RequestProductDetailsUser(int productID, BO.Cart cart)
    {
        if (productID >= 100000)
        {
            try
            {
                int amountInCart = 0;

                if (cart is not null)
                {
                    cart.ListOfItems ??= new List<BO.OrderItem?>();

                    BO.OrderItem? orderItem = cart.ListOfItems.FirstOrDefault(item => item!.ProductID == productID);
                    if (orderItem != null)
                        amountInCart = orderItem.Amount;
                }


                if (dal?.Product.Get(productID) is DO.Product product)
                {
                    BO.ProductItem retProduct = product.CopyPropTo(new BO.ProductItem());

                    retProduct.Amount = amountInCart; // unique prop
                    retProduct.Available = product.InStock > 0 ? BO.Available.Available : BO.Available.Unavailable; // unique prop
                    return retProduct;
                }
                throw new BO.NotFoundInDalException("Product");

            }
            catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    }

    /// <summary>
    /// Adds a Product to the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="product">
    /// The Product that is to be added
    /// </param>
    /// <exception cref="BO.InvalidDataException">If the product's details are invalid</exception>
    /// <exception cref="BO.AlreadyExistInDalException">If the Product already exists in the Dal</exception>
    public void AddProductAdmin(BO.Product product)
    {
        if (product.ID >= 100000 && product.InStock >= 0 && product.Price > 0 && product.Name != null && product.Category != null)
        {
            DO.Product productDo = product.CopyPropToStruct(new DO.Product());

            try { dal?.Product.Add(productDo); }
            catch (DO.AlreadyExistException ex) { throw new BO.AlreadyExistInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    }

    /// <summary>
    /// Removes a Product from the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="productID">
    /// The Product that is to be removed's ID
    /// </param>
    /// <exception cref="BO.RemoveProductThatIsInOrdersException">If product exists in any order/s requests</exception>
    /// <exception cref="BO.NotFoundInDalException">If the product doesn't exist in the Dal</exception>
    /// <exception cref="BO.InvalidDataException">The productID is invalid (less than 6 digits or negative)</exception>
    public void RemoveProductAdmin(int productID)
    {
        if (productID >= 100000)
        {
            if ((bool)(dal?.OrderItem.GetList(orderItem => orderItem?.ProductID == productID).Any()!))
                throw new BO.RemoveProductThatIsInOrdersException();

            try { dal.Product.Delete(productID); }
            catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    }

    /// <summary>
    /// Updates a Product to the database in the Dal if all conditions are met
    /// </summary>
    /// <param name="product">
    /// The Product that is to be Updated
    /// </param>
    /// <exception cref="BO.NotFoundInDalException">If the product doesn't exist in the Dal</exception>
    /// <exception cref="BO.InvalidDataException">If the product's details are invalid</exception>
    public void UpdateProductAdmin(BO.Product product)
    {
        if (product.ID >= 100000 && product.InStock >= 0 && product.Price > 0 && product.Name != null)
        {
            DO.Product dataProduct = product.CopyPropToStruct(new DO.Product());
            try { dal?.Product.Update(dataProduct); }
            catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    }
}

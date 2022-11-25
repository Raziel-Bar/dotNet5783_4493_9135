using BlApi;
using Dal;

namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.IProduct
/// </summary>
internal class Product : IProduct
{
    private DalApi.IDal dal = new DalList();

    /// <summary>
    /// makes a product's list based on the requested Dal data
    /// </summary>
    /// <returns>
    /// list of products : type Ienumerable
    /// </returns>
    public IEnumerable<BO.ProductForList> RequestProducts()
    {
        IEnumerable<DO.Product> products = dal.Product.GetList();

        return products.Select(product => new BO.ProductForList
        {
            ID = product.ID,
            Name = product.Name,
            Category = (BO.WINERYS)product.Category,
            Price = product.Price,
        });
    }
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
                DO.Product product = dal.Product.Get(productID);
                return new BO.Product
                {
                    ID = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Category = (BO.WINERYS)product.Category,
                    InStock = product.InStock,
                };
            }
            catch (DO.NotFoundException ex)
            {
                throw new BO.NotFoundInDalException("Product", ex);
            }
        }
        else
            throw new BO.InvalidDataException("Product");
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
    public BO.ProductItem RequestProductDetailsCart(int productID, BO.Cart cart)
    {
        if (productID >= 100000)
        {
            try
            {
                DO.Product product = dal.Product.Get(productID);

                BO.OrderItem? orderItem = cart.ListOfItems.First(item => item.ProductID == productID);

                if (orderItem is null)
                    throw new BO.ProductNotFoundInCartException();

                return new BO.ProductItem
                {
                    ID = orderItem.ProductID,
                    Name = orderItem.ProductName,
                    Price = orderItem.PricePerUnit,
                    Category = (BO.WINERYS)product.Category,
                    Amount = orderItem.Amount,
                    Available = product.InStock >= orderItem.Amount ? BO.Available.Available : BO.Available.Unavailable
                };
            }
            catch (DO.NotFoundException ex)
            {
                throw new BO.NotFoundInDalException("Product", ex);
            }
        }
        else
            throw new BO.InvalidDataException("Product");
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
        if (product.ID >= 100000 && product.InStock >= 0 && product.Price > 0 && product.Name != null)
        {
            DO.Product product1 = new DO.Product
            {
                ID = product.ID,
                Name = product.Name,
                InStock = product.InStock,
                Category = (DO.WINERYS)product.Category,
                Price = product.Price,
            };
            try
            {
                dal.Product.Add(product1);
            }
            catch (DO.AlreadyExistException ex)
            {
                throw new BO.AlreadyExistInDalException("Product", ex);
            }
        }
        else
            throw new BO.InvalidDataException("Product");
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
            IEnumerable<DO.OrderItem> orderItem = dal.OrderItem.GetList();

            if (orderItem.Any(orderItem => orderItem.ProductID == productID))
                throw new BO.RemoveProductThatIsInOrdersException();

            try
            {
                dal.Product.Delete(productID);
            }
            catch (DO.NotFoundException ex)
            {
                throw new BO.NotFoundInDalException("Product", ex);
            }
        }
        else
            throw new BO.InvalidDataException("Product");
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
            DO.Product dataProduct = new DO.Product
            {
                ID = product.ID,
                InStock = product.InStock,
                Category = (DO.WINERYS)product.Category,
                Name = product.Name,
                Price = product.Price,
            };
            try
            {
                dal.Product.Update(dataProduct);
            }
            catch (DO.NotFoundException ex)
            {
                throw new BO.NotFoundInDalException("Product", ex);
            }
        }
        else
            throw new BO.InvalidDataException("Product");
    }
}

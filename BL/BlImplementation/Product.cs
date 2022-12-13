using BlApi;
using Dal;
namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.IProduct
/// </summary>
internal class Product : IProduct
{
    private DalApi.IDal? dal = DalApi.Factory.Get(); //new DalList();

    public IEnumerable<BO.ProductForList?> RequestProductsByCondition(IEnumerable<BO.ProductForList?> productForLists, Func<BO.ProductForList?, bool>? func) => productForLists.Where(func!);

    /// <summary>
    /// makes a product's list based on the requested Dal data
    /// </summary>
    /// <returns>
    /// list of products : type Ienumerable
    /// </returns>
    ///
    public IEnumerable<BO.ProductForList?> RequestProducts() => dal?.Product.GetList().CopyPropToList<DO.Product?, BO.ProductForList>()?? throw new BO.UnexpectedException();

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

                //if (dal.Product.Get(productID) is DO.Product product)
                //{
                //    //BO.Product retProduct = new BO.Product();

                //    //PropertyCopier<DO.Product, BO.Product>.Copy(product, retProduct); // bonus

                //    //return retProduct;
                //    return product.CopyPropTo(new BO.Product());
                //}
                //throw new BO.NotFoundInDalException("Product");
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
                cart.ListOfItems ??= new List<BO.OrderItem?>();
                int amountInCart = 0;

                BO.OrderItem? orderItem = cart.ListOfItems.FirstOrDefault(item => item!.ProductID == productID);
                if (orderItem != null)
                    amountInCart = orderItem.Amount;

                if (dal?.Product.Get(productID) is DO.Product product)
                {
                    // BO.ProductItem retProduct = new BO.ProductItem();
                    //PropertyCopier<DO.Product, BO.ProductItem>.Copy(product, retProduct); // bonus

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
            //DO.Product product1 = new DO.Product();
            //PropertyCopier<BO.Product, DO.Product>.Copy(product, product1);@@2

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
            if ( (bool)(dal?.OrderItem.GetList(orderItem => orderItem?.ProductID == productID).Any()!) )
                throw new BO.RemoveProductThatIsInOrdersException();

            try { dal.Product.Delete(productID); }
            catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    }
    /*
        public void RemoveProductAdmin(int productID)
    {
        if (productID >= 100000)
        {
            IEnumerable<DO.OrderItem?> orderItems = dal.OrderItem.GetList(orderItem => orderItem?.ProductID == productID);
            if (orderItems.Any())
            {
                foreach (var item in orderItems)
                {
                    var orderItem = item.CopyPropToStruct<DO.OrderItem?, DO.OrderItem>(new DO.OrderItem());
                    if (dal.Order.Get(orderItem.OrderID).CopyPropToStruct<DO.Order?, DO.Order>(new DO.Order()).DeliveryDate != null)
                    {
                        throw new BO.RemoveProductThatIsInOrdersException();
                    }
                }
            }
            try { dal.Product.Delete(productID); }
            catch (DO.NotFoundException ex) { throw new BO.NotFoundInDalException("Product", ex); }
        }
        else throw new BO.InvalidDataException("Product");
    } 
    */

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

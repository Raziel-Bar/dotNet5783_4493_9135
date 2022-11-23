using BlApi;
using Dal;

namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.IProduct
/// </summary>
internal class Product : IProduct
{
    private DalApi.IDal dal = new DalList();

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
            catch (DO.NotFoundException massege) // חריגה שתתקבל משורה 31
            {
                throw;
            }

        }
        else
            throw new BO.InvalidDataException("Product");// ??  @@@@ נראה לך שזו החריגה המתאימה
    }

    public BO.ProductItem RequestProductDetailsCart(int productID, BO.Cart cart)
    {
        if (productID >= 100000)
        {
            try
            {
                DO.Product product = dal.Product.Get(productID);

                BO.OrderItem? orderItem = cart.ListOfItems.Find(item => item.ProductID == productID);

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
            catch (DO.NotFoundException massege)// משכבת הנתונים
            {
                throw (massege);
            }
        }
        else
            throw new BO.InvalidDataException("Product");
    }

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
            catch (DO.AlreadyExistException massege)
            {

                throw massege;
            }
        }
        else
            throw new BO.InvalidDataException("Product");
    }

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
            catch (DO.NotFoundException massege)
            {
                throw massege;
            }
        }
        else
            throw new BO.InvalidDataException("Product");
    }

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
            catch (Exception)
            {

                throw;
            }
        }
        else
            throw new BO.InvalidDataException("Product");
    }
}

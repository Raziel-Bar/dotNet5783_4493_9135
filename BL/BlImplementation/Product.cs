using BlApi;
//using BO;
//using BO;
//using BO; //  רשום שאסורe 1 בדף הוראות 2 עמוד 10 סעיף 
using Dal;
//using DO;

namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.IProduct
/// </summary>
internal class Product : IProduct
{
    private DalApi.IDal dal = new DalList(); // new DalList for each implementation!? won't it make multiple databases!?

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
    public BO.Product RequestProductDetailsAdmin(int productID)// התחלתי עם מימוש הפונקצי הזו
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
            throw new BO.InvalidDataException("RequestProductDetailsAdmin");// ??  @@@@ נראה לך שזו החריגה המתאימה
    }

    public BO.ProductItem RequestProductDetailsCart(int productID, BO.Cart cart)
    {
        if (productID >= 100000)
        {
            try
            {
                DO.Product product = dal.Product.Get(productID);

                int amount = 0;
                foreach (var item in cart.ListOfItems)
                    if (item.ProductID == productID)
                        amount++;

                if (amount == 0)
                    throw new BO.ProductNotFoundInCartException();

                BO.ProductItem productItem = new BO.ProductItem
                {
                    ID = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                };

                productItem.Amount = amount;

                if (product.InStock - amount >= 0)
                {
                    productItem.Available = BO.Available.Available;
                    product.InStock = product.InStock - amount;
                    dal.Product.Update(product);
                }
                else
                    productItem.Available = BO.Available.Unavailable;

                return productItem;
            }
            catch (DO.NotFoundException massege)// משכבת הנתונים
            {
                throw (massege);
            }
        }
        else
            throw new BO.InvalidDataException(" RequestProductDetailsCart");// ??  @@@@ נראה לך שזו החריגה המתאימה
    }

    public void AddProductAdmin(BO.Product product)
    {
        if (product.ID >= 100000 && product.InStock >= 0 && product.Price > 0 && product.Name != null)
        {
            try
            {
                DO.Product product1 = new DO.Product
                {
                    ID = product.ID,
                    Name = product.Name,
                    InStock = product.InStock,
                    Category = (DO.WINERYS)product.Category,
                    Price = product.Price,
                };
                dal.Product.Add(product1);
            }
            catch (DO.AlreadyExistException massege)
            {

                throw massege;
            }
        }
        else

            throw new BO.InvalidDataException("AddProductAdmin");
    }

    public void RemoveProductAdmin(int productID)
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

    public void UpdateProductAdmin(BO.Product product)
    {
        if (product.ID >= 100000 && product.InStock >= 0 && product.Price > 0 && product.Name != null)
        {
            try
            {
                DO.Product dataProduct = dal.Product.Get(product.ID);
                dal.Product.Update(dataProduct);
            }
            catch (Exception)
            {

                throw;
            }
        }

        throw new BO.InvalidDataException("UpdateProductAdmin");

    }
}

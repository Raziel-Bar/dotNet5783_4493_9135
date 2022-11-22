using BlApi;
//using BO; //  רשום שאסורe 1 בדף הוראות 2 עמוד 10 סעיף 
using Dal;
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
        if (productID > 0 || productID < 100000)
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
            catch (DO.NotFoundException massege) // חריגה שתתקבל משורה 33
            {

                throw;
            }
        }
        else
            throw new InvalidDataException();// ??  @@@@ נראה לך שזו החריגה המתאימה
    }
    public BO.Product RequestProductDetailsCart(int productID, BO.Cart cart)
    {
        throw new NotImplementedException();
    }
    public void AddProductAdmin(BO.Product product)
    {
        throw new NotImplementedException();
    }
    public void RemoveProductAdmin(int productID)
    {
        throw new NotImplementedException();
    }
    public void UpdateProductAdmin(BO.Product product)
    {
        throw new NotImplementedException();
    }
}

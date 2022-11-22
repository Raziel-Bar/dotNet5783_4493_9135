using BlApi;
using BO;
using Dal;
namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.IProduct
/// </summary>
public class Product : IProduct
{
    private DalApi.IDal dal = new DalList(); // new DalList for each implementation!? won't it make multiple databases!?

    public IEnumerable<ProductForList> RequestProducts()
    {
        IEnumerable<DO.Product> products = dal.Product.GetList();

        return products.Select(product => new ProductForList
        {
            ID = product.ID,
            Name = product.Name,
            Category = (WINERYS)product.Category,
            Price = product.Price,
        });
    }
    public BO.Product RequestProductDetailsAdmin(int productID)
    {
        throw new NotImplementedException();
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

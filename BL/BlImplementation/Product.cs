using BlApi;
using BO;
using Dal;

namespace BlImplementation;

public class Product : IProduct
{
    private DalApi.IDal dal = new DalList();
    public IEnumerable<ProductForList> GetProducts()
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



 



}

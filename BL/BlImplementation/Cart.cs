
using BlApi;
using BO;
using Dal;

namespace BlImplementation;

public class Cart : ICart
{
    private DalApi.IDal dal = new DalList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idProduct"></param>
    /// <param name="cart"></param>
    public void AddToCart(int productID, BO.Cart cart)
    {
        DO.Product product = dal.Product.Get(productID);

        if (product.InStock > 0)
        {
            OrderItem orderItem = cart.ListOfItems.First(orderItem => orderItem.OrderItemID == product.ID);

            if (orderItem is null)
            {
                cart.ListOfItems.Add(new BO.OrderItem
                {
                    ProductID = product.ID,
                    PricePerUnit = product.Price,
                    ProductName = product.Name,
                    Amount = 1
                });
            }
            else
                orderItem.Amount += 1;

            cart.TotalPrice += product.Price;
        }

    }
}

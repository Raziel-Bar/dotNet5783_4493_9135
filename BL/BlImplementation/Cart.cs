
using BlApi;
//using BO;//  רשום שאסורe 1 בדף הוראות 2 עמוד 10 סעיף 
using Dal;

namespace BlImplementation;

internal class Cart : ICart
{
    private DalApi.IDal dal = new DalList();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idProduct"></param>
    /// <param name="cart"></param>
   
    void ICart.AddProductToCart(int productID, BO.Cart cart)
    {
        DO.Product product = dal.Product.Get(productID);

        if (product.InStock > 0)
        {
            BO.OrderItem orderItem = cart.ListOfItems.First(orderItem => orderItem.OrderItemID == product.ID);

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

    void ICart.ConfirmOrder(BO.Cart cart, string name, string email, string address)
    {
        throw new NotImplementedException();
    }

    void ICart.UpdateProductInCart(int productID, BO.Cart cart, int newAmount)
    {
        throw new NotImplementedException();
    }
}

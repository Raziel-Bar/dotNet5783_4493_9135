
using BlApi;
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
        try
        {
            DO.Product dataProduct = dal.Product.Get(productID);

            if (dataProduct.InStock > 0)
            {
                BO.OrderItem orderItem = cart.ListOfItems.First(orderItem => orderItem.OrderItemID == dataProduct.ID);

                if (orderItem is null)
                {
                    cart.ListOfItems.Add(new BO.OrderItem
                    {
                        ProductID = dataProduct.ID,
                        PricePerUnit = dataProduct.Price,
                        ProductName = dataProduct.Name,
                        Amount = 1
                    });
                }
                else
                    orderItem.Amount += 1;


                cart.TotalPrice += dataProduct.Price;
            }
            //else
              //  throw;
        }
        catch (Exception)
        {

            throw;
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

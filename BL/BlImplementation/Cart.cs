using BlApi;
using BO;
using Dal;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.ICart
/// </summary>
internal class Cart : ICart
{
    private DalApi.IDal? dal = DalApi.Factory.Get(); // new DalList();

    /// <summary>
    /// Adds a product to the cart if All conditions are met
    /// </summary>
    /// <param name="productID">
    /// The product that is to be added's ID
    /// </param>
    /// <param name="cart">
    /// The user's current cart
    /// </param>
    /// <returns>The new Updated cart</returns>
    /// <exception cref="BO.InvalidDataException">productID is invalid</exception>
    /// <exception cref="BO.NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the cart</exception>
    BO.Cart ICart.AddProductToCart(int productID, BO.Cart cart)
    {
        try
        {
            if (productID < 100000) throw new BO.InvalidDataException("Product"); // productID validity check
  

            if (dal?.Product.Get(productID) is DO.Product dataProduct)// product exist in dal check
            {
                cart.ListOfItems ??= new List<BO.OrderItem?>();

                BO.OrderItem? _orderItem = cart.ListOfItems.Find(item => item!.ProductID == dataProduct.ID);

                if (_orderItem is null) // adding the item for the 1st time
                {
                    if (dataProduct.InStock <= 0) throw new BO.StockNotEnoughtOrEmptyException();// stock amount check
                                                                                                 //else

                    cart.ListOfItems.Add(new BO.OrderItem
                    {
                        ProductID = dataProduct.ID,
                        Price = dataProduct.Price,
                        ProductName = dataProduct.Name,
                        Amount = 1,
                        TotalPrice = dataProduct.Price
                    });

                    cart.TotalPrice += dataProduct.Price;   // we only added the item once in any case
                }

                else // adding +1 to the item's amount
                {
                    if (dataProduct.InStock <= _orderItem.Amount)
                        throw new BO.StockNotEnoughtOrEmptyException(); // stock amount check

                    //else
                    cart.ListOfItems.Remove(cart.ListOfItems.First(item => item!.ProductID == dataProduct.ID)); // removing old item

                    _orderItem.Amount += 1;

                    //BO.OrderItem item = dataProduct.CopyPropTo(new BO.OrderItem());

                    //item.Amount = _orderItem.Amount;

                    //item.TotalPrice = _orderItem.Amount * dataProduct.Price;

                    cart.ListOfItems.Add(new BO.OrderItem
                    {
                        ProductID = dataProduct.ID,
                        Price = dataProduct.Price,
                        ProductName = dataProduct.Name,
                        Amount = _orderItem.Amount,
                        TotalPrice = _orderItem.Amount * dataProduct.Price
                    });

                    cart.TotalPrice -= _orderItem.TotalPrice; // we might have changed the product between addings so we erase old price
                    cart.TotalPrice += _orderItem.Amount * dataProduct.Price; // and add the new total price
                }
            }
            else throw new BO.UnexpectedException();

            return cart;
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Product", ex);
        }
    }

    /// <summary>
    /// Updates the amount of a product in the cart
    /// </summary>
    /// <param name="productID">
    /// The product that is to be added's ID
    /// </param>
    /// <param name="cart">
    /// The user's current cart
    /// </param>
    /// <param name="newAmount">
    /// The new product's amount in the cart
    /// </param>
    /// <returns>The new Updated cart</returns>
    /// <exception cref="BO.InvalidDataException">productID is invalid or the amount is negative</exception>
    /// <exception cref="BO.NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the cart</exception>
    /// <exception cref="BO.ProductNotFoundInCartException">If the product is not in the cart at all</exception>
    BO.Cart ICart.UpdateProductInCart(int productID, BO.Cart cart, int newAmount)
    {
        try
        {
            if (productID < 100000) throw new BO.InvalidDataException("Product"); // productID validity check

            if (newAmount < 0) throw new BO.InvalidDataException("amount"); // amount validity check

            cart.ListOfItems ??= new List<BO.OrderItem?>();

            if (dal?.Product.Get(productID)?.InStock < newAmount) throw new BO.StockNotEnoughtOrEmptyException(); // stock amount check

            //DO.Product dataProduct = dal.Product.Get(productID) ?? throw new BO.UnexpectedException(); // product exist in dal check

            //if (dataProduct.InStock < newAmount) throw new BO.StockNotEnoughtOrEmptyException(); // stock amount check

            BO.OrderItem? _orderItem = cart.ListOfItems.Find(item => item!.ProductID == productID);

            if (_orderItem is null) throw new BO.ProductNotFoundInCartException(); // product exist in cart check

            int difference = _orderItem.Amount - newAmount; // we save the difference in the amounts

            cart.ListOfItems.Remove(cart.ListOfItems.First(item => item!.ProductID == productID)); // removing old item

            _orderItem.Amount = newAmount; //setting new amount

            if (difference != 0) // changing amount in cart
            {
                // In case difference is positive => we decrease the amount in the cart, so we use -= regulary
                // In case it's negative => we INCREASE the amount in the cart, so using -= will actually ADD the new change in price because -(difference * <positive const>) > 0
                _orderItem.TotalPrice -= difference * _orderItem.Price;
                cart.TotalPrice -= difference * _orderItem.Price;
                cart.ListOfItems.Add(_orderItem);
            }
            else // removing the item entirely
            {
                cart.TotalPrice -= _orderItem.TotalPrice;
            }

            return cart;
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Product", ex);
        }
    }

    /// <summary>
    /// erases the cart
    /// </summary>
    /// <param name="cart">the current cart</param>
    public void ClearItems(BO.Cart cart)
    {
        cart.ListOfItems!.Clear();
        cart.TotalPrice = 0;
    }

    /// <summary>
    /// Makes an order once a cart is finished and updates all related data in the Dal
    /// </summary>
    /// <param name="cart">
    /// The user's cart
    /// </param>
    /// <param name="name">
    /// The user's name
    /// </param>
    /// <param name="email">
    /// The user's email address
    /// </param>
    /// <param name="address">
    /// The user's address
    /// </param>
    /// <exception cref="BO.NotFoundInDalException">If one of the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.StockNotEnoughtOrEmptyException">If one of the product's stock is empty so we can't add it to the cart</exception>
    /// <exception cref="BO.InvalidDataException">If one of the customer's or product's details is invalid</exception>
    /// 
    void ICart.ConfirmOrder(BO.Cart cart)
    {
        cart.ListOfItems ??= new List<BO.OrderItem?>();

        if (cart.ListOfItems.Count == 0)
            throw new BO.InvalidDataException("cart");

        bool changeInProductDetails = false;

        if (cart.CustomerName == null || cart.CustomerAddress == null || cart.CustomerEmail == null ||
            !new EmailAddressAttribute().IsValid(cart.CustomerEmail)) throw new BO.InvalidDataException("Customer");

        //customer's details check
        DO.Product dataProduct;
        try
        {
            foreach (var item in cart.ListOfItems) // products exist in dal and stock check
            {
                dataProduct = dal?.Product.Get(item!.ProductID) ?? throw new BO.UnexpectedException();

                if (dataProduct.InStock < item.Amount) throw new BO.StockNotEnoughtOrEmptyException();

                if (dataProduct.Name != item.ProductName || dataProduct.Price != item.Price)
                {
                    item.ProductName = dataProduct.Name;
                    item.Price = dataProduct.Price!;
                    item.TotalPrice = item.Amount * item.TotalPrice;
                    changeInProductDetails = true;
                }

            }
        }

        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Product", ex);
        }

        if (changeInProductDetails)
        {
            throw new BO.ChangeInCartItemsDetailsException();
        }

        //DO.Order order = cart.CopyPropTo(new DO.Order());

        //order.OrderDate = DateTime.Now;

        DO.Order order = new DO.Order
        {              // making a new Order for the Dal
            CustomerAddress = cart.CustomerAddress,
            CustomerEmail = cart.CustomerEmail,
            CustomerName = cart.CustomerName,
            OrderDate = DateTime.Now,
            DeliveryDate = null,
            ShipDate = null
        };

        int id = dal?.Order.Add(order)?? throw new BO.UnexpectedException();

        foreach (var item in cart.ListOfItems)      // making new OrderItems for the Dal and updating DalProducts' stocks
        {
            item!.OrderItemID = id;
            DO.OrderItem orderItem = item.CopyPropToStruct(new DO.OrderItem());
            orderItem.OrderID = id;
            dal.OrderItem.Add(orderItem);

            dataProduct = dal.Product.Get(item!.ProductID) ?? throw new UnexpectedException(); // stock update

            dataProduct.InStock -= item.Amount;

            dal.Product.Update(dataProduct);  
        }
        ClearItems(cart);
    }
}

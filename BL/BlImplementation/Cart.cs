﻿using BlApi;
using Dal;
namespace BlImplementation;

/// <summary>
/// Implementation for BlApi.ICart
/// </summary>
internal class Cart : ICart
{
    private DalApi.IDal dal = new DalList();

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
            DO.Product dataProduct = dal.Product.Get(productID); // product exist in dal check

            BO.OrderItem _orderItem = cart.ListOfItems.First(item => item.ProductID == dataProduct.ID);

            if (_orderItem is null) // adding the item for the 1st time
            {
                if (dataProduct.InStock <= 0) throw new BO.StockNotEnoughtOrEmptyException();// stock amount check
                //else
                cart.ListOfItems.Add(new BO.OrderItem
                {
                    ProductID = dataProduct.ID,
                    PricePerUnit = dataProduct.Price,
                    ProductName = dataProduct.Name,
                    Amount = 1,
                    TotalPrice = dataProduct.Price
                });
            }
            else // adding +1 to the item's amount
            {
                if (dataProduct.InStock <= _orderItem.Amount) throw new BO.StockNotEnoughtOrEmptyException(); // stock amount check
                //else
                cart.ListOfItems.Remove(cart.ListOfItems.First(item => item.ProductID == dataProduct.ID)); // removing old item
                _orderItem.Amount += 1;
                _orderItem.TotalPrice += dataProduct.Price;
                cart.ListOfItems.Add(_orderItem); // adding updated item
            }
            cart.TotalPrice += dataProduct.Price; // we only added the item once in any case
            return cart;
        }
        catch (BO.InvalidDataException ex)
        {
            throw ex;
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Product", ex);
        }
        catch (BO.StockNotEnoughtOrEmptyException ex)
        {
            throw ex;
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
    /// <exception cref="BO.InvalidDataException">productID is invalid</exception>
    /// <exception cref="BO.NotFoundInDalException">If the Product doesn't exist in the Dal</exception>
    /// <exception cref="BO.StockNotEnoughtOrEmptyException">If the product's stock is empty so we can't add it to the cart</exception>
    /// <exception cref="BO.ProductNotFoundInCartException">If the product is not in the cart at all</exception>
    BO.Cart ICart.UpdateProductInCart(int productID, BO.Cart cart, int newAmount)
    {
        try
        {
            if (productID < 100000) throw new BO.InvalidDataException("Product"); // productID validity check
            DO.Product dataProduct = dal.Product.Get(productID); // product exist in dal check
            if (dataProduct.InStock < newAmount) throw new BO.StockNotEnoughtOrEmptyException(); // stock amount check

            BO.OrderItem _orderItem = cart.ListOfItems.First(item => item.ProductID == productID);
            if (_orderItem is null) throw new BO.ProductNotFoundInCartException(); // product exist in cart check

            int difference = _orderItem.Amount - newAmount; // we save the difference in the amounts
            cart.ListOfItems.Remove(cart.ListOfItems.First(item => item.ProductID == productID)); // removing old item
            _orderItem.Amount = newAmount; //setting new amount

            if (difference != 0) // changing amount in cart
            {
                // In case difference is positive => we decrease the amount in the cart, so we use -= regulary
                // In case it's negative => we INCREASE the amount in the cart, so using -= will actually ADD the new change in price because -(difference * <positive const>) > 0
                _orderItem.TotalPrice -= difference * _orderItem.PricePerUnit;
                cart.TotalPrice -= difference * _orderItem.PricePerUnit;
                cart.ListOfItems.Add(_orderItem);
            }
            else // removing the item entirely
            {
                cart.TotalPrice -= _orderItem.TotalPrice;
            }

            return cart;
        }
        catch (BO.InvalidDataException ex)
        {
            throw ex;
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Product", ex);
        }
        catch (BO.ProductNotFoundInCartException ex)
        {
            throw ex;
        }
        catch (BO.StockNotEnoughtOrEmptyException ex)
        {
            throw ex;
        }
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
    void ICart.ConfirmOrder(BO.Cart cart)
    {
        if (cart.CustomerName == null || cart.CustomerAdress == null || cart.CustomerEmail == null) throw new BO.InvalidDataException("Customer"); //customer's details check   //mail format not written anywhere...
        DO.Product dataProduct;
        try
        {
            foreach (var item in cart.ListOfItems) // products exist in dal and stock check
            {
                dataProduct = dal.Product.Get(item.ProductID);
                if (dataProduct.InStock < item.Amount) throw new BO.StockNotEnoughtOrEmptyException();
            }
        }
        catch (DO.NotFoundException ex)
        {
            throw new BO.NotFoundInDalException("Product", ex);
        }
        catch (BO.StockNotEnoughtOrEmptyException ex)
        {
            throw ex;
        }
        DO.Order order = new DO.Order{              // making a new Order for the Dal
            CustomerAdress = cart.CustomerAdress,
            CustomerEmail = cart.CustomerEmail,
            CustomerName = cart.CustomerName,
            OrderDate = DateTime.Now,
            DeliveryDate = null,
            ShipDate = null};
        int id = dal.Order.Add(order);


        foreach (var item in cart.ListOfItems)      // making new OrderItems for the Dal and updating DalProducts' stocks
        {
            dataProduct = dal.Product.Get(item.ProductID); // stock update
            dataProduct.InStock -= item.Amount;
            dal.Product.Update(dataProduct);

            DO.OrderItem orderItem = new DO.OrderItem { // new OrderItems make
            ProductID = item.ProductID,
            OrderID = id,
            Price = item.PricePerUnit,
            Amount = item.Amount
            };
            dal.OrderItem.Add(orderItem);
        }
    }
}
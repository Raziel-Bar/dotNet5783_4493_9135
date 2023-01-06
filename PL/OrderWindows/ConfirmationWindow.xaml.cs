using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PL.ProductWindows;
using BlApi;

namespace PL.OrderWindows;

/// <summary>
/// This class represents the window for confirming an order.
/// </summary>
public partial class ConfirmationWindow : Window
{
    /// <summary>
    /// This field stores a reference to the business logic layer.
    /// </summary>
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// This is a dependency property that represents the cart to be confirmed.
    /// </summary>
    public Cart ConfirmCart
    {
        get { return (Cart)GetValue(ConfirmCartProperty); }
        set { SetValue(ConfirmCartProperty, value); }
    }

    /// <summary>
    /// This is the backing store for the ConfirmCart dependency property.
    /// </summary>
    public static readonly DependencyProperty ConfirmCartProperty =
        DependencyProperty.Register("ConfirmCart", typeof(Cart), typeof(ConfirmationWindow));

    /// <summary>
    /// This is the constructor for the ConfirmationWindow class.
    /// It initializes the ConfirmCart property and the window's UI.
    /// </summary>
    /// <param name="cart">The cart to be confirmed.</param>
    public ConfirmationWindow(Cart cart)
    {
        ConfirmCart = cart;
        InitializeComponent();
    }

    /// <summary>
    /// This event handler is called when the "Back to Orders" button is clicked.
    /// It opens the NewOrderWindow and closes the ConfirmationWindow.
    /// </summary>
    private void BackToOrders(object sender, RoutedEventArgs e)
    {
        new NewOrderWindow(ConfirmCart).Show();
        this.Close();
    }

    /// <summary>
    /// This event handler is called when the "Confirm" button is clicked.
    /// It attempts to confirm the order by calling the ConfirmOrder method on the business logic layer.
    /// If the method throws an exception, it shows an error message to the user.
    /// If the method succeeds, it shows a success message to the user and opens the MainWindow.
    /// </summary>
    private void FinalConfirm(object sender, RoutedEventArgs e)
    {
        try
        {
            bl!.Cart.ConfirmOrder(ConfirmCart);
            new SuccessWindow("Done! Your Order has been confirmed!\nHave a splendid day!!").ShowDialog();
            new MainWindow().Show();
            this.Close();
        }
        catch (ChangeInCartItemsDetailsException) { new ErrorMessageWindow("Oops", "Our product's list has changed! Recheck your cart!").ShowDialog(); }
        catch (StockNotEnoughtOrEmptyException) { new ErrorMessageWindow("Oops", "Seems Like one of your ordered Products has ran out of stock. Recheck your cart!").ShowDialog(); }
        catch (NotFoundInDalException) { new ErrorMessageWindow("Oops", "Seems Like one of your ordered Products does not exist. Recheck your cart!").ShowDialog(); }
        catch (InvalidDataException) { new ErrorMessageWindow("Oops", "Is it possible that your cart's empty?....").ShowDialog(); }
        catch (Exception ex) { new ErrorMessageWindow("UNEXPECTED ERROR", ex.Message).ShowDialog(); }
    }
}

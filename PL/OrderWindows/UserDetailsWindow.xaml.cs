using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for UserDetailsWindow.xaml
/// </summary>
public partial class UserDetailsWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    public Cart cart
    {
        get { return (Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Product.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty cartProperty =
        DependencyProperty.Register("cart", typeof(Cart), typeof(ConfirmationWindow));
    public UserDetailsWindow(BO.Cart _cart)
    {
        cart = _cart;
        InitializeComponent();
    }

    private void Confirm(object sender, RoutedEventArgs e)
    {
        if (cart.CustomerName == null || cart.CustomerAddress == null || cart.CustomerEmail == null || !new EmailAddressAttribute().IsValid(cart.CustomerEmail))
        {
            new ErrorMessageWindow("Invalid Data", "Make sure to fill all fields and have a valid Email Adress!").ShowDialog();
        }
        else
        {
            new ConfirmationWindow(cart).Show();
            this.Close();
        }
    }

    private void BackToPreviousWindow(object sender, RoutedEventArgs e)
    {
        new NewOrderWindow(cart).Show();
        this.Close();
    }
}

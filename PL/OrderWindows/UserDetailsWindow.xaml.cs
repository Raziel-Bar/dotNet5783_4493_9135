using BO;
using System;
using System.Collections.Generic;
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
using BO;

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
        DependencyProperty.Register("cart", typeof(Cart), typeof(CartWindow));
    public UserDetailsWindow()
    {
        InitializeComponent();
    }
}

using BO;
using System.Windows;

namespace PL.OrderWindows;
/// <summary>
/// Interaction logic for the CartDetails window
/// </summary>
public partial class CartDetails : Window
{
    /// <summary>
    /// The cartDetails property is a dependency property that represents the cart to display in the window
    /// </summary>
    public Cart cartDetails
    {
        get { return (Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty cartProperty =
        DependencyProperty.Register("cartDetails", typeof(Cart), typeof(CartDetails));

    /// <summary>
    /// The CartDetails constructor initializes the window and sets the cartDetails property
    /// </summary>
    /// <param name="_cart">The cart to display in the window</param>
    public CartDetails(Cart _cart)
    {
        cartDetails = _cart;
        InitializeComponent();
    }

    /// <summary>
    /// The BackToOrderTracking_Button_Click method closes the window when the button is clicked
    /// </summary>
    /// <param name="sender">The button that was clicked</param>
    /// <param name="e">The event arguments</param>
    private void BackToOrderTracking_Button_Click(object sender, RoutedEventArgs e) => this.Close();
    
}


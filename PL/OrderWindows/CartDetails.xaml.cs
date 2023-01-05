using BO;
using System.Windows;

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for CartDetails.xaml
    /// </summary>
    public partial class CartDetails : Window
    {


        public Cart cartDetails
        {
            get { return (Cart)GetValue(cartProperty); }
            set { SetValue(cartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cartProperty =
            DependencyProperty.Register("cartDetails", typeof(Cart), typeof(CartDetails));

        public CartDetails(Cart _cart)
        {
            cartDetails = _cart;
            InitializeComponent();
        }

        private void BackToOrderTracking_Button_Click(object sender, RoutedEventArgs e) => this.Close();


    }
}

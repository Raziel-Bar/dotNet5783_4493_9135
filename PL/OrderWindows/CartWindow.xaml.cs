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

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {

        public Cart cart
        {
            get { return (Cart)GetValue(cartProperty); }
            set { SetValue(cartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Product.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty cartProperty =
            DependencyProperty.Register("cart", typeof(Cart), typeof(CartWindow));
       

        public CartWindow(Cart Ecart)
        {
            cart= Ecart;    
            InitializeComponent();
        }

        private void BackToOrders(object sender, RoutedEventArgs e) => this.Close();
    }
}

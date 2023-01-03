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
using PL.OrderWindows;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductDetailsUserWindow.xaml
    /// </summary>
    public partial class ProductDetailsUserWindow : Window
    {
        public ProductItem Product 
        {
            get { return (ProductItem)GetValue(ProductProperty); }
            set { SetValue(ProductProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Product.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductProperty =
            DependencyProperty.Register("Product", typeof(ProductItem), typeof(ProductDetailsUserWindow));



        Cart cart;
        BlApi.IBl blP;
        public ProductDetailsUserWindow(BlApi.IBl bl, int id)
        {
            blP = bl;
            Product = blP.Product.RequestProductDetailsUser(id,cart);
            InitializeComponent();
        }

        private void BackToOrders(object sender, RoutedEventArgs e)
        {
            new NewOrderWindow().Show();
            this.Close();
        }
    }
}

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
namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductDetailsUserWindow.xaml
    /// </summary>
    public partial class ProductDetailsUserWindow : Window
    {
        ProductItem product;
        Cart cart;
        BlApi.IBl blP;
        public ProductDetailsUserWindow(BlApi.IBl bl, int id)
        {
            blP = bl;
            product =  
            InitializeComponent();
        }
    }
}

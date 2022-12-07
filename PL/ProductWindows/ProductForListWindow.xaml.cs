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
using BlApi;
using BlImplementation;




namespace PL.ProductWindows
{
    public enum WINERYS
    {
        GOLAN,
        DALTON ,
        TEPERBERG,
        CARMEL,
        BARKAN,
        ALL,
    }

    /// <summary>
    /// Interaction logic for ProductForListWindow.xaml
    /// </summary>
    public partial class ProductForListWindow : Window
    {
        IBl bl = new Bl();
        IEnumerable<ProductForList?> productForLists;
        public ProductForListWindow()
        {

            InitializeComponent();

            productForLists = bl.Product.RequestProducts();

            WinesListView.ItemsSource = productForLists;

            WinerySelector.ItemsSource = Enum.GetValues(typeof(WINERYS)); // changed instructions to enable clearing the category
         
        }


        private void WinerySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( (WINERYS) WinerySelector.SelectedItem == WINERYS.ALL ) WinesListView.ItemsSource = productForLists;

            else WinesListView.ItemsSource = bl.Product.RequestProductsByCondition(productForLists, product => product?.Category == (BO.WINERYS)WinerySelector.SelectedItem);
        }

        //private void WinesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
       //  SelectionChanged="WinesListView_SelectionChanged"
        //}

        private void ToProductWindowAddMode(object sender, RoutedEventArgs e) => new ProductWindow().Show();
       
    }
}

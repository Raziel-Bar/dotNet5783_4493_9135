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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{



    public class Product
    {
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(MainWindow));

        public List<Product> Products { get; set; } = new List<Product>();
        public Product Product { get; set; }

        // public string Name { get; set; } = "abc";
        public MainWindow()
        {
            Product = new Product { Price = 670 };
            Name = "abc";
            DataContext = this;
            Products.Add(Product);
            InitializeComponent();
           // ProductsLV.ItemsSource = Products; 
        }



        private void CB_Checked(object sender, RoutedEventArgs e)
        {
            BU.Visibility = CB.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
        }

        private void CB_Checked1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Product.Price.ToString());
            Product.Price = 500;
            //MessageBox.Show(NameTB.Text.ToString());
           
            //MessageBox.Show(Product.Price.ToString());
        }
    }
}

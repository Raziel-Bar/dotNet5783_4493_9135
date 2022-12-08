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
using System.Windows.Controls.Primitives;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        IBl bl = new Bl();

        public ProductWindow(string state, int? id = null)
        {
            InitializeComponent();
            CategoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.WINERYS));
            if (state == "ADD")
            {
                UpdateButton.Visibility = Visibility.Collapsed;
                IdTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                AddButton.Visibility = Visibility.Collapsed;
                IdTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void AddProductEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Product.AddProductAdmin(new BO.Product
                {
                    ID = int.Parse(IdTextBox.Text),
                    Category = (BO.WINERYS)CategoryComboBox.SelectedItem,
                    Name = NameTextBox.Text,
                    Price = int.Parse(PriceTextBox.Text),
                    InStock = int.Parse(AmountTextBox.Text)
                });
                new ProductForListWindow().Show();
                new SuccessWindow("Your Product Has been added successfully!").Show();
                this.Close();
            }
            catch (FormatException)
            {
                new ErrorMessageWindow("Invalid input!\nSome of the textboxes are either empty or contain illegal input.").Show();
            }
            catch (Exception ex) // to check if we need to split catches by exceptions or not..
            {
                new ErrorMessageWindow(ex.Message).Show();
            }
        }

        private void UpdateProductEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Product.UpdateProductAdmin(new BO.Product
                {
                    ID = int.Parse(IdTextBlock.Text),
                    Category = (BO.WINERYS)CategoryComboBox.SelectedItem,
                    Name = NameTextBox.Text,
                    Price = int.Parse(PriceTextBox.Text),
                    InStock = int.Parse(AmountTextBox.Text)
                });
                new ProductForListWindow().Show();
                new SuccessWindow("Your Product Has been added successfully!").Show();
                this.Close();
            }
            catch (FormatException)
            {
                new ErrorMessageWindow("Invalid input!\nSome of the textboxes are either empty or contain illegal input.").Show();
            }
            catch (Exception ex) // to check if we need to split catches by exceptions or not..
            {
                new ErrorMessageWindow(ex.Message).Show();
            }
        }
    }
}

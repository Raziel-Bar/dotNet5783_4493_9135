using BlApi;
using BlImplementation;
using BO;
using System;
using System.Windows;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        IBl bl = new Bl();

        public ProductWindow(string state, Product product = null!)
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

                IdTextBlock.Text = product.ID.ToString();
                NameTextBox.Text = product.Name;
                CategoryComboBox.Text = product.Category.ToString();
                PriceTextBox.Text = product.Price.ToString();
                AmountTextBox.Text = product.InStock.ToString();
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
                    Price = double.Parse(PriceTextBox.Text),
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
                    Price = double.Parse(PriceTextBox.Text),
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

        private void BackToMainWindow(object sender, RoutedEventArgs e)
        {
            new ProductForListWindow().Show();
            this.Close();
        }

        private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void NameTextBox_TextChanged_1(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}

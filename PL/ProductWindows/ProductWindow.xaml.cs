using BO;
using DO;
using System;
using System.Windows;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get(); //new Bl();

        /// <summary>
        /// a window for either adding or updating a product
        /// </summary>
        /// <param name="state">the state of the current window (either ADD mode or UPDATE mode)</param>
        /// <param name="product">In case of UPDATE MODE: the selected product that we wish to update its details</param>
        public ProductWindow(int id = 0)
        {
            InitializeComponent();

            CategoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.WINERYS));

            if (id == 0)
            {
                UpdateButton.Visibility = Visibility.Collapsed;
                IdTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                AddButton.Visibility = Visibility.Collapsed;
                IdTextBox.Visibility = Visibility.Collapsed; // ID can't be changed

                // all old details are shown
                try
                {
                    BO.Product product = bl?.Product.RequestProductDetailsAdmin(id) ?? throw new BO.UnexpectedException();

                    IdTextBlock.Text = product.ID.ToString();
                    NameTextBox.Text = product.Name;
                    CategoryComboBox.Text = product.Category.ToString();
                    PriceTextBox.Text = product.Price.ToString();
                    AmountTextBox.Text = product.InStock.ToString();
                }
                catch(Exception ex) 
                {
                    new ErrorMessageWindow("Unexpected Error!", ex.Message).Show();
                }
               
            }
        }

        /// <summary>
        /// the add product event
        /// </summary>
        /// <param name="sender">the add button</param>
        /// <param name="e">mouse click</param>
        private void AddProductEvent(object sender, RoutedEventArgs e)
        {
            try
            {

                bl?.Product.AddProductAdmin(new BO.Product
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
            catch (FormatException) // we do not allow empty boxes or illegal input
            {
                new ErrorMessageWindow("Input Error", "Invalid input!\nSome of the textboxes are either empty or contain wrong format input.").Show();
            }
            catch (BO.InvalidDataException ex) 
            {
                new ErrorMessageWindow("Invalid Data", ex.Message).Show();
            }
            catch (BO.AlreadyExistInDalException ex)
            {
                new ErrorMessageWindow("Existing Data Error", ex.Message).Show();
            }
            catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
            {
                new ErrorMessageWindow("Unexpected Error!", ex.Message).Show();
            }
        }

        /// <summary>
        /// the update product event
        /// </summary>
        /// <param name="sender">the update button</param>
        /// <param name="e">mouse click</param>
        private void UpdateProductEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                bl?.Product.UpdateProductAdmin(new BO.Product
                {
                    ID = int.Parse(IdTextBlock.Text),
                    Category = (BO.WINERYS)CategoryComboBox.SelectedItem,
                    Name = NameTextBox.Text,
                    Price = double.Parse(PriceTextBox.Text),
                    InStock = int.Parse(AmountTextBox.Text)
                });
                new ProductForListWindow().Show();
                new SuccessWindow("Your Product Has been Updated successfully!").Show();
                this.Close();
            }
            catch (FormatException) // we do not allow empty boxes or illegal input
            {
                new ErrorMessageWindow("Input Error", "Invalid input!\nSome of the textboxes are either empty or contain wrong format input.").Show();
            }
            catch (BO.InvalidDataException ex)
            {
                new ErrorMessageWindow("Invalid Data", ex.Message).Show();
            }
            catch (BO.NotFoundInDalException ex)
            {
                new ErrorMessageWindow("Data Not Found", ex.Message).Show();
            }
            catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
            {
                new ErrorMessageWindow("Unexpected Error!", ex.Message).Show();
            }
        }

        /// <summary>
        /// going back to ProducrFor list window
        /// </summary>
        /// <param name="sender">the back button</param>
        /// <param name="e">mouse click</param>
        private void BackToMainWindow(object sender, RoutedEventArgs e)
        {
            new ProductForListWindow().Show();
            this.Close();
        }


    }
}

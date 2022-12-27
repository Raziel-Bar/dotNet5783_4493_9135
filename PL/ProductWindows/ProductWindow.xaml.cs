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
        /// <param name="id">The product's ID in case the window is in UPDATE mode. otherwise it's 0 which indicates the window is in ADD mode</param>
        public ProductWindow(int id = 0)
        {
            InitializeComponent();

            CategoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.WINERYS));

            if (id == 0) // add mode
            {
                this.Title = "Add";
                UpdateButton.Visibility = Visibility.Collapsed;
                IdTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Title = "Update";
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
                new SuccessWindow("Your Product Has been added successfully!").ShowDialog();
                this.Close();
            }
            catch (FormatException) // we do not allow empty boxes or illegal input
            {
                new ErrorMessageWindow("Input Error", "Invalid input!\nSome of the textboxes are either empty or contain wrong format input.").ShowDialog();
            }
            catch (BO.InvalidDataException ex) 
            {
                new ErrorMessageWindow("Invalid Data", ex.Message).ShowDialog();
            }
            catch (BO.AlreadyExistInDalException ex)
            {
                new ErrorMessageWindow("Existing Data Error", ex.Message).ShowDialog();
            }
            catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
            {
                //MessageBox.Show(ex.Message);
                new ErrorMessageWindow("Unexpected Error!", ex.Message).ShowDialog();
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
                new SuccessWindow("Your Product Has been Updated successfully!").ShowDialog();
                this.Close();
            }
            catch (FormatException) // we do not allow empty boxes or illegal input
            {
                new ErrorMessageWindow("Input Error", "Invalid input!\nSome of the textboxes are either empty or contain wrong format input.").ShowDialog();
            }
            catch (BO.InvalidDataException ex)
            {
                new ErrorMessageWindow("Invalid Data", ex.Message).ShowDialog();
            }
            catch (BO.NotFoundInDalException ex)
            {
                new ErrorMessageWindow("Data Not Found", ex.Message).ShowDialog();
            }
            catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
            {
                new ErrorMessageWindow("Unexpected Error!", ex.Message).ShowDialog();
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

using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.ProductWindows
{
    /// <summary>
    /// Interaction logic for ProductRemoveAdminWindow.xaml
    /// </summary>
    public partial class ProductRemoveAdminWindow : Window
    {
        readonly BlApi.IBl? bl = BlApi.Factory.Get();

        public ProductRemoveAdminWindow()
        {
            InitializeComponent();
        }

        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TextBox box = (TextBox)button.Tag;
            int id = Convert.ToInt32(box.Text);
            try
            {
                bl!.Product.RemoveProductAdmin(id);
                new SuccessWindow("Product has been removed successfully!").ShowDialog();
                new AdminWindow().Show();
                this.Close();
            }
            catch (RemoveProductThatIsInOrdersException) { new ErrorMessageWindow("Denied", "That Product exists in customers' orders thus cannot be removed at the moment!").ShowDialog(); }
            catch (NotFoundInDalException) { new ErrorMessageWindow("ERROR", "No Product with such ID exists").ShowDialog(); }
            catch (InvalidDataException) { new ErrorMessageWindow("Invalid Input", "ID must contain at least 6 digits!").ShowDialog(); }
            catch (Exception ex) { new ErrorMessageWindow("UNEXPECTED ERROR", ex.Message).ShowDialog(); }
        }

        private void IsIntPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
            this.Close();
        }
    }
}

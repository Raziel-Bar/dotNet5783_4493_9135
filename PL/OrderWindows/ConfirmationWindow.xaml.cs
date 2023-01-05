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
using BlApi;

namespace PL.OrderWindows
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        readonly BlApi.IBl? bl = BlApi.Factory.Get();
        public Cart ConfirmCart
        {
            get { return (Cart)GetValue(ConfirmCartProperty); }
            set { SetValue(ConfirmCartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Product.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmCartProperty =
            DependencyProperty.Register("ConfirmCart", typeof(Cart), typeof(ConfirmationWindow));


        public ConfirmationWindow(Cart Ecart)
        {
            ConfirmCart = Ecart;
            InitializeComponent();
        }

        private void BackToOrders(object sender, RoutedEventArgs e)
        {
            new NewOrderWindow(ConfirmCart).Show();
            this.Close();
        }

        private void FinalConfirm(object sender, RoutedEventArgs e)
        {
            try
            {
                bl!.Cart.ConfirmOrder(ConfirmCart);
                new SuccessWindow("Done! Your Order has been confirmed!\nHave a splendid day!!").ShowDialog();
                new MainWindow().Show();
                this.Close();
            }
            catch (ChangeInCartItemsDetailsException) { new ErrorMessageWindow("Oops", "Our product's list has changed! Recheck your cart!").ShowDialog(); }
            catch (StockNotEnoughtOrEmptyException) { new ErrorMessageWindow("Oops", "Seems Like one of your ordered Products has ran out of stock. Recheck your cart!").ShowDialog(); }
            catch (NotFoundInDalException) { new ErrorMessageWindow("Oops", "Seems Like one of your ordered Products does not exist. Recheck your cart!").ShowDialog(); }
            catch (InvalidDataException) { new ErrorMessageWindow("Oops", "Is it possible that your cart's empty?....").ShowDialog(); }
            catch (Exception ex) { new ErrorMessageWindow("UNEXPECTED ERROR", ex.Message).ShowDialog(); }
        }
    }
}

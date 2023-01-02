using BO;
using PL.ProductWindows;
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

namespace PL.OrderWindows
{
    public class OrderListAdminWindowData : DependencyObject
    {
        public IEnumerable<OrderForList>? Orders
        {
            get { return (IEnumerable<OrderForList>?)GetValue(ordersProperty); }
            set { SetValue(ordersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for orders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ordersProperty =
            DependencyProperty.Register("Orders", typeof(IEnumerable<OrderForList>), typeof(OrderListAdminWindowData));
    }

    /// <summary>
    /// Interaction logic for OrderListAdminWindow.xaml
    /// </summary>
    public partial class OrderListAdminWindow : Window
    {
        readonly BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(OrderListAdminWindowData), typeof(AdminWindow));
        public OrderListAdminWindowData Data { get => (OrderListAdminWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

        public OrderListAdminWindow()
        {
            Data = new() { Orders = bl.Order.RequestOrdersListAdmin()! };
            InitializeComponent();
        }
        private void BackToMainWindow(object sender, RoutedEventArgs e) => this.Close();

    }
}

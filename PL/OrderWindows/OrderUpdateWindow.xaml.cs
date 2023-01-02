using BlApi;
using System.Windows;
using System.Windows.Controls;

namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for OrderUpdateWindow.xaml
/// </summary>
public partial class OrderUpdateWindow : Window
{
    readonly BlApi.IBl _bl;
    // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderProperty =
        DependencyProperty.Register("Order1", typeof(BO.Order), typeof(OrderUpdateWindow));
    public BO.Order Order1
    {
        get { return (BO.Order)GetValue(OrderProperty); }
        set { SetValue(OrderProperty, value); }
    }


    public OrderUpdateWindow(BlApi.IBl bl, int id)
    {
        Order1 = bl.Order.RequestOrderDetails(id);
        _bl = bl;
        InitializeComponent();
    }

    private void BackToOrderList_Button_Click(object sender, RoutedEventArgs e)
    {
        new OrderListAdminWindow().Show();
        this.Close();
    }

    private void UpdateShipDate(object sender, RoutedEventArgs e)
    {
        Order1 = _bl.Order.UpdateOrderShipDateAdmin(Order1.ID);
        ((Button)sender).Visibility = Visibility.Collapsed;
    }

    private void UpdateDeliveryDate(object sender, RoutedEventArgs e)
    {
        if (Order1.ShipDate is not null )
        {
            Order1 = _bl.Order.UpdateOrderDeliveryDateAdmin(Order1.ID);
            ((Button)sender).Visibility = Visibility.Collapsed;
        }

    }
}
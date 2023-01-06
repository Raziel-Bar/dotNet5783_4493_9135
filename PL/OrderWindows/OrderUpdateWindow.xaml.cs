using BlApi;
using System.Windows;
using System.Windows.Controls;

namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for OrderUpdateWindow.xaml
/// </summary>
public partial class OrderUpdateWindow : Window
{
    // Reference to the business logic layer
    readonly BlApi.IBl _bl;

    // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderProperty =
        DependencyProperty.Register("Order1", typeof(BO.Order), typeof(OrderUpdateWindow));
    public BO.Order Order1
    {
        get { return (BO.Order)GetValue(OrderProperty); }
        set { SetValue(OrderProperty, value); }
    }


    /// <summary>
    /// Constructor for the OrderUpdateWindow class.
    /// Initializes the window with the specified order details.
    /// </summary>
    /// <param name="bl">Reference to the business logic layer</param>
    /// <param name="id">ID of the order to display</param>
    public OrderUpdateWindow(BlApi.IBl bl, int id)
    {
        // Retrieve the order details from the business logic layer
        Order1 = bl.Order.RequestOrderDetails(id);
        _bl = bl;
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the back button click.
    /// Closes the current window and opens the OrderListAdminWindow.
    /// </summary>
    private void BackToOrderList_Button_Click(object sender, RoutedEventArgs e)
    {
        new OrderListAdminWindow().Show();
        this.Close();
    }


    /// <summary>
    /// Event handler for the update shipping date button click.
    /// Calls the UpdateOrderShipDateAdmin method in the business logic layer and updates the Order1 property.
    /// </summary>
    private void UpdateShipDate(object sender, RoutedEventArgs e)
    {
        Order1 = _bl.Order.UpdateOrderShipDateAdmin(Order1.ID);
        ((Button)sender).Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Event handler for the update delivery date button click.
    /// Calls the UpdateOrderDeliveryDateAdmin method in the business logic layer and updates the Order1 property if the shipping date is not null.
    /// </summary>
    private void UpdateDeliveryDate(object sender, RoutedEventArgs e)
    {
        if (Order1.ShipDate is not null )
        {
            Order1 = _bl.Order.UpdateOrderDeliveryDateAdmin(Order1.ID);
            ((Button)sender).Visibility = Visibility.Collapsed;
        }

    }
}

using System.Windows;


namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for OrderDetailsUser.xaml
/// </summary>
public partial class OrderDetailsUser : Window
{
    // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderProperty =
        DependencyProperty.Register("TOrder", typeof(BO.Order), typeof(OrderUpdateWindow));
    public BO.Order TOrder
    {
        get { return (BO.Order)GetValue(OrderProperty); }
        set { SetValue(OrderProperty, value); }
    }

    /// <summary>
    /// Constructor for the OrderDetailsUser window.
    /// Initializes the Order property with the specified order.
    /// </summary>
    /// <param name="_order">The order to display in this window</param>
    public OrderDetailsUser(BO.Order _order)
    {
        TOrder = _order;
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the "Back to Order Tracking" button click.
    /// Closes this window.
    /// </summary>
    private void BackToOrderTracking_Button_Click(object sender, RoutedEventArgs e) => this.Close();
}


using System.Windows;
namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for OrderUpdateWindow.xaml
/// </summary>
public partial class OrderUpdateWindow : Window
{
    readonly BlApi.IBl? bl;

    // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderProperty =
        DependencyProperty.Register("Order", typeof(BO.Order), typeof(OrderUpdateWindow));
    public BO.Order Order1
    {
        get { return (BO.Order)GetValue(OrderProperty); }
        set { SetValue(OrderProperty, value); }
    }


    public OrderUpdateWindow(BlApi.IBl bl, int id)
    {
        Order1 = bl.Order.RequestOrderDetails(id);
        InitializeComponent();
    }



    
}
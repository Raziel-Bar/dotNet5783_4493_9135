using BO;
using System.Collections.ObjectModel;
using System.Windows;
namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for OrderTrackingWindow.xaml
/// </summary>
public partial class OrderTrackingWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    public ObservableCollection<OrderForList> Orders { set; get; }


    public int id;
    public OrderTrackingWindow()
    {
        Orders = new ObservableCollection<BO.OrderForList>(bl.Order.RequestOrdersListAdmin()!);
        InitializeComponent();
    }

    private void Tracing_Click(object sender, RoutedEventArgs e)
    {
        
    }

    //private void TrackingOrder_Click(object sender, RoutedEventArgs e)
    //{

    //}

    //private void ViewOrder_Click(object sender, RoutedEventArgs e)
    //{
    //}
}

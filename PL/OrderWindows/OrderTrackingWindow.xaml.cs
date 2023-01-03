using BO;
using System;
using System.Collections.ObjectModel;
using System.Windows;
namespace PL.OrderWindows;

/// <summary>
/// Interaction logic for OrderTrackingWindow.xaml
/// </summary>
public partial class OrderTrackingWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    //public ObservableCollection<OrderForList> Orders { set; get; }



    public int id
    {
        get { return (int)GetValue(idProperty); }
        set { SetValue(idProperty, value); }
    }

    // Using a DependencyProperty as the backing store for id.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty idProperty =
        DependencyProperty.Register("id", typeof(int), typeof(OrderTrackingWindow), new PropertyMetadata(0));



    public OrderTrackingWindow()
    {
        //Orders = new ObservableCollection<BO.OrderForList>(bl.Order.RequestOrdersListAdmin()!);
        InitializeComponent();
    }


    private void TrackingOrder_Click(object sender, RoutedEventArgs e)
    {
        try 
        {
            new TrackingWindow(bl!.Order.OrderTrackingAdmin(id)).ShowDialog();
        }
        catch (Exception)
        {
            new ErrorMessageWindow("ID Error", "No order with such ID exists!").ShowDialog();
        }
    }

    private void ViewOrder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            new OrderDetailsUser(bl!.Order.RequestOrderDetails(id)).ShowDialog();
        }
        catch (Exception)
        {
            new ErrorMessageWindow("ID Error", "No order with such ID exists!").ShowDialog();
        }
    }

    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}

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
    // A reference to the business logic layer
    readonly BlApi.IBl? bl = BlApi.Factory.Get();




    // Dependency property to hold the order id
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


    /// <summary>
    /// Event handler for the "Tracking Order" button click.
    /// Opens the TrackingWindow for the order with the specified id.
    /// Shows an error message if no order with the specified id exists.
    /// </summary>
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


    /// <summary>
    /// Event handler for the "View Order" button click.
    /// Opens the OrderDetailsUser window for the order with the specified id.
    /// Shows an error message if no order with the specified id exists.
    /// </summary>
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

    /// <summary>
    /// Event handler for the "Back to Main Window" button click.
    /// Opens the main window and closes this one.
    /// </summary>
    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }

}

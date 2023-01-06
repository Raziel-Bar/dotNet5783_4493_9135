using BO;
using PL.ProductWindows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL.OrderWindows;


/// <summary>
/// Interaction logic for OrderListAdminWindow.xaml
/// </summary>
public partial class OrderListAdminWindow : Window
{
    /// <summary>
    /// Reference to the business logic layer.
    /// </summary>
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// Observable collection that stores a list of orders.
    /// </summary>
    public ObservableCollection<OrderForList> Orders { set; get; }

    /// <summary>
    /// Constructor for the window.
    /// </summary>
    public OrderListAdminWindow()
    {
        // Initialize the Orders property with a list of orders from the business logic layer
        Orders = new ObservableCollection<BO.OrderForList>(bl.Order.RequestOrdersListAdmin()!);
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the "Back to Main Window" button click.
    /// Opens the main window and closes this one.
    /// </summary>
    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new AdminWindow().Show();
        this.Close();
    }

    /// <summary>
    /// Event handler for mouse button click on an order in the list view.
    /// Opens the OrderUpdateWindow in update mode for the selected order.
    /// </summary>
    private void ToOrderWindowUpdateMode(object sender, MouseButtonEventArgs e)
    {
        // Get the selected order in the list view
        var selected = (OrderForList)(((ListView)sender).SelectedItem);
        new OrderUpdateWindow(bl, selected.ID).Show();
        this.Close();
    }

}






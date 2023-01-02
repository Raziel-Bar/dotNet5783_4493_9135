using BO;
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
    readonly BlApi.IBl? bl = BlApi.Factory.Get();
    public ObservableCollection<OrderForList> Orders { set; get; }


    public OrderListAdminWindow()
    {
        Orders = new ObservableCollection<BO.OrderForList>(bl.Order.RequestOrdersListAdmin()!);

        // Data = new() { Orders = bl.Order.RequestOrdersListAdmin()! };
       // Data = new(bl);
        InitializeComponent();
    }

    private void BackToMainWindow(object sender, RoutedEventArgs e) => this.Close();


    private void ToOrderWindowUpdateMode(object sender, MouseButtonEventArgs e)
    {
        var selected = (OrderForList)(((ListView)sender).SelectedItem);
        new OrderUpdateWindow(bl, selected.ID).Show();
        this.Close();
    }

}






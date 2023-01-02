
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BO;

namespace PL.OrderForListWindows;

/// <summary>
/// Interaction logic for ShowAndUpdateOrder.xaml
/// </summary>
public partial class ShowAndUpdateOrder : Window
{
    readonly BlApi.IBl bl;

    public Order Order1
    {
        get { return (Order)GetValue(OrderProperty); }
        set { SetValue(OrderProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderProperty =
        DependencyProperty.Register("Order", typeof(Order), typeof(ShowAndUpdateOrder));


    public ShowAndUpdateOrder(BlApi.IBl bl, int id)
    {
        this.bl = bl;
        Order1 = bl.Order.RequestOrderDetails(id);
        DataContext = this;
        InitializeComponent();
    }

    /// <summary>
    /// BONUS sorts the items in the list view based on column
    /// </summary>
    /// <param name="sender">WinesListView : ListView</param>
    /// <param name="e">mouse double click</param>
    private void WinesListViewColumnHeader_Click(object sender, RoutedEventArgs e)
    {


        //GridViewColumnHeader? column = sender as GridViewColumnHeader;
        //if (sortBy == column?.Tag.ToString()) // we click the same column to flip direction
        //{
        //    if (direction == ListSortDirection.Ascending) direction = ListSortDirection.Descending;
        //    else direction = ListSortDirection.Ascending;
        //}
        //else
        //{
        //    direction = ListSortDirection.Ascending;
        //    sortBy = column?.Tag.ToString();
        //}
        //WinesListView.Items.SortDescriptions.Clear();
        //WinesListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
    }

    private void shipDateClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Order1 = bl.Order.UpdateOrderShipDateAdmin(Order1.ID);
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    public void DeliveryDateClick(object sender, RoutedEventArgs e)
    {
        if (Order1.ShipDate is not null)
            try
            {
                Order1 = bl.Order.UpdateOrderDeliveryDateAdmin(Order1.ID);
            }
            catch (System.Exception)
            {

                throw;
            }
        else
        {

        }
    }


}

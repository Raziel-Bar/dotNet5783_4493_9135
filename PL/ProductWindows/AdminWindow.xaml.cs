using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace PL.ProductWindows;

public enum WINERYS
{
    GOLAN,
    DALTON,
    TEPERBERG,
    CARMEL,
    BARKAN,
    ALL,
}

public class MyData : DependencyObject
{
    public IEnumerable<IGrouping<BO.WINERYS?, BO.ProductForList?>>? Products
    {
        get { return (IEnumerable<IGrouping<BO.WINERYS?, BO.ProductForList?>>?)GetValue(productsProperty); }
        set { SetValue(productsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productsProperty =
        DependencyProperty.Register("Products", typeof(IEnumerable<IGrouping<BO.WINERYS?, BO.ProductForList?>>), typeof(MyData));

    public IEnumerable<OrderForList>? Orders
    {
        get { return (IEnumerable<OrderForList>?)GetValue(ordersProperty); }
        set { SetValue(ordersProperty, value); }
    }

    // Using a DependencyProperty as the backing store for orders.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ordersProperty =
        DependencyProperty.Register("Orders", typeof(IEnumerable<OrderForList>), typeof(MyData));



    public List<ProductForList?>? ProductsList
    {
        get { return (List<ProductForList?>?)GetValue(ProductsListProperty); }
        set { SetValue(ProductsListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductsListProperty =
        DependencyProperty.Register("ProductsList", typeof(List<ProductForList?>), typeof(MyData));


    
    public Array? Categories { get; set; }
}

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class AdminWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();
    MyData data;
    //bonus
    ListSortDirection direction;
    string? sortBy = null;

    /// <summary>
    /// the list window with all prduct and their details
    /// </summary>
    public AdminWindow()
    {
        InitializeComponent();
        data = new()
        {
            Products = bl.Product.RequestProducts(),
            Orders = bl.Order.RequestOrdersListAdmin()!,
            Categories = Enum.GetValues(typeof(WINERYS))
        };
        data.ProductsList = (from _product in data.Products
                             from details in _product
                             select details).ToList();
        DataContext = data;

        // BONUS we made the code below so we could sort the listView (either ascending or descending!) by clicking the column headers
        sortBy = "Name";
        direction = ListSortDirection.Ascending;
        WinesListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
        OrdersListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
    }

    /// <summary>
    /// filter for list view
    /// </summary>
    /// <param name="sender">the WinerySelector combo box</param>
    /// <param name="e">selection changed</param>
    private void WinerySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((WINERYS)WinerySelector.SelectedItem == WINERYS.ALL) data.ProductsList = (from _product in data.Products
                                                                                      from details in _product
                                                                                      select details).ToList();
        else data.ProductsList = (from _product in data.Products
                                 from details in _product
                                 where details.Category == (BO.WINERYS)WinerySelector.SelectedItem
                                 select details).ToList();

    }

    /// <summary>
    /// moving to add a new product to the list
    /// </summary>
    /// <param name="sender">the add button</param>
    /// <param name="e">mouse click</param>
    private void ToProductWindowAddMode(object sender, RoutedEventArgs e)
    {
        new ProductAddOrUpdateAdminWindow().Show();
        this.Close();
    }

    /// <summary>
    /// moving to update the selected product in the list
    /// </summary>
    /// <param name="sender">the selected listView item</param>
    /// <param name="e">mouse Double click</param>
    private void ToProductWindowUpdateMode(object sender, MouseButtonEventArgs e)
    {
        BO.ProductForList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.ProductForList)!;
        if (item != null)
        {
            if (WinesListView.SelectedItem is ProductForList productForList)
            {
                new ProductAddOrUpdateAdminWindow(productForList.ID).Show();
                this.Close();
            }
        }
    }

    /// <summary>
    /// going back to main window
    /// </summary>
    /// <param name="sender">the back button</param>
    /// <param name="e">mouse click</param>
    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }

    private void WinesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    /// <summary>
    /// BONUS sorts the items in the list view based on column
    /// </summary>
    /// <param name="sender">WinesListView : ListView</param>
    /// <param name="e">mouse double click</param>
    private void WinesListViewColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        GridViewColumnHeader? column = sender as GridViewColumnHeader;
        if (sortBy == column?.Tag.ToString()) // we click the same column to flip direction
        {
            if (direction == ListSortDirection.Ascending) direction = ListSortDirection.Descending;
            else direction = ListSortDirection.Ascending;
        }
        else
        {
            direction = ListSortDirection.Ascending;
            sortBy = column?.Tag.ToString();
        }
        WinesListView.Items.SortDescriptions.Clear();
        WinesListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
    }

    /// <summary>
    /// BONUS sorts the items in the list view based on column
    /// </summary>
    /// <param name="sender">WinesListView : ListView</param>
    /// <param name="e">mouse double click</param>
    private void OrdersListViewColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        GridViewColumnHeader? column = sender as GridViewColumnHeader;
        if (sortBy == column?.Tag.ToString()) // we click the same column to flip direction
        {
            if (direction == ListSortDirection.Ascending) direction = ListSortDirection.Descending;
            else direction = ListSortDirection.Ascending;
        }
        else
        {
            direction = ListSortDirection.Ascending;
            sortBy = column?.Tag.ToString();
        }
        OrdersListView.Items.SortDescriptions.Clear();
        OrdersListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
    }

    private void OrderList_Click(object sender, RoutedEventArgs e)
    {
        UpperGridProduct.Visibility = Visibility.Collapsed;
        UpperGridOrder.Visibility = Visibility.Visible;
        AddProductButton.Visibility = Visibility.Collapsed;
    }

    private void ProductList_Click(object sender, RoutedEventArgs e)
    {
        AddProductButton.Visibility = Visibility.Visible;
        UpperGridOrder.Visibility = Visibility.Collapsed;
        UpperGridProduct.Visibility = Visibility.Visible;
    }
}

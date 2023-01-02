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

public enum WINERIES
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
    public IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>? Products
    {
        get => (IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>?)GetValue(productsProperty);
        set => SetValue(productsProperty, value);
    }

    // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productsProperty =
        DependencyProperty.Register("Products", typeof(IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>), typeof(MyData));

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

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(MyData), typeof(AdminWindow));
    public MyData Data { get => (MyData)GetValue(DataDep); set => SetValue(DataDep, value); }
    //bonus
    ListSortDirection direction;
    string? sortBy = null;

    /// <summary>
    /// the list window with all prduct and their details
    /// </summary>
    public AdminWindow()
    {
        InitializeComponent();
        Data = new()
        {
            Products = bl.Product.RequestProducts(),
            Orders = bl.Order.RequestOrdersListAdmin()!,
            Categories = Enum.GetValues(typeof(WINERIES))
        };
        Data.ProductsList = Data.Products.SelectMany(p => p).ToList();

        // BONUS we made the code below so we could sort the listView (either ascending or descending!) by clicking the column headers
        sortBy = "Name";
        direction = ListSortDirection.Ascending;
        //WinesListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
        //OrdersListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
    }

    /// <summary>
    /// filter for list view
    /// </summary>
    /// <param name="sender">the WINERIESelector combo box</param>
    /// <param name="e">selection changed</param>
    private void WINERIESelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        if ((WINERIES)selected == WINERIES.ALL) Data.ProductsList = Data.Products!.SelectMany(p => p).ToList();
        else Data.ProductsList = Data.Products!.FirstOrDefault(g => g.Key == (BO.WINERIES?)selected)?.ToList() ?? new();
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

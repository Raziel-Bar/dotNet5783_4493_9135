using BO;
using PL.OrderWindows;
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


/// <summary>
/// for the category option (all)
/// </summary>
public enum WINERIES
{
    GOLAN,
    DALTON,
    TEPERBERG,
    CARMEL,
    BARKAN,
    ALL,
}

/// <summary>
/// Gets or sets the categories.
/// </summary>
public class AdminWindowData : DependencyObject
{

    /// <summary>
    /// Gets or sets the products.
    /// </summary>
    public IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>? Products
    {
        get => (IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>?)GetValue(productsProperty);
        set => SetValue(productsProperty, value);
    }

    // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productsProperty =
        DependencyProperty.Register("Products", typeof(IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>), typeof(AdminWindowData));


    /// <summary>
    /// Gets or sets the orders.
    /// </summary>
    public IEnumerable<OrderForList>? Orders
    {
        get { return (IEnumerable<OrderForList>?)GetValue(ordersProperty); }
        set { SetValue(ordersProperty, value); }
    }

    // Using a DependencyProperty as the backing store for orders.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ordersProperty =
        DependencyProperty.Register("Orders", typeof(IEnumerable<OrderForList>), typeof(AdminWindowData));


    /// <summary>
    /// Gets or sets the list of products.
    /// </summary>
    public List<ProductForList?>? ProductsList
    {
        get { return (List<ProductForList?>?)GetValue(ProductsListProperty); }
        set { SetValue(ProductsListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductsListProperty =
        DependencyProperty.Register("ProductsList", typeof(List<ProductForList?>), typeof(AdminWindowData));


    /// <summary>
    /// Gets or sets the categories.
    /// </summary>
    public Array? Categories { get; set; }
}

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class AdminWindow : Window
{
    // Reference to the business logic layer
    readonly BlApi.IBl? bl = BlApi.Factory.Get();


    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(AdminWindowData), typeof(AdminWindow));

    /// <summary>
    /// Gets or sets the data for the AdminWindow.
    /// </summary>
    public AdminWindowData Data { get => (AdminWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

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
        else Data.ProductsList = Data.Products!.FirstOrDefault(g => g.Key == (BO.WINERIES)selected)?.ToList() ?? new();
    }

    /// <summary>
    /// moving to add a new product to the list
    /// </summary>
    /// <param name="sender">the add button</param>
    /// <param name="e">mouse click</param>
    private void ToProductWindowAddMode(object sender, RoutedEventArgs e)
    {
        new ProductAddOrUpdateAdminWindow(bl).Show();
        this.Close();
    }

    /// <summary>
    /// moving to update the selected product in the list
    /// </summary>
    /// <param name="sender">the selected listView item</param>
    /// <param name="e">mouse Double click</param>
    private void ToProductWindowUpdateMode(object sender, MouseButtonEventArgs e)
    {
        var selected = ((ListView)sender).SelectedItem;

        BO.ProductForList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.ProductForList)!;

        if (item != null)
        {
            if (selected is ProductForList productForList)
            {
                new ProductAddOrUpdateAdminWindow(bl, productForList.ID).Show();
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

    private void OrderList_Click(object sender, RoutedEventArgs e)
    {
        new OrderListAdminWindow().Show();
        this.Close();
    }

}

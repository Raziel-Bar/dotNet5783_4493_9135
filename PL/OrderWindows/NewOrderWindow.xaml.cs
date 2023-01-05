using BO;
using PL.ProductWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace PL.OrderWindows;

public class NewOrderWindowData : DependencyObject
{
    public IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>? Products
    {
        get => (IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>?)GetValue(productsProperty);
        set => SetValue(productsProperty, value);
    }

    // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productsProperty =
        DependencyProperty.Register("Products", typeof(IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>), typeof(NewOrderWindowData));

    public List<ProductForList?>? ProductsList
    {
        get { return (List<ProductForList?>?)GetValue(ProductsListProperty); }
        set { SetValue(ProductsListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductsListProperty =
        DependencyProperty.Register("ProductsList", typeof(List<ProductForList?>), typeof(NewOrderWindowData));


/*        public Dictionary<BO.WINERIES, List<ProductForList?>?> GroupsToShow
    {
        get { return (Dictionary<BO.WINERIES, List<ProductForList?>?>)GetValue(GroupsToShowProperty); }
        set { SetValue(GroupsToShowProperty, value); }
    }

    // Using a DependencyProperty as the backing store for GroupsToShow.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GroupsToShowProperty =
        DependencyProperty.Register("GroupsToShow", typeof(Dictionary<BO.WINERIES, List<ProductForList?>?>), typeof(NewOrderWindowData));

    public Dictionary<BO.WINERIES, List<ProductForList?>?>? Groups { get; set; }
*/        public Array? Categories { get; set; }
}

/// <summary>
/// Interaction logic for NewOrderWindow.xaml
/// </summary>
public partial class NewOrderWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(NewOrderWindowData), typeof(NewOrderWindow));
    public NewOrderWindowData Data { get => (NewOrderWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

    Cart cart; 

    public NewOrderWindow()
    {
        InitializeComponent();
        Data = new()
        {
            Products = bl.Product.RequestProducts(),
            Categories = Enum.GetValues(typeof(PL.ProductWindows.WINERIES)),
            //Groups = new(),
            //GroupsToShow = new()
        };
        Data.ProductsList = Data.Products.SelectMany(p => p).ToList();
        /*foreach (BO.WINERIES category in Enum.GetValues(typeof(BO.WINERIES)))
        {
            Data.Groups.Add(category, Data.Products!.FirstOrDefault(g => g.Key == category)?.ToList());
            Data.GroupsToShow.Add(category, Data.Products!.FirstOrDefault(g => g.Key == category)?.ToList());
        }*/

    }

    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }

    private void ConfirmOrder(object sender, RoutedEventArgs e)
    {

    }

    private void ProductDetails(object sender, MouseButtonEventArgs e)
    {
        var selected = ((ListView)sender).SelectedItem;

        BO.ProductForList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.ProductForList)!;

        if (item != null)
        {
            if (selected is ProductForList productForList)
            {
                new ProductDetailsUserWindow(bl, productForList.ID).ShowDialog();
            }
        }
    }

    /// <summary>
    /// filter for list view
    /// </summary>
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        if ((PL.ProductWindows.WINERIES)selected == PL.ProductWindows.WINERIES.ALL)
        {
            Data.ProductsList = Data.Products!.SelectMany(p => p).ToList();
            //Data.GroupsToShow = Data.Groups!.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
        else
        {
            Data.ProductsList = Data.Products!.FirstOrDefault(g => g.Key == (BO.WINERIES)selected)?.ToList() ?? new();
            /*Data.GroupsToShow.Clear();
            Data.GroupsToShow = new()
            {
                { (BO.WINERIES)selected, Data.Products!.FirstOrDefault(g => g.Key == (BO.WINERIES)selected)?.ToList() }
            };*/
        }
    }

    private void GoTocart(object sender, RoutedEventArgs e)
    {
        new CartWindow(cart).Show();
    }
}

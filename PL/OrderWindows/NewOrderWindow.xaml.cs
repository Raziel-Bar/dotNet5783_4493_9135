using BO;
using Microsoft.VisualBasic;
using PL.ProductWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

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


        public Array? Categories { get; set; }
}

/// <summary>
/// Interaction logic for NewOrderWindow.xaml
/// </summary>
public partial class NewOrderWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(NewOrderWindowData), typeof(NewOrderWindow));
    public NewOrderWindowData Data { get => (NewOrderWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

    public Cart cart
    {
        get { return (Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty cartProperty =
        DependencyProperty.Register("cart", typeof(Cart), typeof(CartDetails));


    public NewOrderWindow(Cart _cart)
    {
        InitializeComponent();
        Data = new()
        {
            Products = bl.Product.RequestProducts(),
            Categories = Enum.GetValues(typeof(PL.ProductWindows.WINERIES)),
        };
        Data.ProductsList = Data.Products.SelectMany(p => p).ToList();
        cart = _cart;
    }

    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }

    private void ConfirmOrder(object sender, RoutedEventArgs e)
    {
        new UserDetailsWindow(cart).Show();
        this.Close();
    }

    private void ProductDetails(object sender, MouseButtonEventArgs e)
    {
        var selected = ((ListView)sender).SelectedItem;

        BO.ProductForList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.ProductForList)!;

        if (item != null)
        {
            if (selected is ProductForList productForList)
            {
                new ProductDetailsUserWindow(bl!, productForList.ID).ShowDialog();
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
        }
    }

    private void GoTocart(object sender, RoutedEventArgs e)
    {
        new CartDetails(cart).ShowDialog();
    }

    private void Add1ToCart(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        var item = button!.Tag as ProductForList;
        try
        {
            bl!.Cart.AddProductToCart(item!.ID, cart);

            // Save the original style and content
            Style originalStyle = button.Style;
            object originalContent = button.Content;

            // Create a new style based on the original style
            Style clickedStyle = new Style(typeof(Button), originalStyle);
            clickedStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Red));
            clickedStyle.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));

            // Apply the new style and content to the button
            button.Style = clickedStyle;
            button.Content = "Added";

            // Create a timer to restore the original style and content after 3 seconds
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, a) =>
            {
                // Restore the original style and content
                button.Style = originalStyle;
                button.Content = originalContent;
                timer.Stop();
            };
            timer.Start();

        }
        catch (BO.StockNotEnoughtOrEmptyException) { new ErrorMessageWindow("Out of stock", "Sorry!\nItem is out of stock.").Show(); }
        catch (Exception ex) { new ErrorMessageWindow("Unexpected Error", ex.Message).Show(); }
    }

    private void UpdateAmount(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        var item = button!.Tag as ProductForList;
        string input = Interaction.InputBox("Enter Amount:", "Input", "", -1, -1);
        try
        {
            bl!.Cart.UpdateProductInCart(item!.ID, cart, Convert.ToInt32(input));
        }
        catch (BO.StockNotEnoughtOrEmptyException) { new ErrorMessageWindow("Out of stock", "Sorry!\nItem is out of stock.").Show(); }
        catch (BO.ProductNotFoundInCartException) 
        {
            bl!.Cart.AddProductToCart(item!.ID, cart);
            bl!.Cart.UpdateProductInCart(item!.ID, cart, Convert.ToInt32(input));
        }
        catch (BO.InvalidDataException) { new ErrorMessageWindow("Invalid Data", "Make sure your input is a non negative number").Show(); }
        catch (Exception ex) { new ErrorMessageWindow("Unexpected Error", ex.Message).Show(); }

    }

    private void RemoveItem(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        var item = button!.Tag as ProductForList;
        try
        {
            bl!.Cart.UpdateProductInCart(item!.ID, cart, 0);

            // Save the original style and content
            Style originalStyle = button.Style;
            object originalContent = button.Content;

            // Create a new style based on the original style
            Style clickedStyle = new Style(typeof(Button), originalStyle);
            clickedStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Red));
            clickedStyle.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));

            // Apply the new style and content to the button
            button.Style = clickedStyle;
            button.Content = "Removed";

            // Create a timer to restore the original style and content after 3 seconds
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, a) =>
            {
                // Restore the original style and content
                button.Style = originalStyle;
                button.Content = originalContent;
                timer.Stop();
            };
            timer.Start();

        }
        catch (BO.StockNotEnoughtOrEmptyException) { new ErrorMessageWindow("Out of stock", "Sorry!\nItem is out of stock.").Show(); }
        catch (BO.ProductNotFoundInCartException) { }
        catch (Exception ex) { new ErrorMessageWindow("Unexpected Error", ex.Message).Show(); }
    }
}

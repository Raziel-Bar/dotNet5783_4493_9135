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

/// <summary>
/// This class represents the data for the NewOrderWindow.
/// It has a property for a list of products, a property for a list of categories,
/// and a property for a list of products in list format.
/// </summary>
public class NewOrderWindowData : DependencyObject
{
    /// <summary>
    /// Gets or sets the list of products for the window.
    /// </summary>
    public IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>? Products
    {
        get => (IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>?)GetValue(productsProperty);
        set => SetValue(productsProperty, value);
    }

    // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productsProperty =
        DependencyProperty.Register("Products", typeof(IEnumerable<IGrouping<BO.WINERIES?, BO.ProductForList?>>), typeof(NewOrderWindowData));

    /// <summary>
    /// Gets or sets the list of products in list format for the window.
    /// </summary>
    public List<ProductForList?>? ProductsList
    {
        get { return (List<ProductForList?>?)GetValue(ProductsListProperty); }
        set { SetValue(ProductsListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductsListProperty =
        DependencyProperty.Register("ProductsList", typeof(List<ProductForList?>), typeof(NewOrderWindowData));

    /// <summary>
    /// Gets or sets the list of categories for the window.
    /// </summary>
    public Array? Categories { get; set; }
}


/// <summary>
/// Interaction logic for NewOrderWindow.xaml
/// </summary>
public partial class NewOrderWindow : Window
{
    /// <summary>
    /// This field stores a reference to the business logic layer.
    /// </summary>
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency property to hold the data for this window
    /// </summary>
    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(NewOrderWindowData), typeof(NewOrderWindow));
    public NewOrderWindowData Data { get => (NewOrderWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

    /// <summary>
    /// Property for the cart
    /// </summary>
    public Cart cart
    {
        get { return (Cart)GetValue(cartProperty); }
        set { SetValue(cartProperty, value); }
    }

    /// <summary>
    /// Using a DependencyProperty as the backing store for cart.  This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty cartProperty =
        DependencyProperty.Register("cart", typeof(Cart), typeof(CartDetails));

    /// <summary>
    /// Constructor for the window
    /// </summary>
    /// <param name="_cart">The cart for this order</param>
    public NewOrderWindow(Cart _cart)
    {
        InitializeComponent();
        // Initialize the Data property with product and category information

        Data = new()
        {
            Products = bl.Product.RequestProducts(),
            Categories = Enum.GetValues(typeof(PL.ProductWindows.WINERIES)),
        };
        // Initialize the ProductsList property with a list of all products
        Data.ProductsList = Data.Products.SelectMany(p => p).ToList();
        cart = _cart;
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

    /// <summary>
    /// Event handler for the "Confirm Order" button click.
    /// Opens the UserDetailsWindow and closes this one.
    /// </summary>
    private void ConfirmOrder(object sender, RoutedEventArgs e)
    {
        new UserDetailsWindow(cart).Show();
        this.Close();
    }

    /// <summary>
    /// Event handler for the mouse button click on a product in the list view.
    /// Opens the ProductDetailsUserWindow for the selected product.
    /// </summary>
    private void ProductDetails(object sender, MouseButtonEventArgs e)
    {
        // Get the selected item in the list view
        var selected = ((ListView)sender).SelectedItem;

        // Get the product for the selected item
        BO.ProductForList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.ProductForList)!;

        if (item != null)
        {
            if (selected is ProductForList productForList)
            {
                new ProductDetailsUserWindow(bl!, productForList.ID,cart).ShowDialog();
            }
        }
    }

    /// <summary>
    /// Event handler for the selection change of the category combo box.
    /// Filters the products list view to show only products in the selected category.
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


    /// <summary>
    /// Event handler for the "Go to Cart" button click.
    /// Opens the CartDetails window.
    /// </summary>
    private void GoTocart(object sender, RoutedEventArgs e)
    {
        new CartDetails(cart).ShowDialog();
    }

    /// <summary>
    /// Event handler for the "Add to Cart" button click.
    /// Adds one of the product to the cart, and updates the button's style and content temporarily to indicate that the product was added.
    /// </summary>
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



    /// <summary>
    /// Updates the amount of a product in the cart.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void UpdateAmount(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        var item = button!.Tag as ProductForList;
        string input = Interaction.InputBox("Enter Amount:", "Input", "", -1, -1);
        try
        {
            bl!.Cart.UpdateProductInCart(item!.ID, cart, Convert.ToInt32(input));
        }
        // Show an error message if the product is out of stock
        catch (BO.StockNotEnoughtOrEmptyException) { new ErrorMessageWindow("Out of stock", "Sorry!\nItem is out of stock.").Show(); }

        // If the product is not found in the cart, add it and then update its amount
        catch (BO.ProductNotFoundInCartException)
        {
            bl!.Cart.AddProductToCart(item!.ID, cart);
            bl!.Cart.UpdateProductInCart(item!.ID, cart, Convert.ToInt32(input));
        }
        catch (BO.InvalidDataException) { new ErrorMessageWindow("Invalid Data", "Make sure your input is a non negative number").Show(); }
        catch (Exception ex) { new ErrorMessageWindow("Unexpected Error", ex.Message).Show(); }

    }


    /// <summary>
    /// Removes an item from the cart.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
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

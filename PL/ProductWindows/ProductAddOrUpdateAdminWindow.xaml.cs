using BO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PL.ProductWindows;

public class ProductAddOrUpdateAdminWindowData : DependencyObject
{
    // Using a DependencyProperty as the backing store for products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productsProperty =
        DependencyProperty.Register("MyProduct", typeof(BO.Product), typeof(ProductAddOrUpdateAdminWindowData));
    public BO.Product? MyProduct
    {
        get => (BO.Product?)GetValue(productsProperty);
        set => SetValue(productsProperty, value);
    }

    public Array? Categories { get; set; }
    public Visibility updateMode { get; set; }
    public Visibility addMode { get; set; }

    public bool isReadOnlyID { get; set; }
}

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductAddOrUpdateAdminWindow : Window
{
    readonly BlApi.IBl? bl;

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(ProductAddOrUpdateAdminWindowData), typeof(ProductAddOrUpdateAdminWindow));
    public ProductAddOrUpdateAdminWindowData Data { get => (ProductAddOrUpdateAdminWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }


    /// <summary>
    /// a window for either adding or updating a product
    /// </summary>
    /// <param name="id">The product's ID in case the window is in UPDATE mode. otherwise it's 0 which indicates the window is in ADD mode</param>
    public ProductAddOrUpdateAdminWindow(BlApi.IBl? bl, int id = 0)
    {
        this.bl = bl;
        
        Data = new()
        {
            isReadOnlyID = id == 0 ? false : true,
            MyProduct = id == 0 ? new() : bl?.Product.RequestProductDetailsAdmin(id),
            Categories = Enum.GetValues(typeof(BO.WINERIES)),
            addMode = id == 0 ? Visibility.Visible : Visibility.Hidden,
            updateMode = id != 0 ? Visibility.Visible : Visibility.Hidden
        };
        InitializeComponent();
    }

    /// <summary>
    /// the add product event
    /// </summary>
    /// <param name="sender">the add button</param>
    /// <param name="e">mouse click</param>
    private void AddProductEvent(object sender, RoutedEventArgs e)
    {
        try
        {
            bl?.Product.AddProductAdmin(Data.MyProduct!);
            new SuccessWindow("Your Product Has been added successfully!").ShowDialog();
            new AdminWindow().Show();
            this.Close();
        }
        catch (BO.InvalidDataException)
        {
            new ErrorMessageWindow("Invalid Data", "ID must contain at least 6 digits").ShowDialog();
        }
        catch (BO.AlreadyExistInDalException)
        {
            new ErrorMessageWindow("Existing Data Error", "That Product already exists! Try a different ID").ShowDialog();
        }
        catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
        {
            new ErrorMessageWindow("Unexpected Error!", ex.Message).ShowDialog();
        }
    }

    /// <summary>
    /// the update product event
    /// </summary>
    /// <param name="sender">the update button</param>
    /// <param name="e">mouse click</param>
    private void UpdateProductEvent(object sender, RoutedEventArgs e)
    {
        try
        {
            bl?.Product.UpdateProductAdmin(Data.MyProduct!);
            new SuccessWindow("Your Product Has been Updated successfully!").ShowDialog();
            new AdminWindow().Show();
            this.Close();
        }
        catch (BO.InvalidDataException)
        {
            new ErrorMessageWindow("Invalid Data", "ID must contain at least 6 digits").ShowDialog();
        }
        catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
        {
            new ErrorMessageWindow("Unexpected Error!", ex.Message).ShowDialog();
        }
    }

    /// <summary>
    /// going back to ProducrFor list window
    /// </summary>
    /// <param name="sender">the back button</param>
    /// <param name="e">mouse click</param>
    private void BackToMainWindow(object sender, RoutedEventArgs e)
    {
        new AdminWindow().Show();
        this.Close();
    }


    // BONUS Using RegEx to make binding Validations to our input

    /// <summary>
    /// forces text to be only digits with 1 decimal point (optional)
    /// </summary>
    private void IsDoublePreviewTextInput(object sender, TextChangedEventArgs e)
    {
        string isNumber = @"^\d*\.?\d*$";
        TextBox text = (TextBox)sender;

        Match match = Regex.Match(text.Text, isNumber);

        if (text.Text == "" || text.Text.All(t => t =='0'))
        {
            text.Text = "1";
        }

        if (!match.Success)
        {
            if (text.Text.Length > 1)
                text.Text = text.Text.Substring(0, text.Text.Length - 1);
            else
                text.Text = "";

            text.Select(text.Text.Length, 0); //set cursor to the end 
                                              //of the string
        }
    }
    /// <summary>
    /// forces text to be only digits
    /// </summary>
    private void IsIntPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        Regex regex = new("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

 
}

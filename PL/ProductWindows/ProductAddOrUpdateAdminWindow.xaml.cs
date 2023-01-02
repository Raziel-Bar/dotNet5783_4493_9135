using BO;
using System;
using System.Windows;

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
        InitializeComponent();
        Data = new()
        {
            MyProduct = id == 0 ? null : bl?.Product.RequestProductDetailsAdmin(id),
            Categories = Enum.GetValues(typeof(WINERIES)),
            addMode = id == 0 ? Visibility : Visibility.Hidden,
            updateMode = id != 0 ? Visibility : Visibility.Hidden
        };

        // all old details are shown
        try
        {
            BO.Product product = bl?.Product.RequestProductDetailsAdmin(id) ?? throw new BO.UnexpectedException();

        }
        catch (Exception ex)
        {
            new ErrorMessageWindow("Unexpected Error!", ex.Message).Show();
        }

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
            bl?.Product.AddProductAdmin(Data.MyProduct);
            new SuccessWindow("Your Product Has been added successfully!").ShowDialog();
            this.Close();
        }

        catch (FormatException) // we do not allow empty boxes or illegal input
        {
            new ErrorMessageWindow("Input Error", "Invalid input!\nSome of the textboxes are either empty or contain wrong format input.").ShowDialog();
        }
        catch (BO.InvalidDataException ex)
        {
            new ErrorMessageWindow("Invalid Data", ex.Message).ShowDialog();
        }
        catch (BO.AlreadyExistInDalException ex)
        {
            new ErrorMessageWindow("Existing Data Error", ex.Message).ShowDialog();
        }
        catch (Exception ex)// in any other case we will just link the inner exception for better knowledge
        {
            //MessageBox.Show(ex.Message);
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
            bl?.Product.UpdateProductAdmin(Data.MyProduct);
            new SuccessWindow("Your Product Has been Updated successfully!").ShowDialog();
            new AdminWindow().Show();
            this.Close();
        }
        catch (FormatException) // we do not allow empty boxes or illegal input
        {
            new ErrorMessageWindow("Input Error", "Invalid input!\nSome of the textboxes are either empty or contain wrong format input.").ShowDialog();
        }
        catch (BO.InvalidDataException ex)
        {
            new ErrorMessageWindow("Invalid Data", ex.Message).ShowDialog();
        }
        catch (BO.NotFoundInDalException ex)
        {
            new ErrorMessageWindow("Data Not Found", ex.Message).ShowDialog();
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


}

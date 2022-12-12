using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductForListWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get(); //new Bl();
    IEnumerable<ProductForList?> productForLists;

    /// <summary>
    /// the list window with all prduct and their details
    /// </summary>
    public ProductForListWindow()
    {

        InitializeComponent();

        productForLists = bl.Product.RequestProducts();

        WinesListView.ItemsSource = productForLists;

        WinerySelector.ItemsSource = Enum.GetValues(typeof(WINERYS));

    }

    /// <summary>
    /// filter for list view
    /// </summary>
    /// <param name="sender">the WinerySelector combo box</param>
    /// <param name="e">selection changed</param>
    private void WinerySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((WINERYS)WinerySelector.SelectedItem == WINERYS.ALL) WinesListView.ItemsSource = productForLists;

        else WinesListView.ItemsSource = bl.Product.RequestProductsByCondition(productForLists, product => product?.Category == (BO.WINERYS)WinerySelector.SelectedItem);
    }

    /// <summary>
    /// moving to add a new product to the list
    /// </summary>
    /// <param name="sender">the add button</param>
    /// <param name="e">mouse click</param>
    private void ToProductWindowAddMode(object sender, RoutedEventArgs e)
    {
        new ProductWindow("ADD").Show();
        this.Close();
    }

    /// <summary>
    /// moving to update the selected product in the list
    /// </summary>
    /// <param name="sender">the selected listView item</param>
    /// <param name="e">mouse Double click</param>
    private void ToProductWindowUpdateMode(object sender, MouseButtonEventArgs e)
    {

        if (WinesListView.SelectedItem is ProductForList productForList)
        {
           // try
           // {
                //bl.Product.UpdateProductAdmin(bl.Product.RequestProductDetailsAdmin(productForList.ID));
                new ProductWindow("UPDATE", bl.Product.RequestProductDetailsAdmin(productForList.ID)).Show();
                this.Close();
            //}
            //catch (BO.RemoveProductThatIsInOrdersException)
            //{
               // new ErrorMessageWindow("Cannot Update the product because there are orders that includes that product!").Show();
            //}               
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
}

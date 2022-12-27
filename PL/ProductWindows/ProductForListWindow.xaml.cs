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

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductForListWindow : Window
{
    readonly BlApi.IBl? bl = BlApi.Factory.Get();

    IEnumerable<IGrouping<BO.WINERYS?, ProductForList?>> productForLists;
    ListSortDirection direction;
    string? sortBy = null;
    /// <summary>
    /// the list window with all prduct and their details
    /// </summary>
    public ProductForListWindow()
    {

        InitializeComponent();

        productForLists = bl.Product.RequestProducts();

        WinesListView.ItemsSource = from _product in productForLists
                                    from details in _product
                                    select details;
        WinerySelector.ItemsSource = Enum.GetValues(typeof(WINERYS));

        // BONUS we made the it so we could sort the listView (either ascending or descending!) by clicking the column headers
        sortBy = "Name";
        direction = ListSortDirection.Ascending;
        WinesListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
    }

    /// <summary>
    /// filter for list view
    /// </summary>
    /// <param name="sender">the WinerySelector combo box</param>
    /// <param name="e">selection changed</param>
    private void WinerySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((WINERYS)WinerySelector.SelectedItem == WINERYS.ALL) WinesListView.ItemsSource = from _product in productForLists
                                                                                             from details in _product
                                                                                             select details;

        //else WinesListView.ItemsSource = productForLists.Where(_product => _product.Key == (BO.WINERYS)WinerySelector.SelectedItem);
        else WinesListView.ItemsSource = from _product in productForLists
                                         where _product.Key == (BO.WINERYS)WinerySelector.SelectedItem
                                         from details in _product
                                         select details;

        //else WinesListView.ItemsSource = bl?.Product.RequestProductsByCondition(productForLists, product => product?.Category == (BO.WINERYS)WinerySelector.SelectedItem); &*&*&*&*&*&*&*&**
    }

    /// <summary>
    /// moving to add a new product to the list
    /// </summary>
    /// <param name="sender">the add button</param>
    /// <param name="e">mouse click</param>
    private void ToProductWindowAddMode(object sender, RoutedEventArgs e)
    {
        new ProductWindow().Show();
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
                new ProductWindow(productForList.ID).Show();
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
}

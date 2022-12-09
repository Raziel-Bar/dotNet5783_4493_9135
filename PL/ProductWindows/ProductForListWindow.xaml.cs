using BlApi;
using BlImplementation;
using BO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;




namespace PL.ProductWindows
{
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
        IBl bl = new Bl();
        IEnumerable<ProductForList?> productForLists;
        public ProductForListWindow()
        {

            InitializeComponent();

            productForLists = bl.Product.RequestProducts();

            WinesListView.ItemsSource = productForLists;

            WinerySelector.ItemsSource = Enum.GetValues(typeof(WINERYS));

        }

        private void WinerySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((WINERYS)WinerySelector.SelectedItem == WINERYS.ALL) WinesListView.ItemsSource = productForLists;

            else WinesListView.ItemsSource = bl.Product.RequestProductsByCondition(productForLists, product => product?.Category == (BO.WINERYS)WinerySelector.SelectedItem);
        }

        private void ToProductWindowAddMode(object sender, RoutedEventArgs e)
        {
            new ProductWindow("ADD").Show();
            this.Close();
        }

        private void ToProductWindowUpdateMode(object sender, MouseButtonEventArgs e)
        {
            if (WinesListView.SelectedItem is ProductForList productForList)
            {
                new ProductWindow("UPDATE", bl.Product.RequestProductDetailsAdmin(productForList.ID) ).Show();
                this.Close();
            }
        }

        private void BackToMainWindow(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        //private void WinesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        // SelectionChanged="WinesListView_SelectionChanged"
        //}
    }
}

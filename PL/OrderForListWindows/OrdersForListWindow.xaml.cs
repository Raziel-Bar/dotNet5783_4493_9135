using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.OrderForListWindows
{
    /// <summary>
    /// Interaction logic for OrdersForListWindow.xaml
    /// </summary>
    public partial class OrdersForListWindow : Window
    {
        readonly BlApi.IBl? bl = BlApi.Factory.Get();

        ListSortDirection direction;

        string? sortBy = null;

        public ObservableCollection<OrderForList> OrdersForList { get; set; }
        public OrdersForListWindow()
        {
            OrdersForList = new ObservableCollection<OrderForList>(bl.Order.RequestOrdersListAdmin()!);
            DataContext = this;
            InitializeComponent();

            sortBy = "Name";
            direction = ListSortDirection.Ascending;
            OrdersListView.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
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

        private void WinesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OrdersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptureWithin)
            {
                OrderForList orderForList = (OrderForList)OrdersListView.SelectedItem;
                if (orderForList is not null)
                {
                    new ShowAndUpdateOrder(bl ,orderForList.ID).Show();
                }
            }
        }
    }
}

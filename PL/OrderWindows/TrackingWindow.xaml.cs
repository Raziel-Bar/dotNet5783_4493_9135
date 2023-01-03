using BO;
using System;
using System.Collections.Generic;
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

namespace PL.OrderWindows
{
    public class TrackingWindowData
    {
        public BO.OrderTracking? Order { get; set; }
        public string? OrderDateStr { get; set; }
        public string? ShipDateStr { get; set; }
        public string? DeliveryDateStr { get; set; }
    }
    /// <summary>
    /// Interaction logic for TrackingWindow.xaml
    /// </summary>
    public partial class TrackingWindow : Window
    {


        public TrackingWindowData data
        {
            get { return (TrackingWindowData)GetValue(dataProperty); }
            set { SetValue(dataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty dataProperty =
            DependencyProperty.Register("data", typeof(TrackingWindowData), typeof(TrackingWindow));

        public TrackingWindow(OrderTracking _order)
        {
            int length = _order.Tracker!.Count;
            data = new TrackingWindowData {
                Order = _order,
                OrderDateStr = string.Join(Environment.NewLine, _order.Tracker![0]),
                ShipDateStr = length > 1 ? string.Join(Environment.NewLine, _order.Tracker[1]) : "",
                DeliveryDateStr = length > 2 ? string.Join(Environment.NewLine, _order.Tracker[2]) : ""
            };
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
    }
}

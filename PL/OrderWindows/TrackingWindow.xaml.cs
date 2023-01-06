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

namespace PL.OrderWindows;

/// <summary>
/// A class that contains data for the tracking window.
/// </summary>
public class TrackingWindowData
{
    /// <summary>
    /// The order being tracked.
    /// </summary>
    public BO.OrderTracking? Order { get; set; }

    /// <summary>
    /// The string representation of the order date.
    /// </summary>
    public string? OrderDateStr { get; set; }

    /// <summary>
    /// The string representation of the ship date.
    /// </summary>
    public string? ShipDateStr { get; set; }

    /// <summary>
    /// The string representation of the delivery date.
    /// </summary>
    public string? DeliveryDateStr { get; set; }
}

/// <summary>
/// Interaction logic for TrackingWindow.xaml
/// </summary>
public partial class TrackingWindow : Window
{

    /// <summary>
    /// Gets or sets the data for the tracking window.
    /// </summary>
    public TrackingWindowData data
    {
        get { return (TrackingWindowData)GetValue(dataProperty); }
        set { SetValue(dataProperty, value); }
    }

    // Using a DependencyProperty as the backing store for data.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty dataProperty =
        DependencyProperty.Register("data", typeof(TrackingWindowData), typeof(TrackingWindow));

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackingWindow"/> class.
    /// </summary>
    /// <param name="_order">The order to track.</param>
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


    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
}

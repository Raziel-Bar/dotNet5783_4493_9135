using PL.OrderWindows;
using PL.ProductWindows;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// the main window of the app
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// moving to the admin product list window
        /// </summary>
        /// <param name="sender">the admin button</param>
        /// <param name="e">mouse click</param>
        private void AdminAccessButton_Click(object sender, RoutedEventArgs e)
        {
            AdminVerificationWindow verificationWindow = new AdminVerificationWindow();
            verificationWindow.ShowDialog();
            if (verificationWindow.isVerified)
            {
                this.Close();
            }
        }

        /// <summary>
        /// exiting the app
        /// </summary>
        /// <param name="sender">the exit button</param>
        /// <param name="e">mouse click</param>
        private void Exit(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// Event handler for when the "New Order" button is clicked.
        /// Opens a new window for creating a new order.
        /// Closes the current window.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">The event arguments for the button click event.</param>
        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            new NewOrderWindow(new()).Show();
            this.Close();
        }

        /// <summary>
        /// Event handler for when the "Order Trace" button is clicked.
        /// Opens a new window for tracking an order.
        /// Closes the current window.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">The event arguments for the button click event.</param>
        private void OrderTraceButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderTrackingWindow().Show();
            this.Close();
        }
    }
}

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
            //new AdminVerificationWindow().ShowDialog();
            new AdminWindow().Show(); // to be deleted once enabling the verify window
            this.Close();
        }

        /// <summary>
        /// exiting the app
        /// </summary>
        /// <param name="sender">the exit button</param>
        /// <param name="e">mouse click</param>
        private void Exit(object sender, RoutedEventArgs e) => this.Close();

        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrderTraceButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderTrackingWindow().Show();
            this.Close();
        }
    }
}

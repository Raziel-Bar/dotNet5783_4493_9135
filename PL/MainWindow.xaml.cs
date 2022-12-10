using BlApi;
using BlImplementation;
using PL.ProductWindows;
using System.Windows;

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
            IBl bl = new Bl();
        }

        /// <summary>
        /// moving to the admin product list window
        /// </summary>
        /// <param name="sender">the admin button</param>
        /// <param name="e">mouse click</param>
        private void AdminAccess_Click(object sender, RoutedEventArgs e)
        {
            new ProductForListWindow().Show();
           
            this.Close();
        }

        /// <summary>
        /// exiting the app
        /// </summary>
        /// <param name="sender">the exit button</param>
        /// <param name="e">mouse click</param>
        private void Exit(object sender, RoutedEventArgs e) => this.Close();
    }
}

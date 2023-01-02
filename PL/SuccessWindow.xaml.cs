using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

public class SuccessWindowData
{
    public string? TextMessage { get; set; }
}

namespace PL
{
    /// <summary>
    /// Interaction logic for SuccessWindow.xaml
    /// </summary>
    public partial class SuccessWindow : Window
    {
        public SuccessWindowData Data { get; set; }
        
        /// <summary>
        /// a message window that confirms the success of an action
        /// </summary>
        /// <param name="text">The corresponding message</param>
        public SuccessWindow(string text)
        {
            SystemSounds.Beep.Play(); // check later
            Data = new () { 
                TextMessage = text
            };
            InitializeComponent();
        }

        /// <summary>
        /// closing window
        /// </summary>
        /// <param name="sender">the ok button</param>
        /// <param name="e">mouse click</param>
        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();
    }
}

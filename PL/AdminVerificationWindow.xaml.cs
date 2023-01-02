using PL.ProductWindows;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminVerificationWindow.xaml
    /// </summary>
    public partial class AdminVerificationWindow : Window
    {
        private string password = "1234";
        public AdminVerificationWindow()
        {
            InitializeComponent();
        }

        private void VerifyPassword_Click(object sender, RoutedEventArgs e)
        {
            var pwInput = (PasswordBox)sender;
            if (pwInput.Password != password)
            {
                SystemSounds.Beep.Play();
            }
            else
            {
                new AdminWindow().Show();
                this.Close();
            }
        }

        private void EnterKeyCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VerifyPassword_Click(sender, e);
            }
        }
    }
}

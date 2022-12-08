﻿using BlApi;
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

        public MainWindow()
        {
            InitializeComponent();
            IBl bl = new Bl();
        }

        private void AdminAccess_Click(object sender, RoutedEventArgs e)
        {
            new ProductForListWindow().Show();
            this.Close();
        }

        private void Exit(object sender, RoutedEventArgs e) => this.Close();
    }
}

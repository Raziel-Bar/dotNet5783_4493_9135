﻿using System;
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
    /// <summary>
    /// Interaction logic for OrderDetailsUser.xaml
    /// </summary>
    public partial class OrderDetailsUser : Window
    {
        // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderProperty =
            DependencyProperty.Register("TOrder", typeof(BO.Order), typeof(OrderUpdateWindow));
        public BO.Order TOrder
        {
            get { return (BO.Order)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public OrderDetailsUser(BO.Order _order)
        {
            TOrder = _order;
            InitializeComponent();
        }

        private void BackToOrderTracking_Button_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
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

namespace CM_Lab2_WPF
{
    /// <summary>
    /// Логика взаимодействия для AddElementWindow.xaml
    /// </summary>
    public partial class AddElementWindow : Window
    {
        public static Window instance;
        public AddElementWindow()
        {
            InitializeComponent();
        }

        private void Apply_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Owner as MainWindow;
            //mw.InvokeCreation()
        }

        private void Deny_ButtonClick(object sender, RoutedEventArgs e)
        {
            Owner.IsEnabled = true;
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            double res;
            if(!double.TryParse(tb.Text, out res))
                MessageBox.Show("Incorect inputing tau. Try to input double value", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            tb.Text = "1,0";
        }
    }
}

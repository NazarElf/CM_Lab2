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

namespace CM_Lab2_WPF
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public MyMessageBoxResult result = MyMessageBoxResult.None;

        public CustomMessageBox(string text, string caption, MyMessageBoxButton buttons, MyMessageBoxImage icon, MyMessageBoxResult defaultResult)
        {
            InitializeComponent();
            result = MyMessageBoxResult.None;
            Text.Text = text;
            GridLength gl = new GridLength(0, GridUnitType.Star);
            Caption.Content = caption;
            switch (buttons)
            {
                case MyMessageBoxButton.Ok:
                    col2.Width = col3.Width = gl;
                    Yes.IsEnabled = No.IsEnabled = Cancel.IsEnabled = false;
                    Yes.Width = No.Width = Cancel.Width = 0;
                    break;
                case MyMessageBoxButton.OkCancel:
                    col2.Width = gl;
                    Yes.IsEnabled = No.IsEnabled = false;
                    Yes.Width = No.Width = 0;
                    break;
                case MyMessageBoxButton.YesNo:
                    col3.Width = gl;
                    Ok.IsEnabled = Cancel.IsEnabled = false;
                    Ok.Width = Cancel.Width = 0;
                    break;
                case MyMessageBoxButton.YesNoCancel:
                    Ok.IsEnabled = false;
                    Ok.Width = 0;
                    break;
                default:
                    break;
            }
            switch (icon)
            {
                case MyMessageBoxImage.None:
                    break;
                case MyMessageBoxImage.Hand:
                    break;
                case MyMessageBoxImage.Question:
                    break;
                case MyMessageBoxImage.Exclamation:
                    break;
                case MyMessageBoxImage.Asterisk:
                    break;
                default:
                    break;
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            captionRect.Rect = new Rect(0, 0, Caption.ActualWidth, 50);
            captionCanvas.Width = Caption.ActualWidth;
            bgRect.Rect = new Rect(0, 0, bgGrid.ActualWidth, bgGrid.ActualHeight);
            this.Width = main.ActualWidth + 10;
            this.Height = main.ActualHeight + 10;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Top = (screenHeight / 2) - (this.Height / 2);
            this.Left = (screenWidth / 2) - (this.Width / 2);
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            result = MyMessageBoxResult.Yes;
            this.Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            result = MyMessageBoxResult.No;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            result = MyMessageBoxResult.Cancel;
            this.Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            result = MyMessageBoxResult.Ok;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void captionCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

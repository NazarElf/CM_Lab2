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

        }

        private void Deny_ButtonClick(object sender, RoutedEventArgs e)
        {
            Owner.IsEnabled = true;
            this.Close();
        }
    }
}

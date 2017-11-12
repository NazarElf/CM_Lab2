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
    /// Interaction logic for InputRelativeProbability.xaml
    /// </summary>
    public partial class InputRelativeProbability : Window
    {
        public InputRelativeProbability(string from, string to)
        {
            InitializeComponent();
            string res = Title.Content as string;
            Title.Content = res + $"from {from} to {to}";
        }

        public double res;
        private bool canClose = false;

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (double.TryParse(textBox.Text, out res))
                {
                    canClose = true;
                    this.Close();
                }
                else MyMessageBox.Show("Something Goes Wrong", "ERROR", MyMessageBoxButton.Ok, MyMessageBoxImage.Error);

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!canClose)
                e.Cancel = true;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
         /*   if (e.Key == Key.Enter)
                if (double.TryParse(textBox.Text, out res))
                    this.Close();
                else MyMessageBox.Show("Something Goes Wrong", "ERROR", MyMessageBoxButton.Ok, MyMessageBoxImage.Error);
                */
        }
    }
}

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
    /// Логика взаимодействия для AddTransition.xaml
    /// </summary>
    public partial class AddTransition : Window
    {
        DeviceNode dn;
        public AddTransition(DeviceNode dn)
        {
            InitializeComponent();
            this.dn = dn;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Owner.IsEnabled = true;
        }

        Dictionary<Label, ListBoxItem> AccordanceID = new Dictionary<Label, ListBoxItem>();

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in (Owner as MainWindow).Accordance)
            {
                bool isAlreadyExist = false;
                foreach (var next in dn.Transition.Keys)
                {
                    if (item.Value.Equals(next))
                    {
                        isAlreadyExist = true;
                        break;
                    }
                }
                if (!isAlreadyExist)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    StackPanel sp = item.Key.Content as StackPanel;
                    Label l = sp.Children[0] as Label;
                    string name = l.Content.ToString();
                    RadialGradientBrush rgb = l.Background as RadialGradientBrush;
                    GradientStopCollection gsp = rgb.GradientStops;
                    foreach (var stop in gsp)
                    {
                        if (stop.Color != Colors.White && stop.Color != Color.FromArgb(0,0,0,0))
                        {
                            l = new Label();
                            l.Background = rgb;
                            l.Content = name;
                            l.Width = 200;
                            l.Height = 40;
                            l.VerticalContentAlignment = VerticalAlignment.Center;
                            l.HorizontalAlignment = HorizontalAlignment.Center;
                            l.HorizontalContentAlignment = HorizontalAlignment.Center;
                            lbi.Content = l;
                            lbi.MouseDoubleClick += new MouseButtonEventHandler(Adding);
                            lbi.Background = new LinearGradientBrush(new GradientStopCollection
                            {
                                new GradientStop(Colors.Black, 0),
                                new GradientStop(Colors.Black, 1),
                                new GradientStop(Color.FromArgb(0,0,0,0), 0.1),
                                new GradientStop(Color.FromArgb(0,0,0,0), 0.9)
                            },  new Point(0.5, 0), new Point(0.5, 1));
                            AddListBox.Items.Add(lbi);
                            AccordanceID.Add(l, item.Key);
                        }
                    }
                }
            }
            int i = 0;
            ++i;
        }

        private void Adding(object sender, MouseButtonEventArgs e)
        {
            Owner.IsEnabled = true;
            if (AddListBox.Items.Count == 0 || AddListBox.SelectedItems == null)
                return;
            foreach (var item in (Owner as MainWindow).Accordance)
            {
                if(item.Key == AccordanceID[(AddListBox.SelectedItem as ListBoxItem).Content as Label])
                {
                    dn.AddTransition(item.Value, 0);
                }
            }
            this.Close();
        }

        private void Deny_ButtonClick(object sender, RoutedEventArgs e)
        {
            Owner.IsEnabled = true;
            this.Close();
        }

        private void Apply_ButtonClick(object sender, RoutedEventArgs e)
        {
            Owner.IsEnabled = true;
            if (AddListBox.Items.Count == 0 || AddListBox.SelectedItems == null)
                return;
            foreach (var item in (Owner as MainWindow).Accordance)
            {
                if (item.Key == AccordanceID[(AddListBox.SelectedItem as ListBoxItem).Content as Label])
                {
                    dn.AddTransition(item.Value, 0);
                }
            }
            (Owner as MainWindow).InvokeUpdate(dn);
            this.Close();
        }
    }
}

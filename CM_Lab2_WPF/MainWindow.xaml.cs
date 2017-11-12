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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CM_Lab2_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DeviceNode CP = new DeviceNode(1.0, "Central Processor", 6, 2);
            TransitionsDict.Add(MenuItem_From_ListBoxItem(CD), CP.Transition);
            CP.Transition.Add(CP, 1.0);
            Accordance.Add(CD, CP);
            
            #region nice trash
            /*
            ListBoxItem lbi = new ListBoxItem();
            Grid newGrid = new Grid();

            newGrid.MinWidth = newGrid.Width = 100;
            newGrid.MinHeight = newGrid.Height = 50;
            newGrid.Background = Brushes.Black;
            Ellipse el1 = new Ellipse();
            Ellipse el2 = new Ellipse();
            Ellipse el3 = new Ellipse();
            Rectangle r1 = new Rectangle();
            Rectangle r2 = new Rectangle();
            el1.HorizontalAlignment = el2.HorizontalAlignment = el3.HorizontalAlignment = r1.HorizontalAlignment = r2.HorizontalAlignment = HorizontalAlignment.Left;
            el1.VerticalAlignment = el2.VerticalAlignment = el3.VerticalAlignment = r1.VerticalAlignment = r2.VerticalAlignment = VerticalAlignment.Top;
            el1.Width = el1.Height = el2.Width = el2.Height = el3.Width = el3.Height = r2.Width = r2.Height = r1.Height = 20;
            r1.Width = 10;
            el2.Margin = new Thickness(42, 0, 0, 0);
            el3.Margin = new Thickness(42, 26, 0, 0);
            r1.Margin = new Thickness(26, 0, 0, 0);
            r2.Margin = new Thickness(68, 0, 0, 0);
            el1.Fill = el2.Fill = el3.Fill = Brushes.Blue;
            Brush b1 = new LinearGradientBrush(Colors.Blue, Colors.DarkBlue, 0);
            Brush b2 = new LinearGradientBrush(Colors.DarkBlue, Colors.Black, 0);
            r2.Fill = b1;
            r1.Fill = b2;
            r1.Stroke = Brushes.LightBlue;
            r2.Stroke = Brushes.LightBlue;
            el1.Stroke = Brushes.LightBlue;
            el2.Stroke = Brushes.LightBlue;
            el3.Stroke = Brushes.LightBlue;
            Line l1 = new Line();
            l1.Y1 = l1.Y2 = 10;
            l1.X1 = 0; l1.X2 = 68;
            l1.StrokeThickness = 1.5;
            l1.Stroke = Brushes.LightBlue;
            Path p = new Path();
            p.Stroke = Brushes.LightBlue;
            p.StrokeThickness = 1.5;
            p.Data = new PathGeometry(new PathFigureCollection { new PathFigure(new Point(26, 15), new PathSegmentCollection { new BezierSegment(new Point(20, 15), new Point(20, 37), new Point(53, 37), true), new BezierSegment(new Point(100, 37), new Point(100, 15), new Point(85, 15), true) }, false) });
            newGrid.Children.Add(p);
            newGrid.Children.Add(l1);
            newGrid.Children.Add(r1);
            newGrid.Children.Add(r2);
            newGrid.Children.Add(el1);
            newGrid.Children.Add(el2);
            newGrid.Children.Add(el3);
            lbi.Content = newGrid;
            listBox.Items.Add(lbi);*/
            #endregion
        }

        Dictionary<ListBoxItem, DeviceNode> Accordance = new Dictionary<ListBoxItem, DeviceNode>();
        Dictionary<MenuItem, Dictionary<DeviceNode, double>> TransitionsDict = new Dictionary<MenuItem, Dictionary<DeviceNode, double>>();
        DeviceNode SelectToAddTransition;
        bool RemoveItem = false;
        ListBoxItem lbiInvoker;
        Random r = new Random();
        public static double connectorDouble;
        public static string connectorString;

        void StartProcessing(params DeviceNode[] Nodes)
        {
            //to start every every task from queue if exist
            foreach (var item in Nodes)
            {
                item.StartTask();
            }
            //tracking system time
            double sysTime = 0.0;
            for (int i = 0; i < 10000; i++)
            {
                //to find minimum value first comparing with infinity
                double min = double.PositiveInfinity;
                foreach (var item in Nodes)
                {
                    double t = item.GetLowestTime();
                    if (min > t)
                        min = t;
                }
                //substract from every task minimum left
                foreach (var item in Nodes)
                    item.SubstractTime(min);
                //end every task that ended in this time
                foreach (var item in Nodes)
                    item.EndTask();
                //tracking system time
                sysTime += min;
            }


            //counting statistic
            string res = "";
            foreach (var node in Nodes)
            {
                for (int i = 0; i < node.busyTime.Length; i++)
                {
                    node.busyTime[i] /= sysTime;
                    res += $"usage of {node.name} => core[{i}] = {(node.busyTime[i] * 100).ToString("0.00")}%\n";
                }
            }
            MyMessageBox.Show(res, "Usages", MyMessageBoxButton.Ok, MyMessageBoxImage.Nuclear);
           
            //free data from devices
            foreach (var item in Nodes)
            {
                item.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeviceNode CP = new DeviceNode(1, "Central Processor", 6, 2);
            DeviceNode NB = new DeviceNode(2, "North Bridge");
            DeviceNode OM = new DeviceNode(3, "Operating Memory");
            DeviceNode SB = new DeviceNode(3, "South Bridhe");
            DeviceNode CD = new DeviceNode(5, "Disk Controller");
            CP.AddTransition(CP, 0.5);
            CP.AddTransition(NB, 0.5);

            NB.AddTransition(OM, 0.34);
            NB.AddTransition(CP, 0.33);
            NB.AddTransition(SB, 0.33);

            OM.AddTransition(OM, 0.6);
            OM.AddTransition(NB, 0.4);

            SB.AddTransition(SB, 0.33);
            SB.AddTransition(CD, 0.34);
            SB.AddTransition(NB, 0.33);

            CD.AddTransition(CD, 0.5);
            CD.AddTransition(NB, 0.5);

            StartProcessing(CP, NB, OM, SB, CD);
        }

        public void InvokeCreation(string name, double tau)
        {
            ListBoxItem lbi;
            CreatingNewDevice(name, tau, out lbi);
            listBox.Items.Add(lbi);
            Accordance.Add(lbi, new DeviceNode(tau, name));
            TransitionsDict.Add(MenuItem_From_ListBoxItem(lbi), Accordance[lbi].Transition);
        }

        private void CreatingNewDevice(string name, double tau, out ListBoxItem listBoxItem)
        {
            listBoxItem = new ListBoxItem();
            StackPanel itemBase = new StackPanel();
            Label nameLabel = new Label();
            TextBox tauTextBox = new TextBox();
            Button addButton = new Button();
                TextBlock addTextBlock = new TextBlock();
            Button removeButton = new Button();
                TextBlock removeTextBlock = new TextBlock();
            Menu transitions = new Menu();
                MenuItem nameMenu = new MenuItem();
            Brush background = this.FindResource("Buttons") as LinearGradientBrush;
            Style buttonStyle = this.FindResource("BeautyButton") as Style;
            Style textBoxStyle = this.FindResource("BeautyTextBox") as Style;

            listBoxItem.Padding = new Thickness(0);
            listBoxItem.Height = 40;

            itemBase.Height = 40;
            itemBase.Orientation = Orientation.Horizontal;

            nameLabel.Content = name;
            nameLabel.HorizontalAlignment = HorizontalAlignment.Left;
            nameLabel.VerticalContentAlignment = VerticalAlignment.Center;
            nameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            nameLabel.Width = 122;
            nameLabel.Foreground = Brushes.Black;
            nameLabel.FontFamily = new FontFamily("Consolas");
            nameLabel.FontWeight = FontWeights.Bold;
            Color randomColor = HsvToRgb(r.Next(360), 100, 100);
            Brush labelEffectBackground = new RadialGradientBrush(new GradientStopCollection
            {
                new GradientStop(Color.FromArgb(0,0,0,0), 1),
                new GradientStop(Colors.White, 0.49),
                new GradientStop(randomColor, 0.66)
            });
            nameLabel.Background = labelEffectBackground;

            tauTextBox.Text = tau.ToString();
            tauTextBox.Width = 80;
            tauTextBox.BorderThickness = new Thickness(2);
            tauTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF,0x41,0x66,0x6C));
            tauTextBox.ToolTip = "Tau";
            tauTextBox.Margin = new Thickness(0, 5, 0, 5);
            tauTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            tauTextBox.Background = background;
            tauTextBox.KeyUp += new KeyEventHandler(TextBox_KeyUp);
            tauTextBox.Style = textBoxStyle;

            transitions.Margin = new Thickness(0, 5, 0, 5);
            transitions.Padding = new Thickness(0);
            transitions.HorizontalAlignment = HorizontalAlignment.Left;
            transitions.Width = 78; 
            transitions.Background = background;
            transitions.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x41, 0x66, 0x6C));
            transitions.BorderThickness = new Thickness(2);
            //transitions.Height = 36;

            nameMenu.Header = "Transitions";
            nameMenu.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            nameMenu.VerticalContentAlignment = VerticalAlignment.Stretch;
            nameMenu.Width = 74; nameMenu.Height = 28;
            nameMenu.FontWeight = FontWeights.SemiBold;

            addButton.Margin = new Thickness(0, 5, 0, 5);
            addButton.HorizontalAlignment = HorizontalAlignment.Center;
            addButton.Padding = new Thickness(4.5);
            addButton.VerticalContentAlignment = VerticalAlignment.Center;
            addButton.Content = "Add Transition";
            addButton.Style = buttonStyle;
            addButton.Click += new RoutedEventHandler(AddTransition_ButtonClick);

            removeButton.Margin = new Thickness(0, 5, 0, 5);
            removeButton.HorizontalAlignment = HorizontalAlignment.Center;
            removeButton.Padding = new Thickness(4);
            removeButton.VerticalContentAlignment = VerticalAlignment.Center;
            removeButton.Content = "Remove Transition";
            removeButton.Style = buttonStyle;
            removeButton.Width = 0;

            transitions.Items.Add(nameMenu);

            Separator s0 = new Separator(), s1 = new Separator(), s2 = new Separator(), s3 = new Separator();
            s0.Width = s1.Width = s2.Width = s3.Width = 4;
            s0.Background = s1.Background = s2.Background = s3.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            itemBase.Children.Add(nameLabel);
            itemBase.Children.Add(s0);
            itemBase.Children.Add(tauTextBox);
            itemBase.Children.Add(s1);
            itemBase.Children.Add(transitions);
            itemBase.Children.Add(s2);
            itemBase.Children.Add(addButton);
            itemBase.Children.Add(s3);
            itemBase.Children.Add(removeButton);

            listBoxItem.Background = new LinearGradientBrush(new GradientStopCollection
            {
                new GradientStop(Colors.Black, 0),
                new GradientStop(Colors.Black, 1),
                new GradientStop(Color.FromArgb(0,0,0,0), 0.1),
                new GradientStop(Color.FromArgb(0,0,0,0), 0.9)
            }, new Point(0.5, 0), new Point(0.5, 1));

            listBoxItem.Content = itemBase;
            listBoxItem.MouseDoubleClick += new MouseButtonEventHandler(lbi_DoubleClick);
            //listBoxItem.MouseDoubleClick += new MouseEventHandler(lbi_DoubleClick);
        }

        private static Color HsvToRgb(int hue, int saturation, int value)
        {
            double h = hue;
            double s = saturation / 100.0;
            double v = value / 100.0;
            double r = 0, g = 0, b = 0;
            if (s == 0)
                r = g = b = v;
            else
            {
                double sector = h / 60;
                int sectorNumber = (int)Math.Floor(sector);
                double sectorPart = sector - sectorNumber;
                double p = v * (1 - s);
                double q = v * (1 - (s * sectorPart));
                double t = v * (1 - (s * (1 - sectorPart)));
                switch (sectorNumber)
                {
                    case 0:
                        r = v; g = t; b = p;
                        break;
                    case 1:
                        r = q; g = v; b = p;
                        break;
                    case 2:
                        r = p; g = v; b = t;
                        break;
                    case 3:
                        r = p; g = q; b = v;
                        break;
                    case 4:
                        r = t; g = p; b = v;
                        break;
                    case 5:
                        r = v; g = p; b = q;
                        break;
                }
            }
            return Color.FromArgb(0x92, (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddElementWindow aew = new AddElementWindow();
            //this.IsEnabled = false;
            aew.Owner = this;
            aew.Show();
            /*double tauInput = 1.2;
            ListBoxItem newItem;
            CreatingNewDevice("New", tauInput, out newItem);
            listBox.Items.Add(newItem);
            Accordance.Add(newItem, new DeviceNode(tauInput));*/
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;
                StackPanel sp = tb.Parent as StackPanel;
                ListBoxItem lbi = sp.Parent as ListBoxItem;
                double res;
                if (double.TryParse(tb.Text, out res))
                {
                    Accordance[lbi].SetTau(res);
                }
                else
                    tb.Text = Accordance[lbi].GetTau().ToString();
            }
        }

        private MenuItem MenuItem_From_ListBoxItem(ListBoxItem lbiIn)
        {
            return (((lbiIn.Content as StackPanel).Children[4] as Menu).Items[0] as MenuItem);
        }

        private void lbi_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SelectToAddTransition != null && lbiInvoker != null)
            {
                try
                {
                    double prob = 1.0;
                    InputRelativeProbability irp = new InputRelativeProbability(SelectToAddTransition.name, Accordance[sender as ListBoxItem].name);
                    irp.ShowDialog();
                    prob = irp.res;
                    SelectToAddTransition.Transition.Add(Accordance[sender as ListBoxItem], prob);

                    Label l = ((sender as ListBoxItem).Content as StackPanel).Children[0] as Label;
                    Brush labelBG = l.Background.Clone();
                    string name = l.Content as string;
                    l = new Label();
                    l.Background = labelBG;
                    l.Content = name;
                    l.Height = 30;
                    l.Width = 200;
                    l.HorizontalContentAlignment = HorizontalAlignment.Center;
                    l.VerticalContentAlignment = VerticalAlignment.Center;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(l);
                    sp.Width = 275;
                    sp.Orientation = Orientation.Horizontal;

                    Label probabilityLabel = new Label();
                    probabilityLabel.Content = prob.ToString();

                    Separator s0 = new Separator();
                    s0.Width = 3;
                    s0.Background = Brushes.Transparent;

                    sp.Children.Add(s0);
                    sp.Children.Add(probabilityLabel);

                    MenuItem_From_ListBoxItem(lbiInvoker).Items.Add(sp);
                    SelectToAddTransition = null;
                    lbiInvoker = null;
                    listBox.Cursor = Cursors.Arrow;
                    cancelButton.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MyMessageBox.Show(ex.Message);
                }
                addButton.IsEnabled = true;
                removeButton.IsEnabled = true;
            }
            if(RemoveItem)
            {
                //DeviceNode forDelete = Accordance[sender as ListBoxItem];
                ////remove from transitions
                ///*foreach (var spDict in TransitionsDict)
                //{
                //    foreach (var trnsDict in spDict.Value)
                //    {
                //        if(forDelete == trnsDict.Key)
                //        {
                //
                //        }
                //    }
                //}*/
                ////remove from dictionary
                //
                ////remove from list
                //listBox.Cursor = Cursors.Arrow;
            }
        }

        private void AddTransition_ButtonClick(object sender, RoutedEventArgs e)
        {
            MyMessageBox.Show("Double click on list item to add transition", "Add Transition");
            SelectToAddTransition = Accordance[(((sender as Button).Parent as StackPanel).Parent as ListBoxItem)];
            lbiInvoker = (((sender as Button).Parent as StackPanel).Parent as ListBoxItem);
            listBox.Cursor = Cursors.UpArrow;
            addButton.IsEnabled = false;
            removeButton.IsEnabled = false;
            cancelButton.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /*cancelButton.IsEnabled = true;
            listBox.Cursor = Cursors.Hand;*/
            Accordance = new Dictionary<ListBoxItem, DeviceNode>();
            TransitionsDict = new Dictionary<MenuItem, Dictionary<DeviceNode, double>>();
            listBox.Items.Clear();
            InvokeCreation("Central Processor", 1);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectToAddTransition = null;
            lbiInvoker = null;
            RemoveItem = false;
            addButton.IsEnabled = true;
            removeButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
            listBox.Cursor = Cursors.Arrow;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            uint res;
            if (uint.TryParse(Count.Text, out res))
            {
                Accordance[listBox.Items.GetItemAt(0) as ListBoxItem].queue = res;
                StartProcessing(Accordance.Values.ToArray());
            }
            else
                MyMessageBox.Show("Input unsigned integer", "Queue", MyMessageBoxButton.Ok,MyMessageBoxImage.Error);
        }
    }
}

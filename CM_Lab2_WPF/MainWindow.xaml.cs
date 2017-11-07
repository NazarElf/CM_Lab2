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
            Slider1.MouseWheel += new MouseWheelEventHandler(Slider_MouseWheel);
            Slider1.ValueChanged += new RoutedPropertyChangedEventHandler<double>(Slider_ValueChanged);
            Slider1.IsEnabled = false;

            DeviceNode CP = new DeviceNode(1.0, 6, 2);
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
        Random r = new Random();

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
            //TODO: here i should count statistic
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeviceNode CP = new DeviceNode(1, 6, 2);
            DeviceNode NB = new DeviceNode(2);
            DeviceNode OM = new DeviceNode(3);
            DeviceNode SB = new DeviceNode(3);
            DeviceNode CD = new DeviceNode(5);
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

        private void CreatingNewDevice(string name, double tau, out ListBoxItem listBoxItem)
        {
            listBoxItem = new ListBoxItem();
            Grid itemBase = new Grid();
            TextBox tauTextBox = new TextBox();
            Label nameLabel = new Label();
            Button addButton = new Button();
                TextBlock addTextBlock = new TextBlock();
            Button removeButton = new Button();
                TextBlock removeTextBlock = new TextBlock();
            Menu transitions = new Menu();
                MenuItem nameMenu = new MenuItem();
            Brush style = this.FindResource("Buttons") as LinearGradientBrush;


            //if (first)
            //{
            //    first = false;
            //    listBox.Resources.Add(SystemColors.HighlightBrushKey, new LinearGradientBrush(new GradientStopCollection
            //                {
            //    new GradientStop(Color.FromArgb(0x00, 0x00, 0x00, 0x00), 0),
            //    new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.5),
            //    new GradientStop(Color.FromArgb(0x00, 0x00, 0x00, 0x00), 1)
            //}, new Point(0.5, 0), new Point(0.5, 1)));
            //    listBox.Resources.Add(SystemColors.InactiveSelectionHighlightBrushKey, new LinearGradientBrush(new GradientStopCollection
            //                {
            //    new GradientStop(Color.FromArgb(0x00, 0x00, 0x00, 0x00), 0),
            //    new GradientStop(Color.FromArgb(0xFF, 0x4B, 0x4B, 0x4B), 0.5),
            //    new GradientStop(Color.FromArgb(0x00, 0x00, 0x00, 0x00), 1)
            //}, new Point(0.5, 0), new Point(0.5, 1)));
            //}

            itemBase.Height = 40;

            tauTextBox.Text = tau.ToString();
            tauTextBox.Margin = new Thickness(127, 2, 42, 2);
            tauTextBox.BorderThickness = new Thickness(2);
            Color c = new Color(); c.A = 0xFF; c.R = 0x41; c.G = 0x66; c.B = 0x6C;
            tauTextBox.BorderBrush = new SolidColorBrush(c);
            tauTextBox.ToolTip = "Tau";
            tauTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            tauTextBox.Background = style;
            tauTextBox.KeyUp += new KeyEventHandler(TextBox_KeyUp);

            nameLabel.Content = name;
            nameLabel.HorizontalAlignment = HorizontalAlignment.Left;
            nameLabel.VerticalAlignment = VerticalAlignment.Top;
            nameLabel.VerticalContentAlignment = VerticalAlignment.Center;
            nameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            nameLabel.Height = 36;
            nameLabel.Margin = new Thickness(0, 2, 0, 0);
            nameLabel.Width = 122;
            nameLabel.Foreground = Brushes.Black;
            nameLabel.FontFamily = new FontFamily("Consolas");
            nameLabel.FontWeight = FontWeights.Bold;
            Color randomColor = HsvToRgb(r.Next(360), 100, 100);
            GradientStopCollection backGroundCollection = new GradientStopCollection
            {
                new GradientStop(Color.FromArgb(0,0,0,0), 1),
                new GradientStop(Colors.White, 0.49),
                new GradientStop(randomColor, 0.66)
            };
            Brush labelEffectBackground = new RadialGradientBrush(backGroundCollection);
            nameLabel.Background = labelEffectBackground;

            addButton.Margin = new Thickness(624, 0, -456, 0);
            addButton.Background = style;
            addTextBlock.TextWrapping = TextWrapping.Wrap;
            addTextBlock.Text = "Add Transition";
            addButton.Content = addTextBlock;

            removeButton.Margin = new Thickness(532, 0, -364, 0);
            removeButton.Background = style;
            removeTextBlock.TextWrapping = TextWrapping.Wrap;
            removeTextBlock.Text = "Remove Transition";
            removeTextBlock.Height = 30;
            removeTextBlock.Width = 77;
            removeTextBlock.TextAlignment = TextAlignment.Center;
            removeTextBlock.FontSize = 11;
            removeButton.Content = removeTextBlock;

            transitions.HorizontalAlignment = HorizontalAlignment.Left;
            transitions.Margin = new Thickness(218, 2, -272, 0);
            transitions.VerticalAlignment = VerticalAlignment.Top;
            transitions.Width = 309; transitions.Height = 36;
            transitions.Background = style;

            nameMenu.Header = "Transitions";
            nameMenu.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            nameMenu.VerticalContentAlignment = VerticalAlignment.Stretch;
            nameMenu.Width = 309; nameMenu.Height = 36;
            nameMenu.FontSize = 22;
            nameMenu.FontWeight = FontWeights.SemiBold;

            nameMenu.Items.Add("_Text");

            transitions.Items.Add(nameMenu);

            itemBase.Children.Add(tauTextBox);
            itemBase.Children.Add(nameLabel);
            itemBase.Children.Add(addButton);
            itemBase.Children.Add(removeButton);
            itemBase.Children.Add(transitions);

            listBoxItem.Background = new LinearGradientBrush(new GradientStopCollection
            {
                new GradientStop(Colors.Black, 0),
                new GradientStop(Colors.Black, 1),
                new GradientStop(Color.FromArgb(0,0,0,0), 0.1),
                new GradientStop(Color.FromArgb(0,0,0,0), 0.9)
            }, new Point(0.5, 0), new Point(0.5, 1));

            listBoxItem.Content = itemBase;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string s = String.Format("{0:0.0000}",e.NewValue / 100.0);
            double r = double.Parse(s);
            CP_SliderTextBox.Text = r.ToString();
            //SliderTry.Value = 100.0 * (1.0 - r);
        }

        private void Slider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int i = e.Delta;
            Slider s = sender as Slider;
            s.Value += i / 120;
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

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            double tauInput = 1.2;
            ListBoxItem newItem;
            CreatingNewDevice("New", tauInput, out newItem);
            listBox.Items.Add(newItem);
            Accordance.Add(newItem, new DeviceNode(tauInput));
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;
                Grid g = tb.Parent as Grid;
                ListBoxItem lbi = g.Parent as ListBoxItem;
                double res;
                if (double.TryParse(tb.Text, out res))
                {
                    Accordance[lbi].SetTau(res);
                }
                else
                    tb.Text = Accordance[lbi].GetTau().ToString();
            }
        }
    }
}

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
    }
}

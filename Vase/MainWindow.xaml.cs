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

namespace Vase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int alf = 30, bet = 30, angle = 10;
        double zoom = 0.5, size = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void gridSelect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid selector)
            {
                Point mouse = e.GetPosition(selector);
                selector.Children.Add(new Ellipse
                {
                    Height = size,
                    Width = size,
                    Margin = new Thickness(mouse.X - size / 2, mouse.Y - size / 2, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Fill = Brushes.Red,
                });
            }
        }

        private void btnRender_Click(object sender, RoutedEventArgs e)
        {
            angle = (int) sldrAngle.Value;
            Draw();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            gridSelect.Children.Clear();
            Draw();
        }

        void Draw()
        {
            gridik.Children.Clear();
            Line line = new Line();

            List<Point> body = gridSelect.Children.Cast<Ellipse>().Select(x => new Point(x.Margin.Left + size / 2, 150 - x.Margin.Top + size / 2)).OrderBy(x => x.Y).ToList();
            if (body.Count > 0)
            {
                for (int j = 0; j < 360; j += angle)
                {
                    Point point1 = Axon3Dto2D(alf, bet, body[0].X, 0, body[0].Y, zoom, "z", j);
                    for (int i = 1; i < body.Count; i++)
                    {
                        Point point2 = Axon3Dto2D(alf, bet, body[i].X, 0, body[i].Y, zoom, "z", j);
                        gridik.Children.Add(new Line
                        {
                            Stroke = Brushes.Red,
                            X1 = point1.X,
                            Y1 = point1.Y,
                            X2 = point2.X,
                            Y2 = point2.Y
                        });
                        point1 = point2;
                    }

                    for (int i = 0; i < body.Count; i++)
                    {
                        Point point3 = Axon3Dto2D(alf, bet, body[i].X, 0, body[i].Y, zoom, "z", j);
                        Point point4 = Axon3Dto2D(alf, bet, body[i].X * Math.Cos(angle * Math.PI / 180) , body[i].X * Math.Sin(angle * Math.PI / 180), body[i].Y, zoom, "z", j);
                        gridik.Children.Add(new Line
                        {
                            Stroke = Brushes.Red,
                            X1 = point3.X,
                            Y1 = point3.Y,
                            X2 = point4.X,
                            Y2 = point4.Y
                        });
                    }
                }
            }
        }

        public Point Axon3Dto2D(int alfa, int beta, double x, double y, double z, double zoom, string osaOtoc, int uhelOtoc)
        {
            double X, Y, Z;
            double radUhelOtoc = uhelOtoc * Math.PI / 180;
            switch (osaOtoc)
            {
                case "x":
                    X = x;
                    Y = y * Math.Cos(radUhelOtoc) - (z * Math.Sin(radUhelOtoc));
                    Z = y * Math.Sin(radUhelOtoc) + (z * Math.Cos(radUhelOtoc));
                    break;
                case "y":
                    X = x * Math.Cos(radUhelOtoc) + (z * Math.Sin(radUhelOtoc));
                    Y = y;
                    Z = -x * Math.Sin(radUhelOtoc) + (z * Math.Cos(radUhelOtoc));
                    break;
                case "z":
                    X = x * Math.Cos(radUhelOtoc) - (y * Math.Sin(radUhelOtoc));
                    Y = x * Math.Sin(radUhelOtoc) + (y * Math.Cos(radUhelOtoc));
                    Z = z;
                    break;
                default: X = x; Y = y; Z = z; break;
            }
            Point bod2D = new Point();
            double alfaR = alfa * Math.PI / 180;
            double betaR = beta * Math.PI / 180;

            bod2D.X = -(Math.Cos(alfaR) * X * zoom) + (Math.Cos(betaR) * Y * zoom) + (gridik.ActualWidth / 2);
            bod2D.Y = gridik.ActualHeight - (-Math.Sin(alfaR) * X * zoom - (Math.Sin(betaR) * Y * zoom) + (Z * zoom) + (gridik.ActualHeight / 2));
            return bod2D;
        }
    }
}

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
        int alf = 30, bet = 30, uhel = 10;
        double zoom = 0.5, velikost = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid selector)
            {
                Point mys = e.GetPosition(selector);
                Ellipse kolecko = new Ellipse
                {
                    Height = velikost,
                    Width = velikost,
                    Margin = new Thickness(mys.X - velikost / 2, mys.Y - velikost / 2, 0 ,0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Fill = Brushes.Red,
                };
                selector.Children.Add(kolecko);
            }
        }

        private void btnVykreslit_Click(object sender, RoutedEventArgs e)
        {
            Vykresli();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            gridSelect.Children.Clear();
        }

        void Vykresli()
        {
            gridik.Children.Clear();
            Line cara = new Line();

            List<Point> body = gridSelect.Children.Cast<Ellipse>().Select(x => new Point(x.Margin.Left + velikost / 2, 150 - x.Margin.Top + velikost / 2)).OrderBy(x => x.Y).ToList();
            if (body.Count > 0)
            {
                for (int j = 0; j < 360; j += uhel)
                {
                    Point bod1 = Axon3Dto2D(alf, bet, body[0].X, 0, body[0].Y, zoom, "z", j);
                    for (int i = 1; i < body.Count; i++)
                    {
                        Point bod2 = Axon3Dto2D(alf, bet, body[i].X, 0, body[i].Y, zoom, "z", j);
                        cara = new Line
                        {
                            Stroke = Brushes.Red,
                            X1 = bod1.X,
                            Y1 = bod1.Y,
                            X2 = bod2.X,
                            Y2 = bod2.Y
                        };
                        gridik.Children.Add(cara);
                        bod1 = bod2;
                    }

                    for (int i = 0; i < body.Count; i++)
                    {
                        Point bod3 = Axon3Dto2D(alf, bet, body[i].X, 0, body[i].Y, zoom, "z", j);
                        double X = body[i].X * Math.Cos(uhel * Math.PI / 180) - (0 * Math.Sin(uhel * Math.PI / 180));
                        double Y = body[i].X * Math.Sin(uhel * Math.PI / 180) + (0 * Math.Cos(uhel * Math.PI / 180));
                        Point bod4 = Axon3Dto2D(alf, bet, X, Y, body[i].Y, zoom, "z", j);
                        cara = new Line
                        {
                            Stroke = Brushes.Red,
                            X1 = bod3.X,
                            Y1 = bod3.Y,
                            X2 = bod4.X,
                            Y2 = bod4.Y
                        };
                        gridik.Children.Add(cara);
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

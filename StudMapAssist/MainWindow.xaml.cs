using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace StudMapAssist
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TransformGroup mapControl = new TransformGroup()
        {
            Children = { new TranslateTransform(), new ScaleTransform() }
        };

        private TranslateTransform GetMapMove() => mapControl.Children[0] as TranslateTransform;
        private ScaleTransform GetMapScale() => mapControl.Children[1] as ScaleTransform;
        
        private Point lastPosition;
        private Point currentPosition;

        private Point mapPoint;

        private Point FirstPoint = new Point(double.MaxValue, double.MaxValue);
        private Point SecondPoint = new Point(double.MaxValue, double.MaxValue);

        private PointMarker firstPointMarker;
        private PointMarker secondPointMarker;
        private SolidColorBrush FIRST_MARKER_BRUSH = Brushes.AliceBlue;
        private SolidColorBrush SECOND_MARKER_BRUSH = Brushes.LightGreen;
        private DistanceViewer distanceViewer;

        private bool FirstPointWaiting = false;
        private bool SecondPointWaiting = false;

        private double ZoomLevel = 1;

        private void SetStatus(string msg) => ProgramStatus.Text = msg;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ImageSourceConverter imgSource = new ImageSourceConverter();
                MapImage.SetValue(Image.SourceProperty, imgSource.ConvertFromString("map.jpg"));

                MapPanel.RenderTransform = mapControl;
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't load an image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            distanceViewer = new DistanceViewer(MapPanel, FIRST_MARKER_BRUSH, SECOND_MARKER_BRUSH);
            distanceViewer.Hide();

            firstPointMarker = new PointMarker(MapPanel, 0, 0);
            firstPointMarker.SetFillColor(FIRST_MARKER_BRUSH);
            firstPointMarker.SetStrokeColor(Brushes.Blue);
            firstPointMarker.Hide();

            secondPointMarker = new PointMarker(MapPanel, 0, 0);
            secondPointMarker.SetFillColor(SECOND_MARKER_BRUSH);
            secondPointMarker.SetStrokeColor(Brushes.Green);
            secondPointMarker.Hide();
        }

        private void MapViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point currentPos = e.GetPosition(MainPanel);

            ScaleTransform mapScale = GetMapScale();
            double scaleDelta = 0;

            if (e.Delta > 0)
            {
                if (mapScale.ScaleX < 4.5)
                {
                    // Масштабирование замедляется при приближении

                    scaleDelta = 1.2 / (1 + (ZoomLevel / 10));
                    mapScale.ScaleX *= scaleDelta;
                    mapScale.ScaleY *= scaleDelta;

                    // Для сохранения собственных размеров маркеры удаляются при приближении изображения

                    firstPointMarker.ZoomOut(scaleDelta);
                    secondPointMarker.ZoomOut(scaleDelta);

                    ZoomLevel += 0.027;
                }
                else
                {
                    mapScale.ScaleX = mapScale.ScaleX;
                    mapScale.ScaleY = mapScale.ScaleY;
                }
            }

            if (e.Delta < 0)
            {
                if (mapScale.ScaleX > 1)
                {
                    // Масштабирование ускоряется при отдалении

                    scaleDelta = 1.2 / (1 + (ZoomLevel / 10));
                    mapScale.ScaleX /= scaleDelta;
                    mapScale.ScaleY /= scaleDelta;

                    // Для сохранения собственных размеров маркеры приближаются при отдалении изображения

                    firstPointMarker.ZoomIn(scaleDelta);
                    secondPointMarker.ZoomIn(scaleDelta);

                    ZoomLevel -= 0.027;
                }
                else
                {
                    mapScale.ScaleX = mapScale.ScaleX;
                    mapScale.ScaleY = mapScale.ScaleY;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
                MessageBox.Show($"Current scaleX is: {GetMapScale().ScaleX}\nCurrent scaleY is: {GetMapScale().ScaleY}\nZoomLevel: {ZoomLevel}\nMarkerScale: {firstPointMarker.GetMarkerScale().CenterX}");
        }

        private void MapViewer_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition = Mouse.GetPosition(MainPanel);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                TranslateTransform mapMove = GetMapMove();

                // При сильном масштабировании движение намеренно замедляется
                mapMove.X -= (lastPosition.X - currentPosition.X) / (1.2 / (1 + ZoomLevel / 10));
                mapMove.Y -= (lastPosition.Y - currentPosition.Y) / (1.2 / (1 + ZoomLevel / 10));
            }

            lastPosition = currentPosition;

            mapPoint = Mouse.GetPosition(MapImage);

            XCordView.Text = $"X: {mapPoint.X:f2}";
            YCordView.Text = $"Y: {mapPoint.Y:f2}";
        }

        private void CenterMap_Click(object sender, RoutedEventArgs e)
        {
            GetMapMove().X = 0;
            GetMapMove().Y = 0;
            GetMapScale().ScaleX = 1;
            GetMapScale().ScaleY = 1;

            ZoomLevel = 1;
        }

        private void InitFirstPointInput(object sender, RoutedEventArgs e)
        {
            if (SecondPointWaiting)
                SecondPointWaiting = false;

            SetStatus("Use right mouse button to set the first point");
            FirstPointWaiting = true;
        }

        private void InitSecondPointInput(object sender, RoutedEventArgs e)
        {
            if (FirstPointWaiting)
                FirstPointWaiting = false;

            SetStatus("Use right mouse button to set the second point");
            SecondPointWaiting = true;
        }

        private void MapPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FirstPointWaiting)
            {
                FirstPoint = Mouse.GetPosition(MapImage);
                FirstPointWaiting = false;

                FirstPointXCordView.Text = $"X: {FirstPoint.X:f2}";
                FirstPointYCordView.Text = $"Y: {FirstPoint.Y:f2}";

                SetStatus($"Setted first point with coordinates: X:{FirstPoint.X:f2} Y:{FirstPoint.Y:f2}");

                firstPointMarker.SetLocation(FirstPoint.X, FirstPoint.Y);
                firstPointMarker.Show();
            }

            if (SecondPointWaiting)
            {
                SecondPoint = Mouse.GetPosition(MapImage);
                SecondPointWaiting = false;

                SecondPointXCordView.Text = $"X: {SecondPoint.X:f2}";
                SecondPointYCordView.Text = $"Y: {SecondPoint.Y:f2}";

                SetStatus($"Setted second point with coordinates: X:{SecondPoint.X:f2} Y:{SecondPoint.Y:f2}");

                secondPointMarker.SetLocation(SecondPoint.X, SecondPoint.Y);
                secondPointMarker.Show();
            }

            // Если обе точки были заданы, то можно уже рассчитывать расстояние
            if (FirstPoint.X != double.MaxValue && SecondPoint.X != double.MaxValue)
            {
                DistanceView.Text = $"Distance: {GeodCalculations.CalcLength(FirstPoint, SecondPoint):f2}m";
                distanceViewer.SetPoints(FirstPoint, SecondPoint);
                distanceViewer.Show();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetMapScale().CenterX = MapPanel.ActualWidth / 2;
            GetMapScale().CenterY = MapPanel.ActualHeight / 2;
        }

        private void SecondPointButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GetMapScale().CenterX = MapPanel.ActualWidth / 2;
            GetMapScale().CenterY = MapPanel.ActualHeight / 2;
        }
    }
}
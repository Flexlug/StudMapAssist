using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ImageSourceConverter imgSource = new ImageSourceConverter();
                MapViewer.SetValue(Image.SourceProperty, imgSource.ConvertFromString("map.jpg"));

                MapViewer.RenderTransform = mapControl;
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't load an image", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void MapViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point currentPos = e.GetPosition(MapPanel);

            ScaleTransform mapScale = GetMapScale();

            mapScale.CenterX = (currentPos.X + mapScale.CenterX) / 2;
            mapScale.CenterY = (currentPos.Y + mapScale.CenterY) / 2;

            if (e.Delta > 0)
            {
                mapScale.ScaleX *= 1.05;
                mapScale.ScaleY *= 1.05;
            }

            if (e.Delta < 0)
            {
                mapScale.ScaleX /= (mapScale.ScaleX > 1) ? 1.05 : 1;
                mapScale.ScaleY /= (mapScale.ScaleY > 1) ? 1.05 : 1;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
                MessageBox.Show($"Current scaleX is: {GetMapScale().ScaleX}\nCurrent scaleY is: {GetMapScale().ScaleY}");
        }

        private void MapViewer_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition = Mouse.GetPosition(MapPanel);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                TranslateTransform mapMove = GetMapMove();

                mapMove.X -= (lastPosition.X - currentPosition.X) / 1.5;
                mapMove.Y -= (lastPosition.Y - currentPosition.Y) / 1.5;
            }

            lastPosition = currentPosition;

            mapPoint = Mouse.GetPosition(MapViewer);

            XCordView.Text = $"X: {lastPosition.X}";
            YCordView.Text = $"Y: {lastPosition.Y}";
        }
    }
}

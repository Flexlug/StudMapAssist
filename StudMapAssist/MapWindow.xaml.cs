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

using StudMapAssist.ProgramMode.DistanceMeasure;
using StudMapAssist.ProgramMode.DirectGeodTask;
using StudMapAssist.ProgramMode;
using StudMapAssist.Graphics;

namespace StudMapAssist
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        private TransformGroup mapControl = new TransformGroup()
        {
            Children = { new TranslateTransform(), new ScaleTransform() }
        };        

        private TranslateTransform GetMapMove() => mapControl.Children[0] as TranslateTransform;
        private ScaleTransform GetMapScale() => mapControl.Children[1] as ScaleTransform;

        public ScreenCross screenCross;

        public IProgramMode currentMode;

        private Point lastPosition;
        private Point currentPosition;

        private Point mapPoint;

        private double ZoomLevel = 1;

        public void SetStatus(string msg) => ProgramStatus.Text = msg;

        public Point GetPositionOnMap() => Mouse.GetPosition(MapPanel);
        public Point GetPositionOnPanel() => Mouse.GetPosition(MainPanel);

        public Point LeftTopCorner;
        public Point LeftBottomCorner;
        public Point RightTopCorner;
        public Point RightBottomCorner;

        public MapWindow()
        {
            InitializeComponent();

            // Пробуем загрузить карту
            // Если загрузка не удаётся, то выдаём ошибку и закрываем прилоежние
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

            currentMode = new DirectGeodTaskControl(this);
            CurrentModeControl.Navigate(currentMode.GetPage());
        }

        private void MainPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point currentPos = e.GetPosition(MapImage);


            ScaleTransform mapScale = GetMapScale();
            double scaleDelta = 0;

            if (e.Delta > 0)
            {
                if (mapScale.ScaleX < 3.0)
                {
                    // Масштабирование замедляется при приближении

                    scaleDelta = 1.2 / (1 + (ZoomLevel / 10));
                    mapScale.ScaleX *= scaleDelta;
                    mapScale.ScaleY *= scaleDelta;

                    // Для сохранения собственных размеров маркеры удаляются при приближении изображения
                    //currentMode.UpdateScale(scaleDelta, true);

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
                if (mapScale.ScaleX > 0.4)
                {
                    // Масштабирование ускоряется при отдалении

                    scaleDelta = 1.2 / (1 + (ZoomLevel / 10));
                    mapScale.ScaleX /= scaleDelta;
                    mapScale.ScaleY /= scaleDelta;

                    // Для сохранения собственных размеров маркеры приближаются при отдалении изображения
                    //currentMode.UpdateScale(scaleDelta, false);

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
            MessageBox.Show($"{GetPositionOnPanel().X}  {GetPositionOnPanel().Y}");
        }

        private void MapViewer_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition = GetPositionOnPanel();
            mapPoint = GetPositionOnMap();

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                TranslateTransform mapMove = GetMapMove();

                // При сильном масштабировании движение намеренно замедляется
                mapMove.X -= (lastPosition.X - currentPosition.X) / (1.2 / (1 + ZoomLevel / 10));
                mapMove.Y -= (lastPosition.Y - currentPosition.Y) / (1.2 / (1 + ZoomLevel / 10));
            }

            if (screenCross.IsVisible)
            {
                screenCross.SetLocation(currentPosition.X, currentPosition.Y);
                screenCross.SetXCordValue(mapPoint.X);
                screenCross.SetYCordValue(mapPoint.Y);
            }

            lastPosition = currentPosition;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetMapScale().CenterX = MainPanel.ActualWidth / 2;
            GetMapScale().CenterY = MainPanel.ActualHeight / 2;

            screenCross = new ScreenCross(MainPanel, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            screenCross.Hide();
        }

        private void ChangeMode_Click(object sender, RoutedEventArgs e)
        {
            (new ChangeProgramMode(this)).ShowDialog();
        }
    }
}
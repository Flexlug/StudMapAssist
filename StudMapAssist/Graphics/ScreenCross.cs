using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudMapAssist.Graphics
{
    public class ScreenCross
    {
        TransformGroup controlCross = new TransformGroup()
        {
            Children =
            {
                new ScaleTransform(),
                new TranslateTransform()
            }
        };

        Panel mainPanel;

        Path crossFigure;

        public TranslateTransform GetCrossMove() => controlCross.Children[1] as TranslateTransform;
        public ScaleTransform GetCrossScale() => controlCross.Children[0] as ScaleTransform;

        public TextBlock xCordViewer;
        public TextBlock yCordViewer;

        private double screenWidth;
        private double screenHeight;        

        public bool IsVisible
        {
            get => (crossFigure.Opacity == 1);
            private set
            {

            }
        }

        public ScreenCross(Panel parentPanel, double screenWidth, double screenHeight)
        {
            // Инициализируем панель, куда поместим перекрестие и надписи с координатами
            mainPanel = new Canvas()
            {
                Width = screenWidth,
                Height = screenHeight
            };

            // Инициализируем TextBlock для отображения X координат
            xCordViewer = new TextBlock()
            {
                Background = Brushes.White,
                Foreground = Brushes.Black,
                Text = "sdasd"
            };

            // Инициализруем TextBlock для отображения Y координат
            yCordViewer = new TextBlock()
            {
                Background = Brushes.White,
                Foreground = Brushes.Black,
                Text = "sdasda"
            };

            crossFigure = new Path()
            {
                Stroke = Brushes.Red,
                StrokeDashArray = new DoubleCollection(new double[] { 4, 3.2 }),
                StrokeThickness = 2,
                Data = new GeometryGroup()
                {
                    Children =
                    {
                        // horisontal line
                        new LineGeometry(new Point(-screenWidth / 2, screenHeight / 2), new Point(screenWidth / 2 * 3, screenHeight / 2)),
                    
                        // vertical line
                        new LineGeometry(new Point(screenWidth / 2, -screenHeight / 2), new Point(screenWidth / 2, screenHeight / 2 * 3))
                    }
                },
                OpacityMask = Brushes.Black,
                Opacity = 1
            };

            mainPanel.Children.Add(crossFigure);
            mainPanel.Children.Add(xCordViewer);
            mainPanel.Children.Add(yCordViewer);

            Canvas.SetLeft(xCordViewer, 10 + screenWidth / 2);
            Canvas.SetTop(xCordViewer, 10 + screenHeight / 2);
            Canvas.SetLeft(yCordViewer, 10 + screenWidth / 2);
            Canvas.SetTop(yCordViewer, 30 + screenHeight / 2);

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            mainPanel.RenderTransform = controlCross;
            parentPanel.Children.Add(mainPanel);
        }

        public void SetLocation(double x, double y)
        {
            TranslateTransform crossMove = GetCrossMove();

            crossMove.X = x - screenWidth / 2;
            crossMove.Y = y - screenHeight / 2;
        }

        public void SetCordValue(Point cord)
        {
            xCordViewer.Text = $"x: {cord.X:f0}";
            yCordViewer.Text = $"y: {cord.Y:f0}";
        }

        public void Show()
        {
            mainPanel.Opacity = 1;
        }

        public void Hide()
        {
            mainPanel.Opacity = 0;
        }

        public Panel GetPanel()
        {
            return mainPanel;
        }
    }
}

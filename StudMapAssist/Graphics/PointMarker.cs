using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StudMapAssist.Graphics
{
    class PointMarker
    {
        TransformGroup controlMarker = new TransformGroup()
        {
            Children =
            {
                new ScaleTransform(),
                new TranslateTransform()
            }
        };

        Path markerFigure = new Path()
        {
            Fill = Brushes.Transparent,
            Stroke = Brushes.Blue,
            Data = new GeometryGroup()
            {
                Children =
                {
                    new EllipseGeometry(new Point(0, 0), 8, 8),
                    new LineGeometry(new Point(-12, 0), new Point(12, 0)),
                    new LineGeometry(new Point(0, 12), new Point(0, -12))
                }
            },
            OpacityMask = Brushes.Black,
            Opacity = 1
        };

        public bool IsVisible
        {
            get => markerFigure.Opacity == 1;
            private set { }
        }

        public TranslateTransform GetMarkerMove() => controlMarker.Children[1] as TranslateTransform;
        public ScaleTransform GetMarkerScale() => controlMarker.Children[0] as ScaleTransform;

        public void SetLocation(Point p)
        {
            TranslateTransform markerMove = GetMarkerMove();

            markerMove.X = p.X;
            markerMove.Y = p.Y;
        }

        public void ZoomIn(double scale)
        {
            ScaleTransform markerScale = GetMarkerScale();

            markerScale.ScaleX *= scale;
            markerScale.ScaleY *= scale;
        }

        public void ZoomOut(double scale)
        {
            ScaleTransform markerScale = GetMarkerScale();

            markerScale.ScaleX /= scale;
            markerScale.ScaleY /= scale;
        }

        public void SetFillColor(Brush color)
        {
            markerFigure.Fill = color;
        }

        public void SetStrokeColor(Brush color)
        {
            markerFigure.Stroke = color;
        }

        public void Show()
        {
            markerFigure.Opacity = 1;
        }

        public void Hide()
        {
            markerFigure.Opacity = 0;
        }

        public Path GetPath()
        {
            return markerFigure;
        }

        public PointMarker(Panel newParent, double x, double y)
        {
            markerFigure.RenderTransform = controlMarker;
            newParent.Children.Add(markerFigure);
            GetMarkerMove().X = x;
            GetMarkerMove().Y = y;
        }
    }
}


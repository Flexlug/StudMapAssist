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
    public class DistanceLine
    {
        TransformGroup controlLine = new TransformGroup()
        {
            Children =
            {
                new ScaleTransform(),
                new TranslateTransform()
            }
        };

        public TranslateTransform GetMarkerMove() => controlLine.Children[1] as TranslateTransform;
        public ScaleTransform GetMarkerScale() => controlLine.Children[0] as ScaleTransform;

        Path mainLine;

        public DistanceLine(Panel parentPanel, SolidColorBrush firstColor, SolidColorBrush secondColor)
        {
            mainLine = new Path()
            {
                Stroke = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection()
                },

                StrokeThickness = 3,

                Data = new GeometryGroup()
                {
                    Children =
                    {
                        new LineGeometry(new Point(0, 0), new Point(1, 1))
                    }
                },

                OpacityMask = Brushes.Black,
                Opacity = 0
            };

            GradientStopCollection linear = new GradientStopCollection();
            for (double i = 0; i < 1; i += 0.1)
            {
                linear.Add(new GradientStop(firstColor.Color, i));
                linear.Add(new GradientStop(secondColor.Color, i + 0.05));
            }
            (mainLine.Stroke as LinearGradientBrush).GradientStops = linear;

            parentPanel.Children.Add(mainLine);
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

        public void Show()
        {
            mainLine.Opacity = 1;
        }

        public void Hide()
        {
            mainLine.Opacity = 0;
        }

        public Path GetPath()
        {
            return mainLine;
        }

        public void SetPoints(Point fstPoint, Point sndPoint)
        {
            (mainLine.Data as GeometryGroup).Children.Clear();
            (mainLine.Data as GeometryGroup).Children.Add(new LineGeometry(fstPoint, sndPoint));
        }
    }
}
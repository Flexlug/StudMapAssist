using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StudMapAssist
{
    public class DistanceViewer
    {
        Path mainLine;

        public DistanceViewer(Panel parentPanel, SolidColorBrush firstColor, SolidColorBrush secondColor)
        {
            mainLine = new Path()
            {
                Stroke = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection()
                    {
                        new GradientStop(firstColor.Color, 1),
                        new GradientStop(secondColor.Color, 0)
                    }
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

            parentPanel.Children.Add(mainLine);
        }

        public void Show()
        {
            mainLine.Opacity = 1;
        }

        public void Hide()
        {
            mainLine.Opacity = 0;
        }

        public void SetPoints(Point fstPoint, Point sndPoint)
        {
            (mainLine.Data as GeometryGroup).Children.Clear();
            (mainLine.Data as GeometryGroup).Children.Add(new LineGeometry(fstPoint, sndPoint));
        }
    }
}

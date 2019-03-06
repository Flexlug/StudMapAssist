using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudMapAssist
{
    public static class GeodCalculations
    {
        // Константа, которая позволяет переводить пиксели в метры
        public static double PixelScale = 11;
        
        public static double CalcLength(Point fstPoint, Point sndPoint)
        {
            // Вычисляем разницу в координатах между точками
            double distanceX = fstPoint.X - sndPoint.X,
                   distanceY = fstPoint.Y - sndPoint.Y;

            // Вычисляем расстояние между точками (в пикселях)
            double distanceInPixels = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            // Переводим пиксели в метры, умножая полученное значение на MapScale;
            return distanceInPixels * PixelScale;
        }
    }
}

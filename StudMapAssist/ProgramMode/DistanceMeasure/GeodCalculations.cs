using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudMapAssist.ProgramMode.DistanceMeasure
{
    public static class GeodCalculations
    {
        // Константа, которая позволяет переводить пиксели в метры
        public static double PixelScale = 1.2698412698412698412698412698413;

        /// <summary>
        /// Вычисляет расстояние между двумя точками. Точки должны быть в пиксельных координатах
        /// </summary>
        /// <param name="fstPoint">Первая точка [PX]</param>
        /// <param name="sndPoint">Вторая точка [PX]</param>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает дерикционный угол между данными точками
        /// </summary>
        /// <param name="yDiff">Разница в координатах по оси Y</param>
        /// <param name="XDiff">Разница в координатах по оси X</param>
        /// <returns></returns>
        public static double GetDlAngle(double yDiff, double xDiff)
        {
            if (yDiff > 0 && xDiff > 0)
            {
                return Math.Atan2(yDiff, xDiff);
            }
            else
            {
                if (yDiff > 0 && xDiff < 0)
                {
                    return Math.Atan2(Math.Abs(xDiff), yDiff) + (new DirectGeodTask.Angle(false, 90, 0)).ToRadian();
                }
                else
                {
                    if (yDiff < 0 && xDiff < 0)
                    {
                        return Math.Atan2(Math.Abs(yDiff), Math.Abs(xDiff)) + new DirectGeodTask.Angle(false, 180, 0).ToRadian();
                    }
                    else
                    {
                        if (yDiff < 0 && xDiff > 0)
                        {
                            return Math.Atan2(xDiff, Math.Abs(yDiff)) + new DirectGeodTask.Angle(false, 270, 0).ToRadian();
                        }
                    }
                }
            }

            throw new ArithmeticException($"Ошибка во время рассчёта дирекционного угла yDiff:{yDiff}, xDiff:{xDiff}");
        }
    }
}

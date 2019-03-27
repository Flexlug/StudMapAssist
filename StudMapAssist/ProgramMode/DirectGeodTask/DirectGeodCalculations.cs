using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using StudMapAssist;

namespace StudMapAssist.ProgramMode.DirectGeodTask
{
    public static class DirectGeodCalculations
    {
        public static readonly Point METERS_LT = new Point(6068829.52, 4310680.54);
        public static readonly Point METERS_LB = new Point(6064195.01, 4310487.19);
        public static readonly Point METERS_RT = new Point(6068663.44, 4314705.98);
        public static readonly Point METERS_RB = new Point(6064029.68, 4314520.12);

        public static readonly Point PIXELS_LT = new Point( 320.26,  257.19);
        public static readonly Point PIXELS_LB = new Point(319.155, 3903.29);
        public static readonly Point PIXELS_RT = new Point(   3490,  256.98);
        public static readonly Point PIXELS_RB = new Point(   3490, 3903.29);

        public static readonly double METERS_MAP_WIDTH = METERS_RT.Y - METERS_LT.Y;
        public static readonly double METERS_MAP_HEIGHT = METERS_LB.X - METERS_LT.X;
        public static readonly double PIXEL_MAP_WIDTH = PIXELS_RT.X - PIXELS_LT.X;
        public static readonly double PIXEL_MAP_HEIGHT = PIXELS_LB.Y - PIXELS_LT.Y;

        /// <summary>
        /// Переводит координаты в пикселях в прямоугольную систему координат карты
        /// </summary>
        /// <param name="pxCords">Точка, в которой записаны координаты в пикселях</param>
        /// <returns></returns>
        public static Point PixelsToSquareCords(Point pxCords)
        {
            // В КАРТОГРАФИИ X И Y ОСИ ИНВЕРТИРОВАНЫ!!!
            double xMeters, yMeters, pxCord;

            // Насколько сильно курсор ушёл влево (от 0 до 1)
            double xRatio = (pxCords.X - PIXELS_LT.X) / PIXEL_MAP_WIDTH;

            // Насколько сильно курсор ушёл вниз (от 0 до 1)
            double yRatio = (pxCords.Y - PIXELS_LT.Y) / PIXEL_MAP_HEIGHT;

            // Вынужденная поправка из-за кривизны скана карты
            double DUMMY_CORRECTION_Y = 5;

            // Ось X            
            pxCord = pxCords.X - PIXELS_LT.X;
            yMeters = pxCord * MapWindow.MAP_SCALE;

            // Поправка по кривизне карты
            double xDifference = METERS_LT.Y - METERS_LB.Y;
            yMeters += METERS_LT.Y;
            yMeters -= xDifference * yRatio;
            

            // Ось Y
            pxCord = pxCords.Y - PIXELS_LT.Y;
            xMeters = pxCord * MapWindow.MAP_SCALE;

            // Поправка по кривизне карты
            double yDifference = METERS_RT.X - METERS_LT.X;
            xMeters = METERS_LT.X - xMeters;
            xMeters += yDifference * xRatio;
            xMeters -= DUMMY_CORRECTION_Y * yRatio;

            return new Point(xMeters, yMeters);
        }

        ///// <summary>
        ///// Переводит координаты в прямоугольной системе координат в координаты в пикселях
        ///// </summary>
        ///// <param name="mCords">Точка, в которой записаны координаты в прямоугольной системе координат</param>
        ///// <returns></returns>
        //public static Point SquareCordsToPixels(Point mCords)
        //{


        //    return new Point(double.MaxValue, double.MaxValue);
        //}
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudMapAssist.ProgramMode.DirectGeodTask
{
    public struct Angle
    {
        public double Degrees;
        public double Minutes;
        //public double Seconds;

        public bool isNegative;

        public Angle(double angleInRadian)
        {
            isNegative = angleInRadian < 0;
            double decDegrees = angleInRadian * 180 / Math.PI;

            Degrees = Math.Truncate(decDegrees);
            Minutes = (decDegrees - Degrees) * 60;
            if (Minutes == 60)
            {
                Minutes = 0;
                Degrees++;
            }
        }

        public Angle(bool isNegative, double deg, double mins)
        {
            this.isNegative = isNegative;
            Degrees = deg;
            Minutes = mins;
        }

        public double ToRadian()
        {
            return ((Degrees + Minutes / 60) * Math.PI / 180) * (isNegative ? -1 : 1);
        }

        public static Angle operator /(Angle ang1, double val)
        {
            double secondsResult = AngleToSeconds(ang1) / val;
            return SecondsToAngle(secondsResult);
        }

        public static Angle operator *(Angle ang1, double val)
        {
            double secondsResult = AngleToSeconds(ang1) * val;
            return SecondsToAngle(secondsResult);
        }

        public static Angle operator +(Angle ang1, Angle ang2)
        {
            double secondsResult = AngleToSeconds(ang1) + AngleToSeconds(ang2);
            return SecondsToAngle(secondsResult);
        }

        public static Angle operator -(Angle ang1, Angle ang2)
        {
            double secondsResult = AngleToSeconds(ang1) - AngleToSeconds(ang2);
            return SecondsToAngle(secondsResult);
        }

        public static bool operator <(Angle ang1, Angle ang2)
        {
            return (ang1 - ang2).ToRadian() < 0;
        }

        public static bool operator >(Angle ang1, Angle ang2)
        {
            return (ang1 - ang2).ToRadian() > 0;
        }

        public static double AngleToSeconds(Angle ang)
        {
            return (ang.Degrees * 3600 + ang.Minutes * 60) * (ang.isNegative ? -1 : 1);
        }

        public static Angle SecondsToAngle(double seconds)
        {
            double secondsResult = seconds;
            double minutesResult = 0,
                   degreesResult = 0;

            bool isNegativeResult = false;

            if (secondsResult < 0)
            {
                secondsResult *= -1;
                isNegativeResult = true;
            }

            while (secondsResult >= 60)
            {
                if (secondsResult >= 3600)
                {
                    degreesResult++;
                    secondsResult -= 3600;
                    continue;
                }
                if (secondsResult >= 60)
                {
                    minutesResult++;
                    secondsResult -= 60;
                    continue;
                }
            }

            minutesResult += secondsResult / 60;

            return new Angle(isNegativeResult, degreesResult, minutesResult);
        }

        // Решение от Даценд
        // cyberforum
        // http://www.cyberforum.ru/csharp-beginners/thread1543203.html
        static double PrintWithPrecision(double x, int precision)
        {
            return ((int)(x * Math.Pow(10.0, precision)) / Math.Pow(10.0, precision));
        }

        public override string ToString()
        {
            return $"{(isNegative ? "-" : "")}{Degrees:f0}°{PrintWithPrecision(Minutes, 0)}\'";
        }
    }
}
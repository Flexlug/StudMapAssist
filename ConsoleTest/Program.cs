using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StudMapAssist.ProgramMode.DirectGeodTask;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AngleTest();

            Console.ReadKey();
        }

        static void AngleTest()
        {
            Angle angle1 = new Angle(false, 50, 0);
            Angle angle2 = new Angle(true, 50, 0);
            Angle angle3 = new Angle(false, 25, 0);

            Console.WriteLine(Math.Atan2(400, -200));
            Console.WriteLine(angle3 < angle1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using static System.Convert;

using StudMapAssist.ProgramMode.DistanceMeasure;

namespace StudMapAssist.ProgramMode.DirectGeodTask
{
    /// <summary>
    /// Логика взаимодействия для MeasuredDirectGeodInput.xaml
    /// </summary>
    public partial class MeasuredDirectGeodInput : Window
    {
        public Angle DlAngle1;
        public Angle DlAngle2;
        public Angle DlAngle3;

        public double Dist1;
        public double Dist2;
        public double Dist3;

        public double DeltaX1;
        public double DeltaX2;
        public double DeltaX3;

        public double DeltaY1;
        public double DeltaY2;
        public double DeltaY3;

        public double CalcedX1;
        public double CalcedX2;
        public double CalcedX3;

        public double CalcedY1;
        public double CalcedY2;
        public double CalcedY3;

        public double MeasuredX1;
        public double MeasuredX2;
        public double MeasuredX3;

        public double MeasuredY1;
        public double MeasuredY2;
        public double MeasuredY3;

        public DirectGeodDifference geodDiff = null;

        MapWindow mainWindow;
        DirectGeodTaskControl geodControl;       

        public MeasuredDirectGeodInput(MapWindow mw, DirectGeodTaskControl gc)
        {
            InitializeComponent();

            mainWindow = mw;
            geodControl = gc;
        }

        private void CancelInput_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void SubmitInput_Click(object sender, RoutedEventArgs e)
        {
            // Пробуем получить значения, которые ввёл пользователь
            try
            {
                geodDiff = new DirectGeodDifference();

                DlAngle1 = new Angle(TBDlAngle1Deg.Text.Contains('-'), ToDouble(TBDlAngle1Deg.Text), ToDouble(TBDlAngle1Min.Text));
                DlAngle2 = new Angle(TBDlAngle2Deg.Text.Contains('-'), ToDouble(TBDlAngle2Deg.Text), ToDouble(TBDlAngle2Min.Text));
                DlAngle3 = new Angle(TBDlAngle3Deg.Text.Contains('-'), ToDouble(TBDlAngle3Deg.Text), ToDouble(TBDlAngle3Min.Text));
                Dist1 = ToDouble(TBDist1.Text);
                Dist2 = ToDouble(TBDist2.Text);
                Dist3 = ToDouble(TBDist3.Text);
                DeltaX1 = ToDouble(TBDeltaX1.Text);
                DeltaX2 = ToDouble(TBDeltaX2.Text);
                DeltaX3 = ToDouble(TBDeltaX3.Text);
                DeltaY1 = ToDouble(TBDeltaY1.Text);
                DeltaY2 = ToDouble(TBDeltaY2.Text);
                DeltaY3 = ToDouble(TBDeltaY3.Text);
                CalcedX1 = ToDouble(TBCalcedX1.Text);
                CalcedX2 = ToDouble(TBCalcedX2.Text);
                CalcedX3 = ToDouble(TBCalcedX3.Text);
                CalcedY1 = ToDouble(TBCalcedY1.Text);
                CalcedY2 = ToDouble(TBCalcedY2.Text);
                CalcedY3 = ToDouble(TBCalcedY3.Text);
                MeasuredX1 = ToDouble(TBMeasuredX1.Text);
                MeasuredX2 = ToDouble(TBMeasuredX2.Text);
                MeasuredX3 = ToDouble(TBMeasuredX3.Text);
                MeasuredY1 = ToDouble(TBMeasuredY1.Text);
                MeasuredY2 = ToDouble(TBMeasuredY2.Text);
                MeasuredY3 = ToDouble(TBMeasuredY3.Text);

                Point mapPointA = DirectGeodCalculations.PixelsToSquareCords(geodControl.APoint);
                Point mapPointB = DirectGeodCalculations.PixelsToSquareCords(geodControl.BPoint);
                Point mapPointC = DirectGeodCalculations.PixelsToSquareCords(geodControl.CPoint);

                geodDiff.MeasuredX1.Text = string.Empty;
                geodDiff.MeasuredX1.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = MeasuredX1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapPointA.X:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Math.Abs(mapPointA.X - MeasuredX1) < 50 ? "В пределах нормы" : "Ошибка!")}",
                        Foreground = (Math.Abs(mapPointA.X - MeasuredX1) < 50 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.MeasuredX2.Text = string.Empty;
                geodDiff.MeasuredX2.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = MeasuredX2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapPointB.X:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Math.Abs(mapPointB.X - MeasuredX2) < 50 ? "В пределах нормы" : "Ошибка!")}",
                        Foreground = (Math.Abs(mapPointB.X - MeasuredX2) < 50 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.MeasuredX3.Text = string.Empty;
                geodDiff.MeasuredX3.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = MeasuredX3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapPointC.X:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Math.Abs(mapPointC.X - MeasuredX3) < 50 ? "В пределах нормы" : "Ошибка!")}",
                        Foreground = (Math.Abs(mapPointC.X - MeasuredX3) < 50 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.MeasuredY1.Text = string.Empty;
                geodDiff.MeasuredY1.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = MeasuredY1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapPointA.Y:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Math.Abs(mapPointA.Y - MeasuredY1) < 50 ? "В пределах нормы" : "Ошибка!")}",
                        Foreground = (Math.Abs(mapPointA.Y - MeasuredY1) < 50 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.MeasuredY2.Text = string.Empty;
                geodDiff.MeasuredY2.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = MeasuredY2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapPointB.Y:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Math.Abs(mapPointB.Y - MeasuredY2) < 50 ? "В пределах нормы" : "Ошибка!")}",
                        Foreground = (Math.Abs(mapPointB.Y - MeasuredY2) < 50 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.MeasuredY3.Text = string.Empty;
                geodDiff.MeasuredY3.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = MeasuredY3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapPointC.Y:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Math.Abs(mapPointC.Y - MeasuredY3) < 50 ? "В пределах нормы" : "Ошибка!")}",
                        Foreground = (Math.Abs(mapPointC.Y - MeasuredY3) < 50 ? Brushes.Green : Brushes.Red)
                    }
                });

                Angle mapAngleAB, mapAngleBC, mapAngleCA;

                // oX - координатная сетка
                // (oXA A B) angle
                mapAngleAB = new Angle(GeodCalculations.GetDlAngle(mapPointB.Y - mapPointA.Y, mapPointB.X - mapPointA.X));
                geodDiff.Angle1.Text = string.Empty;
                geodDiff.Angle1.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DlAngle1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = mapAngleAB.ToString(),
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{DlAngle1 - mapAngleAB}",
                        Foreground = ((DlAngle1 - mapAngleAB).ToRadian() > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                mapAngleBC = new Angle(GeodCalculations.GetDlAngle(mapPointC.Y - mapPointB.Y, mapPointC.X - mapPointB.X));
                geodDiff.Angle2.Text = string.Empty;
                geodDiff.Angle2.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DlAngle2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = mapAngleBC.ToString(),
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{DlAngle2 - mapAngleBC}",
                        Foreground = ((DlAngle2 - mapAngleBC).ToRadian() > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                mapAngleCA = new Angle(GeodCalculations.GetDlAngle(mapPointA.Y - mapPointC.Y, mapPointA.X - mapPointC.X));
                geodDiff.Angle3.Text = string.Empty;
                geodDiff.Angle3.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DlAngle3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = mapAngleCA.ToString(),
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{DlAngle3 - mapAngleCA}",
                        Foreground = ((DlAngle3 - mapAngleCA).ToRadian() > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                double mapDistAB = GeodCalculations.CalcLength(geodControl.APoint, geodControl.BPoint),
                       mapDistBC = GeodCalculations.CalcLength(geodControl.BPoint, geodControl.CPoint),
                       mapDistCA = GeodCalculations.CalcLength(geodControl.CPoint, geodControl.APoint);


                geodDiff.Dist1.Text = string.Empty;
                geodDiff.Dist1.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = Dist1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDistAB:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Dist1 - mapDistAB):f0}",
                        Foreground = (Dist1 - mapDistAB > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.Dist2.Text = string.Empty;
                geodDiff.Dist2.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = Dist2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDistBC:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Dist2 - mapDistBC):f0}",
                        Foreground = (Dist2 - mapDistBC) > 0 ? Brushes.Green : Brushes.Red
                    }
                });

                geodDiff.Dist3.Text = string.Empty;
                geodDiff.Dist3.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = Dist3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDistCA:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(Dist3 - mapDistCA):f0}",
                        Foreground = (Dist3 - mapDistCA > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                double mapDeltaX1 = Dist1 * Math.Cos(mapAngleAB.ToRadian()),
                       mapDeltaX2 = Dist2 * Math.Cos(mapAngleBC.ToRadian()),
                       mapDeltaX3 = Dist3 * Math.Cos(mapAngleCA.ToRadian()),
                       mapDeltaY1 = Dist1 * Math.Sin(mapAngleAB.ToRadian()),
                       mapDeltaY2 = Dist2 * Math.Sin(mapAngleBC.ToRadian()),
                       mapDeltaY3 = Dist3 * Math.Sin(mapAngleCA.ToRadian());

                geodDiff.DeltaX1.Text = string.Empty;
                geodDiff.DeltaX1.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DeltaX1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDeltaX1:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(DeltaX1 - mapDeltaX1):f0}",
                        Foreground = (DeltaX1 - mapDeltaX1 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.DeltaX2.Text = string.Empty;
                geodDiff.DeltaX2.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DeltaX2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDeltaX2:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(DeltaX2 - mapDeltaX2):f0}",
                        Foreground = ((DeltaX2 - mapDeltaX2) > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.DeltaX3.Text = string.Empty;
                geodDiff.DeltaX3.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DeltaX3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDeltaX3:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(DeltaX3 - mapDeltaX3):f0}",
                        Foreground = ((DeltaX3 - mapDeltaX3) > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.DeltaY1.Text = string.Empty;
                geodDiff.DeltaY1.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DeltaY1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDeltaY1:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(DeltaY1 - mapDeltaY1):f0}",
                        Foreground = (DeltaY1 - mapDeltaY1 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.DeltaY2.Text = string.Empty;
                geodDiff.DeltaY2.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DeltaY2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDeltaY2:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(DeltaY2 - mapDeltaY2):f0}",
                        Foreground = (DeltaY2 - mapDeltaY2 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.DeltaY3.Text = string.Empty;
                geodDiff.DeltaY3.Inlines.AddRange(new List<Inline>() {
                    new Run()
                    {
                        Text = DeltaY3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapDeltaY3:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(DeltaY3 - mapDeltaY3):f0}",
                        Foreground = (DeltaY3 - mapDeltaY3 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                double mapCalcedX2 = mapDeltaX1 + mapPointA.X, 
                       mapCalcedY2 = mapDeltaY1 + mapPointA.Y,
                       mapCalcedX3 = mapDeltaX2 + mapCalcedX2, 
                       mapCalcedY3 = mapDeltaY2 + mapCalcedY2,
                       mapCalcedX1 = mapDeltaX3 + mapCalcedX3, 
                       mapCalcedY1 = mapDeltaY3 + mapCalcedY3;

                geodDiff.CalcedX1.Text = string.Empty;
                geodDiff.CalcedX1.Inlines.AddRange(new List<Inline>()
                {
                    new Run()
                    {
                        Text = CalcedX1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapCalcedX1:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(mapCalcedX1 - CalcedX1):f0}",
                        Foreground = (mapCalcedX1 - CalcedX1 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.CalcedY1.Text = string.Empty;
                geodDiff.CalcedY1.Inlines.AddRange(new List<Inline>()
                {
                    new Run()
                    {
                        Text = CalcedY1.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapCalcedY1:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(mapCalcedY1 - CalcedY1):f0}",
                        Foreground = (mapCalcedY1 - CalcedY1 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.CalcedX2.Text = string.Empty;
                geodDiff.CalcedX2.Inlines.AddRange(new List<Inline>()
                {
                    new Run()
                    {
                        Text = CalcedX2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapCalcedX2:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(mapCalcedX2 - CalcedX2):f0}",
                        Foreground = (mapCalcedX2 - CalcedX2 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.CalcedY2.Text = string.Empty;
                geodDiff.CalcedY2.Inlines.AddRange(new List<Inline>()
                {
                    new Run()
                    {
                        Text = CalcedY2.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapCalcedY2:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(mapCalcedY2 - CalcedY2):f0}",
                        Foreground = (mapCalcedY2 - CalcedY2 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.CalcedX3.Text = string.Empty;
                geodDiff.CalcedX3.Inlines.AddRange(new List<Inline>()
                {
                    new Run()
                    {
                        Text = CalcedX3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapCalcedX3:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(mapCalcedX3 - CalcedX3):f0}",
                        Foreground = (mapCalcedX3 - CalcedX3 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.CalcedY3.Text = string.Empty;
                geodDiff.CalcedY3.Inlines.AddRange(new List<Inline>()
                {
                    new Run()
                    {
                        Text = CalcedY3.ToString(),
                        Foreground = Brushes.Gray
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{mapCalcedY3:f0}",
                        Foreground = Brushes.Green
                    },
                    new LineBreak(),
                    new Run()
                    {
                        Text = $"{(mapCalcedY3 - CalcedY3):f0}",
                        Foreground = (mapCalcedY3 - CalcedY3 > 0 ? Brushes.Green : Brushes.Red)
                    }
                });

                geodDiff.DiffX1.Text = $"{(int)(MeasuredX1 - mapCalcedX1)}";
                geodDiff.DiffX2.Text = $"{(int)(MeasuredX2 - mapCalcedX2)}";
                geodDiff.DiffX3.Text = $"{(int)(MeasuredX3 - mapCalcedX3)}";
                geodDiff.DiffY1.Text = $"{(int)(MeasuredY1 - mapCalcedY1)}";
                geodDiff.DiffY2.Text = $"{(int)(MeasuredY2 - mapCalcedY2)}";
                geodDiff.DiffY3.Text = $"{(int)(MeasuredY3 - mapCalcedY3)}";

                geodDiff.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при конвертации ввода пользователя", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public static Point Add(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }
}
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using StudMapAssist.Graphics;

namespace StudMapAssist.ProgramMode.DirectGeodTask
{
    /// <summary>
    /// Логика взаимодействия для DirectGeodTaskControl.xaml
    /// </summary>
    public partial class DirectGeodTaskControl : Page, IDisposable, IProgramMode
    {
        /// <summary>
        /// Ссылка на главное окно с отображаемой картой
        /// </summary>
        MapWindow mapWindow;



        #region Второй треугольник
        // Треугольник, который построен по координатам, которые ввёл студент
        // Все переменные аналогичны, только имеют приписку Stud

        //private Point studAPoint = new Point(double.MaxValue, double.MaxValue);
        //private Point studBPoint = new Point(double.MaxValue, double.MaxValue);
        //private Point studCPoint = new Point(double.MaxValue, double.MaxValue);

        //private PointMarker studAPointMarker;
        //private PointMarker studBPointMarker;
        //private PointMarker studCPointMarker;

        //private DistanceLine studABDistanceLine;
        //private DistanceLine studBCDistanceLine;
        //private DistanceLine studCADistanceLine;

        //private SolidColorBrush STUD_A_POINT_MARKER_BRUSH = Brushes.PaleVioletRed;
        //private SolidColorBrush STUD_B_POINT_MARKER_BRUSH = Brushes.LightGreen;
        //private SolidColorBrush STUD_C_POINT_MARKER_BRUSH = Brushes.BlueViolet;

        #endregion



        /// <summary>
        /// Флаг. Если true, значит программа сейчас ожидает ввода новой точки
        /// </summary>
        private bool APointWaiting = false;

        /// <summary>
        /// Флаг. Если true, значит программа сейчас ожидает ввода новой точки
        /// </summary>
        private bool BPointWaiting = false;

        /// <summary>
        /// Флаг. Если true, значит программа сейчас ожидает ввода новой точки
        /// </summary>
        private bool CPointWaiting = false;



        /// <summary>
        /// Точка А, выставленная на карте
        /// </summary>
        public Point APoint = new Point(double.MaxValue, double.MaxValue);

        /// <summary>
        /// Маркер точки А
        /// </summary>
        private PointMarker APointMarker;

        /// <summary>
        /// Цвет маркера точки А
        /// </summary>
        private SolidColorBrush A_POINT_MARKER_BRUSH = Brushes.Red;


        /// <summary>
        /// Точка B, выставленная на карте
        /// </summary>
        public Point BPoint = new Point(double.MaxValue, double.MaxValue);

        /// <summary>
        /// Маркер точки B
        /// </summary>
        private PointMarker BPointMarker;

        /// <summary>
        /// Цвет маркера точки B
        /// </summary>
        private SolidColorBrush B_POINT_MARKER_BRUSH = Brushes.Green;


        /// <summary>
        /// Точка C, выставленная на карте
        /// </summary>
        public Point CPoint = new Point(double.MaxValue, double.MaxValue);

        /// <summary>
        /// Маркер точки С
        /// </summary>
        private PointMarker CPointMarker;

        /// <summary>
        /// Цвет маркера точки C
        /// </summary>
        private SolidColorBrush C_POINT_COLOR_BRUSH = Brushes.Blue;



        /// <summary>
        /// Линия, отображающая расстояние между точками A и B
        /// </summary>
        private DistanceLine ABDistanceLine;

        /// <summary>
        /// Линия, отображающая расстояние между точками B и C
        /// </summary>
        private DistanceLine BCDistanceLine;

        /// <summary>
        /// Линия, отображающая расстояние между точками C и A
        /// </summary>
        private DistanceLine CADistanceLine;



        /// <summary>
        /// Окно, в которое студент должен будет ввести все высчитанные значения
        /// </summary>
        MeasuredDirectGeodInput studentCalculations = null;



        /// <summary>
        /// Делегат, который передаётся главному окну
        /// </summary>
        MouseButtonEventHandler mouseLMBHandler;

        /// <summary>
        /// Делегат, который передаётся главному окну
        /// </summary>
        MouseButtonEventHandler mouseRMBHandler;



        /// <summary>
        /// Контестное меню, которое вызывается по нажатию правой кнопки мыши
        /// </summary>
        private ContextMenu rmbMenu;


        public DirectGeodTaskControl(MapWindow w)
        {
            InitializeComponent();

            mapWindow = w;

            // Создаём делегаты
            mouseLMBHandler = new MouseButtonEventHandler(MouseLMBDown);
            mouseRMBHandler = new MouseButtonEventHandler(MouseRMBDown);

            // Подписываем их на соответствующие события в главном окне
            mapWindow.MainPanel.MouseLeftButtonDown += mouseLMBHandler;
            mapWindow.MainPanel.MouseRightButtonDown += mouseRMBHandler;

            // Инициализируем PointMarker-ы, добавляем их на главную панель
            APointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            APointMarker.SetStrokeColor(A_POINT_MARKER_BRUSH);
            APointMarker.Hide();

            BPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            BPointMarker.SetStrokeColor(B_POINT_MARKER_BRUSH);
            BPointMarker.Hide();

            CPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            CPointMarker.SetStrokeColor(C_POINT_COLOR_BRUSH);
            CPointMarker.Hide();

            //studAPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            //studAPointMarker.SetStrokeColor(STUD_A_POINT_MARKER_BRUSH);
            //studAPointMarker.Hide();

            //studBPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            //studBPointMarker.SetStrokeColor(STUD_B_POINT_MARKER_BRUSH);
            //studBPointMarker.Hide();

            //studCPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            //studCPointMarker.SetStrokeColor(STUD_C_POINT_MARKER_BRUSH);
            //studCPointMarker.Hide();

            // Выставим соответствующий цвет шрифтов, чтобы маркеры на карте можно было отличить друг от друга
            APointHint.Foreground = A_POINT_MARKER_BRUSH;
            BPointHint.Foreground = B_POINT_MARKER_BRUSH;
            CPointHint.Foreground = C_POINT_COLOR_BRUSH;

            // Инициализируем DistanceLine-ы, добавим их на панель
            ABDistanceLine = new DistanceLine(mapWindow.MapPanel, A_POINT_MARKER_BRUSH, B_POINT_MARKER_BRUSH);
            ABDistanceLine.Hide();
            BCDistanceLine = new DistanceLine(mapWindow.MapPanel, B_POINT_MARKER_BRUSH, C_POINT_COLOR_BRUSH);
            BCDistanceLine.Hide();
            CADistanceLine = new DistanceLine(mapWindow.MapPanel, C_POINT_COLOR_BRUSH, A_POINT_MARKER_BRUSH);
            CADistanceLine.Hide();

            //studABDistanceLine = new DistanceLine(mapWindow.MapPanel, STUD_A_POINT_MARKER_BRUSH, STUD_B_POINT_MARKER_BRUSH);
            //studABDistanceLine.Hide();
            //studBCDistanceLine = new DistanceLine(mapWindow.MapPanel, STUD_B_POINT_MARKER_BRUSH, STUD_C_POINT_MARKER_BRUSH);
            //studBCDistanceLine.Hide();
            //studCADistanceLine = new DistanceLine(mapWindow.MapPanel, STUD_C_POINT_MARKER_BRUSH, STUD_A_POINT_MARKER_BRUSH);
            //studCADistanceLine.Hide();

            rmbMenu = this.FindResource("rmbMenu") as ContextMenu;
            rmbMenu.PlacementTarget = mapWindow.MainPanel;
        }

        /// <summary>
        /// Производит отписку от событий главной формы и удаляет элемены с MapPanel
        /// </summary>
        public void Dispose()
        {
            mapWindow.MainPanel.MouseLeftButtonDown -= mouseLMBHandler;
            mapWindow.MainPanel.MouseRightButtonDown -= mouseRMBHandler;

            mapWindow.MapPanel.Children.Remove(APointMarker.GetPath());
            mapWindow.MapPanel.Children.Remove(BPointMarker.GetPath());
            mapWindow.MapPanel.Children.Remove(CPointMarker.GetPath());
            mapWindow.MapPanel.Children.Remove(ABDistanceLine.GetPath());
            mapWindow.MapPanel.Children.Remove(BCDistanceLine.GetPath());
            mapWindow.MapPanel.Children.Remove(CADistanceLine.GetPath());
            //mapWindow.MapPanel.Children.Remove(studAPointMarker.GetPath());
            //mapWindow.MapPanel.Children.Remove(studBPointMarker.GetPath());
            //mapWindow.MapPanel.Children.Remove(studCPointMarker.GetPath());
            //mapWindow.MapPanel.Children.Remove(studABDistanceLine.GetPath());
            //mapWindow.MapPanel.Children.Remove(studBCDistanceLine.GetPath());
            //mapWindow.MapPanel.Children.Remove(studCADistanceLine.GetPath());

            if (studentCalculations != null)
            {
                if (studentCalculations.geodDiff != null)
                    studentCalculations.geodDiff.Close();

                studentCalculations.Close();
            }
            mapWindow.CurrentModeControl.Resources.Remove(this);
        }

        /// <summary>
        /// Возвращает ссылку на данную страницу (используется для метода Navigate в главном окне)
        /// </summary>
        /// <returns></returns>
        public Page GetPage() => this;

        /// <summary>
        /// То, что будет выполняться по нажатию левой кнопки мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseLMBDown(object sender, MouseButtonEventArgs e)
        {
            if (APointWaiting)
            {
                // Добавление новой точки на карте. Получение её координат в пикселях и в метрах
                APoint = mapWindow.GetPositionOnMap();
                APointWaiting = false;

                // Разместим маркер по полученным координатам, 
                APointMarker.SetLocation(APoint);
                APointMarker.Show();

                // Отобразим координаты точки A
                Point printingPoint = DirectGeodCalculations.PixelsToSquareCords(APoint);
                PointAXCord.Text = $"X: {printingPoint.X:f0}";
                PointAYCord.Text = $"Y: {printingPoint.Y:f0}";

                // Уберём перекрестие
                mapWindow.screenCross.Hide();

                // Уведомим пользователя о том, что точка А обработана
                mapWindow.SetStatus("Точка А установлена");
            }

            if (BPointWaiting)
            {
                // Добавление новой точки на карте. Получение её координат в пикселях и в метрах
                BPoint = mapWindow.GetPositionOnMap();
                BPointWaiting = false;

                // Разместим маркер по полученным координатам, 
                BPointMarker.SetLocation(BPoint);
                BPointMarker.Show();

                // Отобразим координаты точки A
                Point printingPoint = DirectGeodCalculations.PixelsToSquareCords(BPoint);
                PointBXCord.Text = $"X: {printingPoint.X:f0}";
                PointBYCord.Text = $"Y: {printingPoint.Y:f0}";

                // Уберём перекрестие
                mapWindow.screenCross.Hide();

                // Уведомим пользователя о том, что точка А обработана
                mapWindow.SetStatus("Точка B установлена");
            }

            if (CPointWaiting)
            {
                // Добавление новой точки на карте. Получение её координат в пикселях и в метрах
                CPoint = mapWindow.GetPositionOnMap();
                CPointWaiting = false;

                // Разместим маркер по полученным координатам, 
                CPointMarker.SetLocation(CPoint);
                CPointMarker.Show();

                // Отобразим координаты точки A
                Point printingPoint = DirectGeodCalculations.PixelsToSquareCords(CPoint);
                PointCXCord.Text = $"X: {printingPoint.X:f0}";
                PointCYCord.Text = $"Y: {printingPoint.Y:f0}";

                // Уберём перекрестие
                mapWindow.screenCross.Hide();

                // Уведомим пользователя о том, что точка А обработана
                mapWindow.SetStatus("Точка C установлена");
            }

            if (APointMarker.IsVisible && BPointMarker.IsVisible && CPointMarker.IsVisible)
            {
                // Отобразим получившийся треугольник
                ABDistanceLine.SetPoints(APoint, BPoint);
                BCDistanceLine.SetPoints(BPoint, CPoint);
                CADistanceLine.SetPoints(CPoint, APoint);

                ABDistanceLine.Show();
                BCDistanceLine.Show();
                CADistanceLine.Show();

                // Включим возможность ввести собственные рассчёты
                HaveStudentCalculationsTB.Text = "Точки введены";
                HaveStudentCalculationsTB.Foreground = Brushes.Green;
                StartStudentInput.IsEnabled = true;
            }
        }

        /// <summary>
        /// ТО, что бует выполняться по нажатию правой кнопки мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseRMBDown(object sender, MouseButtonEventArgs e)
        {
            // Если контекстное меню открыто, то закроем его, иначе же откроем его
            if (rmbMenu.IsOpen)
                rmbMenu.IsOpen = false;
            else
                rmbMenu.IsOpen = true;
        }

        public void UpdateScale(double newScale, bool zoomIn)
        {

        }

        /// <summary>
        /// Очиащет все введённые точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPoints(object sender, RoutedEventArgs e)
        {
            APointWaiting = false;
            APoint = new Point(double.MaxValue, double.MaxValue);
            if (APointMarker.IsVisible)
                APointMarker.Hide();

            BPointWaiting = false;
            BPoint = new Point(double.MaxValue, double.MaxValue);
            if (BPointMarker.IsVisible)
                BPointMarker.Hide();

            CPointWaiting = false;
            CPoint = new Point(double.MaxValue, double.MaxValue);
            if (CPointMarker.IsVisible)
                CPointMarker.Hide();

            ABDistanceLine.Hide();
            BCDistanceLine.Hide();
            CADistanceLine.Hide();

            HaveStudentCalculationsTB.Text = "Введите точки на карте";
            HaveStudentCalculationsTB.Foreground = Brushes.Red;
            StartStudentInput.IsEnabled = false;
        }

        private void InitStudentCalculationsInput(object sender, RoutedEventArgs e)
        {
            if (studentCalculations == null)
            {
                studentCalculations = new MeasuredDirectGeodInput(mapWindow, this);
                studentCalculations.Show();
            }
            else
                studentCalculations.Show();
        }

        /// <summary>
        /// Начинаем ожидание ввода точки A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitPointAInput(object sender, RoutedEventArgs e)
        {
            //  Включим ввод точки А. Выключим ввод остальных точек
            APointWaiting = true;
            BPointWaiting = false;
            CPointWaiting = false;

            // Покажем перекрестие
            mapWindow.screenCross.Show();

            // Оповестим пользователя о начале ожидания ввода точки А
            mapWindow.SetStatus("Используйте ЛКМ для выбора местоположения точки А");
        }


        /// <summary>
        /// Начинаем начало ввода точки B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitPointBInput(object sender, RoutedEventArgs e)
        {
            //  Включим ввод точки B. Выключим ввод остальных точек
            APointWaiting = false;
            BPointWaiting = true;
            CPointWaiting = false;

            // Покажем перекрестие
            mapWindow.screenCross.Show();

            // Оповестим пользователя о начале ожидания ввода точки B
            mapWindow.SetStatus("Используйте ЛКМ для выбора местоположения точки B");
        }


        /// <summary>
        /// Ожидаем начало ввода точки C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitPointCInput(object sender, RoutedEventArgs e)
        {
            //  Включим ввод точки C. Выключим ввод остальных точек
            APointWaiting = false;
            BPointWaiting = false;
            CPointWaiting = true;

            // Покажем перекрестие
            mapWindow.screenCross.Show();

            // Оповестим пользователя о начале ожидания ввода точки C
            mapWindow.SetStatus("Используйте ЛКМ для выбора местоположения точки C");
        }
    }
}

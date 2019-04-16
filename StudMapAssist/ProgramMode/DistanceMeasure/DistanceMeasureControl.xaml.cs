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

using StudMapAssist;
using StudMapAssist.Graphics;

namespace StudMapAssist.ProgramMode.DistanceMeasure
{
    /// <summary>
    /// Логика взаимодействия для DistanceMeasureControl.xaml
    /// </summary>
    public partial class DistanceMeasureControl : Page, IProgramMode, IDisposable
    {
        /// <summary>
        /// Ссылка на главное окно с отображаемой картой
        /// </summary>
        MapWindow mapWindow;



        /// <summary>
        /// Флаг. Если true, значит программа сейчас ожидает ввода новой точки
        /// </summary>
        private bool FirstPointWaiting = false;

        /// <summary>
        /// Флаг. Если true, значит программа сейчас ожидает ввода новой точки
        /// </summary>
        private bool SecondPointWaiting = false;
        


        /// <summary>
        /// Первая точка, которая учавствует в измерении расстояний
        /// </summary>
        private Point markerFirstPoint = new Point(double.MaxValue, double.MaxValue);

        /// <summary>
        /// Вторая точка которая учавствует в измерении расстояний
        /// </summary>
        private Point markerSecondPoint = new Point(double.MaxValue, double.MaxValue);



        /// <summary>
        /// Делегат, который передаётся главному окну
        /// </summary>
        MouseButtonEventHandler mouseLMBHandler;

        /// <summary>
        /// Делегат, который передаётся главному окну
        /// </summary>
        MouseButtonEventHandler mouseRMBHandler;



        /// <summary>
        /// Цвет первого маркера
        /// </summary>
        private SolidColorBrush FIRST_MARKER_BRUSH = Brushes.Blue;

        /// <summary>
        /// Цвет второго маркера
        /// </summary>
        private SolidColorBrush SECOND_MARKER_BRUSH = Brushes.Green;



        /// <summary>
        /// Маркер, который показывает местоположение первой точки
        /// </summary>
        private PointMarker firstPointMarker;

        /// <summary>
        /// Маркер, который показывает местоположение второй точки
        /// </summary>
        private PointMarker secondPointMarker;



        /// <summary>
        /// Линия, которая наглядно показывает вычисляемое расстоние
        /// </summary>
        private DistanceLine distanceViewer;



        /// <summary>
        /// Контестное меню, которое вызывается по нажатию правой кнопки мыши
        /// </summary>
        private ContextMenu rmbMenu;

        /// <summary>
        /// Изменить размер маркеров в соответствии с новым зумом карты
        /// </summary>
        /// <param name="newScale">Новое значение, которое показывает настоящее увеличение карты</param>
        public void UpdateMarkersScale(double newScale)
        {
            firstPointMarker.ZoomOut(newScale);
            secondPointMarker.ZoomOut(newScale);
        }

        /// <summary>
        /// Стандартный конструктор. Инициализирует панель рассчёта дистанций
        /// </summary>
        /// <param name="w">Ссылка на главное окно с отображаемой картой</param>
        public DistanceMeasureControl(MapWindow w)
        {
            InitializeComponent();

            mapWindow = w;


            // Создаём делегаты
            mouseLMBHandler = new MouseButtonEventHandler(MouseLMBDown);
            mouseRMBHandler = new MouseButtonEventHandler(MouseRMBDown);

            // Подписываем их на соответствующие события в главном окне
            mapWindow.MainPanel.MouseLeftButtonDown += mouseLMBHandler;
            mapWindow.MainPanel.MouseRightButtonDown += mouseRMBHandler;

            // Инициализируем DistanceLine, добавляем её на главную панель
            distanceViewer = new DistanceLine(mapWindow.MapPanel, FIRST_MARKER_BRUSH, SECOND_MARKER_BRUSH);
            distanceViewer.Hide();

            // Инициализируем PointMarker-ы, добавляем их на главную панель
            firstPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            firstPointMarker.SetStrokeColor(FIRST_MARKER_BRUSH);
            firstPointMarker.Hide();

            secondPointMarker = new PointMarker(mapWindow.MapPanel, 0, 0);
            secondPointMarker.SetStrokeColor(SECOND_MARKER_BRUSH);
            secondPointMarker.Hide();

            rmbMenu = this.FindResource("rmbMenu") as ContextMenu;
            rmbMenu.PlacementTarget = mapWindow.MainPanel;

            mapWindow.SetStatus("Включён режим: \"Измерение расстояний\"");
        }

        /// <summary>
        /// Программа ждёт ввода первой точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitFirstPointInput(object sender, RoutedEventArgs e)
        {
            // Если программа ожидает ввода второй точки - отключаем его, поставив false на соответствующий флаг
            if (SecondPointWaiting)
                SecondPointWaiting = false;

            // Говорим пользователю, что запущено ожидание ввода первой точки. Выставляем true на соответствующий флаг
            mapWindow.SetStatus("Используйте ЛКМ для выбора местоположения первой точки");
            FirstPointWaiting = true;

            // Покажем перекрестие
            mapWindow.screenCross.Show();
        }

        /// <summary>
        /// Программа ждёт ввода второй точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitSecondPointInput(object sender, RoutedEventArgs e)
        {
            // Если программа ожидает ввода второй точки - отключаем его, поставив false на соответствующий флаг
            if (FirstPointWaiting)
                FirstPointWaiting = false;

            // Говорим пользователю, что запущено ожидание ввода первой точки. Выставляем true на соответствующий флаг
            mapWindow.SetStatus("Используйте ЛКМ для выбора местоположения второй точки");
            SecondPointWaiting = true;

            // Покажем перекрестие
            mapWindow.screenCross.Show();
        }

        /// <summary>
        /// Производит отписку от событий главной формы и удаляет элемены с MapPanel
        /// </summary>
        public void Dispose()
        { 
            mapWindow.MainPanel.MouseLeftButtonDown -= mouseLMBHandler;
            mapWindow.MainPanel.MouseRightButtonDown -= mouseRMBHandler;

            mapWindow.MapPanel.Children.Remove(firstPointMarker.GetPath());
            mapWindow.MapPanel.Children.Remove(secondPointMarker.GetPath());
            mapWindow.MapPanel.Children.Remove(distanceViewer.GetPath());

            mapWindow.CurrentModeControl.Resources.Remove(this);
        }

        /// <summary>
        /// Возвращает ссылку на данную страницу (используется для метода Navigate в главном окне)
        /// </summary>
        /// <returns></returns>
        public Page GetPage() => this;

        /// <summary>
        /// Обработчик нажатий левой кнопки мыши. Подписка идёт при инициализации данного Page на LeftMouseDown на MapWindow.MainPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseLMBDown(object sender, MouseButtonEventArgs e)
        {
            if (FirstPointWaiting)
            {
                markerFirstPoint = mapWindow.GetPositionOnMap();
                FirstPointWaiting = false;

                Point squareCordFirstPoint = DirectGeodTask.DirectGeodCalculations.PixelsToSquareCords(markerFirstPoint);

                FirstPointXCordView.Text = $"X: {squareCordFirstPoint.X:f2}";
                FirstPointYCordView.Text = $"Y: {squareCordFirstPoint.Y:f2}";

                mapWindow.SetStatus($"Первая точка установлена: X:{squareCordFirstPoint.X:f2} Y:{squareCordFirstPoint.Y:f2}");

                firstPointMarker.SetLocation(markerFirstPoint);
                firstPointMarker.Show();

                mapWindow.screenCross.Hide();
            }

            if (SecondPointWaiting)
            {
                markerSecondPoint = mapWindow.GetPositionOnMap();
                SecondPointWaiting = false;

                Point squareCordSecondPoint = DirectGeodTask.DirectGeodCalculations.PixelsToSquareCords(markerSecondPoint);

                SecondPointXCordView.Text = $"X: {squareCordSecondPoint.X:f2}";
                SecondPointYCordView.Text = $"Y: {squareCordSecondPoint.Y:f2}";

                mapWindow.SetStatus($"Вторая точка установлена: X:{squareCordSecondPoint.X:f2} Y:{squareCordSecondPoint.Y:f2}");

                secondPointMarker.SetLocation(markerSecondPoint);
                secondPointMarker.Show();

                mapWindow.screenCross.Hide();
            }

            // Если обе точки были заданы, то можно уже рассчитывать расстояние
            if (markerFirstPoint.X != double.MaxValue && markerSecondPoint.X != double.MaxValue)
            {
                DistanceView.Text = $"Растояние: {GeodCalculations.CalcLength(markerFirstPoint, markerSecondPoint):f2}m";
                distanceViewer.SetPoints(markerFirstPoint, markerSecondPoint);
                distanceViewer.Show();
            }
        }

        /// <summary>
        /// Обработчик нажатий правой кнопки мыши. Подписка идёт при инициализации данного Page на LeftMouseDown на MapWindow.MainPanel
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

        private void ClearPoints(object sender, RoutedEventArgs e)
        {
            markerFirstPoint.X = double.MaxValue;
            markerFirstPoint.Y = double.MaxValue;
            markerSecondPoint.X = double.MaxValue;
            markerSecondPoint.Y = double.MaxValue;
            firstPointMarker.Hide();
            secondPointMarker.Hide();
            distanceViewer.Hide();

            FirstPointXCordView.Text = "X: -";
            FirstPointYCordView.Text = "Y: -";
            SecondPointXCordView.Text = "X: -";
            SecondPointYCordView.Text = "Y: -";
            DistanceView.Text = "Расстояние: ";

            mapWindow.SetStatus("Точки очищены");
        }

        /// <summary>
        /// Обновим размеры маркеров для соответствующего зума карты
        /// </summary>
        /// <param name="newScale">Новое значение зума карты</param>
        /// <param name="zoomIn">Если true, то идёт приближение. Иначе идёт отдаление</param>
        public void UpdateScale(double newScale, bool zoomIn)
        {
            if (zoomIn)
            {
                firstPointMarker.ZoomIn(newScale);
                secondPointMarker.ZoomIn(newScale);
                distanceViewer.ZoomIn(newScale);
            }
            else
            {
                firstPointMarker.ZoomOut(newScale);
                secondPointMarker.ZoomOut(newScale);
                distanceViewer.ZoomOut(newScale);
            }
        }
    }
}

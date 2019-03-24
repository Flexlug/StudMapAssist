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

using StudMapAssist.ProgramMode;

namespace StudMapAssist.ProgramMode
{
    /// <summary>
    /// Логика взаимодействия для ChangeProgramMode.xaml
    /// </summary>
    public partial class ChangeProgramMode : Window
    {
        MapWindow mapWindow;

        public ChangeProgramMode(MapWindow w)
        {
            InitializeComponent();

            mapWindow = w;
        }

        private void EnableDistanceMeasureControl(object sender, MouseButtonEventArgs e)
        {
            (mapWindow.currentMode as IDisposable).Dispose();
            DistanceMeasure.DistanceMeasureControl control = new DistanceMeasure.DistanceMeasureControl(mapWindow);
            mapWindow.currentMode = control;
            mapWindow.CurrentModeControl.Navigate(control);

            Close();
        }

        private void EnableDirectGeodDifference(object sender, MouseButtonEventArgs e)
        {

            (mapWindow.currentMode as IDisposable).Dispose();
            DirectGeodTask.DirectGeodTaskControl control = new DirectGeodTask.DirectGeodTaskControl(mapWindow);
            mapWindow.currentMode = control;
            mapWindow.CurrentModeControl.Navigate(control);

            Close();
        }

        private void SelectedDistanceMeasureControl(object sender, RoutedEventArgs e)
        {
            ModePreview.BeginInit();
            ModePreview.Source = new BitmapImage(new Uri("pack://application:,,,/ProgramModePreviews/DistanceMeasure.png", UriKind.Absolute));
            ModePreview.EndInit();
            ModeDescription.Text = "Измерение расстояний между двумя точками";
        }

        private void SelectedDirectGeodDifference(object sender, RoutedEventArgs e)
        {
            ModePreview.BeginInit();
            ModePreview.Source = new BitmapImage(new Uri("pack://application:,,,/ProgramModePreviews/DirectGeod.png", UriKind.Absolute));
            ModePreview.EndInit();
            ModeDescription.Text = "Проверка выполнения лабораторной работы по решению прямой геодезической задачи";
        }
    }
}

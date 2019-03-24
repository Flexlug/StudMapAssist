using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudMapAssist
{
    public class MapControl
    {
        /// <summary>
        /// Ссылка на главное окно
        /// </summary>
        MapWindow mainWindow;

        public MapControl(MapWindow window)
        {
            mainWindow = window;
        }
    }
}

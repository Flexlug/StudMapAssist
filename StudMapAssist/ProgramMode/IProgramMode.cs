using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudMapAssist.ProgramMode
{
    public interface IProgramMode : IDisposable
    {
        void MouseRMBDown(object sender, MouseButtonEventArgs e);
        void MouseLMBDown(object sender, MouseButtonEventArgs e);
        void UpdateScale(double newScale, bool zoomIn);
        Page GetPage();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace SoftwareKobo.UniversalToolkit.PInvoke
{
    public class User32
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(VirtualKey vKey);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
    }
}

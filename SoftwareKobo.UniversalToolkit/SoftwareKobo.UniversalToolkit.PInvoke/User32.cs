using System.Runtime.InteropServices;
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
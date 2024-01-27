using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsHID;




public static class Device
{
    const int HORZRES = 8; // Width in pixels
    const int VERTRES = 10; // Height in pixels
/*    const int LOGPIXELSX = 88; // DPI for X-axis
    const int LOGPIXELSY = 90; // DPI for Y-axis

    const int SM_CXSCREEN = 0;
    const int SM_CYSCREEN = 1;*/

/*    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);*/

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
/*    [DllImport("user32.dll")]
    public static extern uint GetDpiForSystem();*/
    [DllImport("user32.dll")]
    public static extern bool SetProcessDPIAware();


    public static int width { get; private set; }
    public static int height { get; private set; }
    static Device()
    {
        _=SetProcessDPIAware();
        IntPtr hdc = GetDC(IntPtr.Zero);

        width = GetDeviceCaps(hdc, HORZRES);
        height = GetDeviceCaps(hdc, VERTRES);
        //int scale = GetDeviceCaps(hdc, LOGPIXELSX);
        //int dpiY = GetDeviceCaps(hdc, LOGPIXELSY);
        //float dpi = scale / 96f;
        ReleaseDC(IntPtr.Zero, hdc);
        //width= (int)(width * dpi); height= (int)(height * dpi);
    
    }
    public static string MachineName { get
        {
            string machineName = Environment.MachineName;
            string text = machineName;
            if (machineName.Length > 5)
            {
                text = machineName.Substring(machineName.Length - 5);
            }
            return text;
        } }
    public static string Resolution { get {
            return $"{width}x{height}";
        } }
}


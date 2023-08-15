using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib;

public static class Device
{
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
            return $"{ScreenResolution.GetScreenWidth()}x{ScreenResolution.GetScreenHeight()}";
        } }
}
public static class ScreenResolution
{
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);
    public static int GetScreenWidth()
    {
        return GetSystemMetrics(1);
    }
    public static int GetScreenHeight()
    {
        return GetSystemMetrics(2);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileMouse;

internal class LowPasser
{
    public static double parameter { set; get; } = 0.01;
    public static bool Enable {  set; get; } = false;
    float? value ;
    DateTime lastTime;
    public float step(float newValue)
    {
        if (!Enable)
        {
            return newValue;
        }
        if (value==null)
        {
            lastTime = DateTime.Now;
            value = newValue;
            return value.Value;
        }
        else
        {
            var timeNow = DateTime.Now;
            var duration = (timeNow - lastTime).Microseconds / 1.0e6;
            var inverse = (newValue - value.Value) / duration * parameter;
            var v = (float)(newValue - Math.Abs(inverse-newValue)>0.5?0.5:inverse);
            lastTime = timeNow;
            value = v;
            return v;
        }

    }
}

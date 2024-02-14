using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileMouse;

internal class Integral
{
    bool isNegtiveable = true;

    public Integral(bool isNegtiveable=true) {
        this.isNegtiveable=isNegtiveable;
    }
    float sum=0;
    DateTime time = DateTime.Now;
    float lastValue = 0;
    float step(float value, DateTime time)
    {
        var deltaTime = time - this.time;
        sum +=(float)(( value+lastValue )/2* (deltaTime.TotalMilliseconds) /1e3f);
        if (!isNegtiveable)
        {
            if (sum < 0)
            {
                sum = 0;
            }
        }
        lastValue = value;
        this.time = time;
        return sum;
    }
    public float step(float value)
    {
        return step(value, DateTime.Now);
    }
    public void reset()
    {
        sum = 0;
    }
}

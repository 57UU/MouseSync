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
    int time = DateTime.Now.Nanosecond;
    float lastValue = 0;
    float step(float value, int time)
    {
        var deltaTime = time - this.time;
        sum +=( value+lastValue )/2* (deltaTime) /1e9f;
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
        return step(value, DateTime.Now.Nanosecond);
    }
    public void reset()
    {
        sum = 0;
    }
}

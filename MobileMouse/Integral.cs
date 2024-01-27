using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileMouse;

internal class Integral
{
    double sum=0;
    int time = DateTime.Now.Millisecond;
    double step(double value, int time)
    {
        value += value * (time - this.time);
        return value;
    }
    public double step(double value)
    {
        return step(value, DateTime.Now.Millisecond);
    }
    public void reset()
    {
        sum = 0;
    }
}

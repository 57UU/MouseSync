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
    double sum=0;
    int time = DateTime.Now.Millisecond;
    double step(double value, int time)
    {

        sum += value * (time - this.time)/1000;
        if (!isNegtiveable)
        {
            if (sum < 0)
            {
                sum = 0;
            }
        }
        return sum;
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

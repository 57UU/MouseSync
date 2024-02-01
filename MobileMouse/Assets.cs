using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;

namespace MobileMouse;

public class Assets
{
    public Thread mainThread;
    static Assets()
    {

    }
    public static Assets instance =new Assets();
    private Assets() { }
    public ObservableCollection<string> logs = new();
    public MouseInputData mouseInputData = new MouseInputData() { hookStruct=new MSLLHOOKSTRUCT { pt = new POINT() ,mouseData=0} };
    
    public double scale = 1000;
    public double X_x { set { 
            
            x_x = value * scale;
            var s = mouseInputData.hookStruct;
            s.pt.X = ((int)(x_x)) ;
            mouseInputData.hookStruct = s;
        }get { return x_x; } }
    private double x_x=0;
    public double X_y
    {
        set
        {
            x_y = value * scale;
            var s = mouseInputData.hookStruct;
            s.pt.Y = ((int)(x_y));
            mouseInputData.hookStruct = s;
        }
        get { return x_y; }
    }
    public int MouseCode { set {
            mouseInputData.code = value;
        }
        get
        {
            return mouseInputData.code;
        }
    }
    private double x_y = 0;
    public double magnitude_a_xy;
    public Vector3 rawData;

}

using MobileMouse.Controls;
using MouseSyncServerCore;
using System.Numerics;
using WindowsHID;

namespace MobileMouse;

public partial class MainPage : ContentPage
{
    ServerCore server; MouseButton left; MouseButton right;
    public MainPage()
    {
        InitializeComponent();
        if (!Accelerometer.Default.IsSupported)
        {
            DisplayAlert("Warn", "Your device does NOT support accelerometer", "OK");
            Navigation.PopAsync();
        }
        else
        {
            loadAccelerator();
        }
        left = new("Left");
        right = new("Right");
        grid.Add(left);
        grid.Add(right,1,0);
        reset();
        loadServer();
        
    }
    void loadServer()
    {

         server= new ServerCore()
         {
             LogHandler = (s) => { Assets.instance.logs.Add(s); }
         };
        left.buttonDown += () =>
        {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
        left.buttonRelease += () =>
        {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
        right.buttonDown += () =>
        {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
        right.buttonRelease += () =>
        {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
    }
    void loadAccelerator()
    {
        Accelerometer.Default.ReadingChanged += Default_ReadingChanged; ;
        Accelerometer.Default.Start(SensorSpeed.Fastest);
    }

    AccelerometerData? lastData;
    Integral x_a = new();
    Integral y_a = new();
    Integral x_v = new();
    Integral y_v = new();
    private void Default_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var current = e.Reading;
        if(lastData!=null)
        {
            //check weather static
            var delta = current.Acceleration - lastData.Value.Acceleration;
            if (delta.Length() < 0.01f)
            {
                reset();
            }
            double v_x = x_a.step(current.Acceleration.X);
            double v_y=y_a.step(current.Acceleration.Y);
            double x_x = x_v.step(v_x);
            double x_y=y_v.step(v_y);   
            
        }
        lastData = current;
    }
    Quaternion rotate;
    void reset()
    {
        OrientationSensor.Default.ReadingChanged += (s,e)=> { 
            rotate = e.Reading.Orientation;
            OrientationSensor.Default.Stop();
        };
        OrientationSensor.Default.Start(SensorSpeed.UI);
        x_a.reset();
        y_a.reset();
        x_v.reset();
        y_v.reset();
    }
}



using CommonLib;
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
    LogHandler logHandler = Assets.instance.logs.Add;
    async void loadServer()
    {
        await Task.Delay(100);
        server = new ServerCore(logHandler);
        left.buttonDown += () =>
        {
            sendData(513);

        };
        left.buttonRelease += () =>
        {
            sendData(514);
        };
        right.buttonDown += () =>
        {

            sendData(516);
        };
        right.buttonRelease += () =>
        {
            sendData(517);
        };
        var realIP=Utils.getIPAddress();
        MouseSyncServerCore.Network.boardcastIP = realIP;
        logHandler("override IPv4 to "+realIP);
    }
    void sendData(int mouseCode)
    {
        Assets.instance.MouseCode = mouseCode;
        server.mouseHandler(this, Assets.instance.mouseInputData);
    }
    async void loadAccelerator()
    {
        await Task.Delay(200);
        Accelerometer.Default.ReadingChanged += Default_ReadingChanged; ;
        Accelerometer.Default.Start(SensorSpeed.Fastest);
    }
    void stopAccelerator()
    {
        Accelerometer.Default.ReadingChanged -= Default_ReadingChanged; ;
        Accelerometer.Default.Stop();
    }

    Vector3? lastData;
    Integral x_a = new();
    Integral y_a = new();
    Integral x_v = new(false);
    Integral y_v = new(false);
    DateTime? stopTime;
    LowPasser lowPasserX = new();
    LowPasser lowPasserY = new();
    LowPasser lowPasserZ = new();
    float avg_x_value = 0;
    float avg_y_value = 0;
    private void Default_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var current = e.Reading.Acceleration;
        var raw=current;
        
        //apply lowpass
        current.X=lowPasserX.step(current.X-avg_x_value);
        current.Y=lowPasserY.step(current.Y-avg_y_value);
        current.Z=lowPasserZ.step(current.Z);
        Assets.instance.rawData = current;
        float v_x;
        float v_y;
        if (lastData!=null &&(MathF.Abs(current.Z-1)<0.019f))
        {
            //current = (lastData.Value + current) / 2;

            Assets.instance.magnitude_a_xy = new Vector2(current.X, current.Y).Length();
            var magnitude = new Vector3(raw.X-lastData.Value.X, raw.Y-lastData.Value.Y, raw.Z - lastData.Value.Z).Length();
            if (magnitude < 0.016f )
            {//not moving
                if(stopTime==null)
                {
                    stopTime = DateTime.Now;
                }
                else
                {
                    var deltaTime = (DateTime.Now - stopTime.Value).Milliseconds;
                    if (deltaTime > 50)
                    {
                        avg_x_value = (raw.X + avg_x_value) / 2;
                        avg_y_value = (raw.Y + avg_y_value) / 2;
                        if (deltaTime > 100)
                        {
                            reset();
                            stopTime = null;
                        }
                    }


                }

                v_x = x_a.step(0);
                v_y = y_a.step(0);
            }
            else//moving
            {
                stopTime = null;
                v_x = x_a.step(current.X);
                v_y = y_a.step(-current.Y);
            }

            float x_x = x_v.step(v_x);
            float x_y =y_v.step(v_y);   
            Assets.instance.X_x = x_x;
            Assets.instance.X_y = x_y;
        }
        lastData = current;
        sendData(512);//moving
    }

    void reset()
    {
        //logHandler("Reset Accelerator");
        x_a.reset();
        y_a.reset();
    }
}



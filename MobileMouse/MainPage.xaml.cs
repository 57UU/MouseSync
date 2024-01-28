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
        Accelerometer.Default.Start(SensorSpeed.Game);
    }

    AccelerometerData? lastData;
    Integral x_a = new();
    Integral y_a = new();
    Integral x_v = new(false);
    Integral y_v = new(false);
    private void Default_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var current = e.Reading;
        if(lastData!=null)
        {
            //check weather static
            var delta = current.Acceleration - lastData.Value.Acceleration;

            Assets.instance.magnitude_x = new Vector2(delta.X,delta.Y).Length();
            Assets.instance.magnitude_a = new Vector2(current.Acceleration.X, current.Acceleration.Y).Length();
            if (Assets.instance.magnitude_a < 0.019f)
            {
                reset();
            }
            double v_x = x_a.step(current.Acceleration.X);
            double v_y=y_a.step(-current.Acceleration.Y);
            double x_x = x_v.step(v_x);
            double x_y=y_v.step(v_y);   
            Assets.instance.X_x = x_x;
            Assets.instance.X_y = x_y;
        }
        lastData = current;
        sendData(512);
    }

    void reset()
    {
        x_a.reset();
        y_a.reset();
    }
}



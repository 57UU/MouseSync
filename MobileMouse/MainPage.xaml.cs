using MobileMouse.Controls;
using MouseSyncServerCore;
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
        left = new("Left");
        right = new("Right");
        grid.Add(left);
        grid.Add(right,1,0);
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
}



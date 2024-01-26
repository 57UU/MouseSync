using MobileMouse.Controls;
using MouseSyncServerCore;
using WindowsHID;

namespace MobileMouse;

public partial class MainPage : ContentPage
{
    ServerCore server= new ServerCore(isCreateFakeWindowAndHook:false,isHook:false) {
        LogHandler = (s) => { Assets.instance.logs.Add(s); }
    };
    public MainPage()
    {
        InitializeComponent();
        MouseButton left = new("Left");
        MouseButton right = new("Right");
        grid.Add(left);
        grid.Add(right,1,0);
        left.buttonDown += () => {
            server.mouseHandler(this,Assets.instance.mouseInputData);
        };
        left.buttonRelease += () => {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
        right.buttonDown += () => {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
        right.buttonRelease += () => {
            server.mouseHandler(this, Assets.instance.mouseInputData);
        };
    }
}

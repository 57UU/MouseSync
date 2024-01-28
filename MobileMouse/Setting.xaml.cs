using CommonLib;

namespace MobileMouse;

public partial class Setting : ContentPage
{
	public Setting()
	{
		InitializeComponent();
		isDebug.IsToggled = false;

	}

    private void isDebug_Toggled(object sender, ToggledEventArgs e)
    {
        MouseSyncServerCore.Entry.isDebug = isDebug.IsToggled;
    }
}
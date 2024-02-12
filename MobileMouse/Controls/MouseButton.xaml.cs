

namespace MobileMouse.Controls;

public partial class MouseButton : ContentView
{
	public event Action? buttonDown;
    public event Action? buttonRelease;
	public MouseButton(string text="")
	{
		InitializeComponent();
        view.Text = text;
        view.BackgroundColor = AppColors.buttonUp;


    }

    private void Pressed(object sender, EventArgs e)
    {
        view.BackgroundColor =AppColors.buttonDown;
        buttonDown?.Invoke();
    }

    private void Released(object sender, EventArgs e)
    {
        view.BackgroundColor = AppColors.buttonUp;
        buttonRelease?.Invoke();
    }
}
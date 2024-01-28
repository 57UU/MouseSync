using System.Text;

namespace MobileMouse;

public partial class Test : ContentPage
{
	public Test()
	{
		InitializeComponent();
		update();

    }
	private async void update()
	{
		var assets = Assets.instance;
		while (true)
		{
			await Task.Delay(100);
			StringBuilder stringBuilder = new();
			stringBuilder.AppendLine("Magnitude_a: "+assets.magnitude_a);
			stringBuilder.AppendLine("x: "+assets.X_x);
			stringBuilder.AppendLine("y: "+assets.X_y);
			xLabel.Text=stringBuilder.ToString();
		}
	}
}
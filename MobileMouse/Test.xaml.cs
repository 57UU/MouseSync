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
			stringBuilder.AppendLine("Magnitude_a: "+assets.magnitude_a_xy);
			stringBuilder.AppendLine("x: "+assets.X_x);
			stringBuilder.AppendLine("y: "+assets.X_y);
			stringBuilder.AppendLine("z_a: "+Math.Abs(assets.rawData.Z-1));
			xLabel.Text=stringBuilder.ToString();
		}
	}
}
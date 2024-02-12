using Microsoft.Extensions.Logging;

namespace MobileMouse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            CommonLib.Info.CLIENT_SETTING = Path.Combine(FileSystem.Current.AppDataDirectory, "setting.config");
            CommonLib.Info.load();
            
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

using Microsoft.Extensions.Logging;

namespace Samples.MusicCollection.App;

/// <summary>
/// 
/// </summary>
public static class MauiProgram
{
    #region Static Method Declarations

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder mauiAppBuilder = MauiApp.CreateBuilder();
        mauiAppBuilder.UseMauiApp<App>()
                      .ConfigureFonts(fonts =>
                      {
                          fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                          fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                      });

#if DEBUG
        mauiAppBuilder.Logging.AddDebug();
#endif

        MauiApp mauiApp = mauiAppBuilder.Build();
        return mauiApp;
    }

    #endregion
}

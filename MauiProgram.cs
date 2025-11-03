using Microsoft.Extensions.Logging;
using BingoMAUI.Services;
using BingoMAUI.ViewModels;
using BingoMAUI.Views;
using MauiIcons.FontAwesome;

namespace BingoMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseFontAwesomeMauiIcons()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<IBingoBoardService, BingoBoardService>();

            // Register ViewModels
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<BoardConfigPageViewModel>();
            builder.Services.AddTransient<BoardViewPageViewModel>();

            // Register Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<BoardConfigPage>();
            builder.Services.AddTransient<BoardViewPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

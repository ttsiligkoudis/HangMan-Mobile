using CommunityToolkit.Maui.Markup;
using HangMan.ViewModels;

namespace HangMan;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiApp<App>().UseMauiCommunityToolkitMarkup()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<OnlineViewModel>();
        builder.Services.AddTransient<OnlineGameViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<GamePage>();
        builder.Services.AddTransient<OnlinePage>();
        builder.Services.AddTransient<OnlineGamePage>();

        var app = builder.Build();

        return app;
	}
}

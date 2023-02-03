using CommunityToolkit.Maui.Markup;
using HangMan.Helpers;
using HangMan.ViewModels;
using Microsoft.Extensions.Configuration;

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

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<GamePage>();
        builder.Services.AddTransient<OnlinePage>();

        builder.Services.AddSingleton<IAlertService, AlertService>();

        var app = builder.Build();

        return app;
	}
}

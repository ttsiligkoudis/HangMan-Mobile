namespace HangMan;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
		Routing.RegisterRoute(nameof(OnlinePage), typeof(OnlinePage));
        Routing.RegisterRoute(nameof(OnlineGamePage), typeof(OnlineGamePage));
    }
}

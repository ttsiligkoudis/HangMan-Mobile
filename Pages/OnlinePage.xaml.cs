namespace HangMan;

using HangMan.ViewModels;

public partial class OnlinePage : ContentPage
{
    public OnlinePage(OnlineViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = (OnlineViewModel)BindingContext;
        vm.ConnectionID = null;
        vm.WordStr = null;
        vm.ConnectionEstablished = false;
        Task.Run(async () => await vm.ConnectionInit());
    }
}
namespace HangMan;

using HangMan.Helpers;
using HangMan.ViewModels;

public partial class OnlinePage : ContentPage
{
    public Terminology Terminology { get; set; }
    public Language Language { get; set; }

    public OnlinePage(OnlineViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = (OnlineViewModel)BindingContext;
        Terminology = vm.Terminology;
        Language = vm.Language;
    }

    private void UserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        var vm = (OnlineViewModel)BindingContext;
        vm.UserName = UserName.Text.Trim();
    }

    private void ConnectionID_TextChanged(object sender, TextChangedEventArgs e)
    {
        var vm = (OnlineViewModel)BindingContext;
        vm.ConnectionID = ConnectionID.Text.Trim();
    }
}
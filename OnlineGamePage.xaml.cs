namespace HangMan;

using HangMan.Helpers;
using HangMan.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

public partial class OnlineGamePage : ContentPage
{
    public Terminology Terminology { get; set; }
    public Language Language { get; set; }
    public string UserName { get; set; }
    public HubConnectionHelper HubConnection { get; set; }

    public OnlineGamePage(OnlineGameViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = (OnlineGameViewModel)BindingContext;
        Terminology = vm.Terminology;
        Language = vm.Language;
        UserName = vm.UserName;
        HubConnection = new HubConnectionHelper(UserName);
        lblChat.Text = HubConnection.ReceiveMessage();
        HubConnection.Run();
    }

    private async void btnSend_Clicked(object sender, EventArgs e)
    {
        HubConnection.SendMessageToAll(txtMessage.Text);
        txtMessage.Text = String.Empty;
        lblChat.Text = HubConnection.ReceiveMessage();
    }
}
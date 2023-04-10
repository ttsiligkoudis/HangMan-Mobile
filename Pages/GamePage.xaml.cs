namespace HangMan;

using HangMan.Helpers;
using HangMan.ViewModels;

public partial class GamePage : ContentPage
{
    public GamePage(GameViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = (GameViewModel)BindingContext;
        vm.WordArea = WordArea;
        vm.FirstRow = FirstRow;
        vm.SecondRow = SecondRow;
        vm.ThirdRow = ThirdRow;

        Chat.IsVisible = vm.GameType != GameType.Local;
        Copy.IsVisible = vm.GameType != GameType.Local;
        ResetBtn.IsVisible = vm.GameType != GameType.Join;
        WordLabel.IsVisible = vm.GameType != GameType.Join;
        WordFrame.IsVisible = vm.GameType != GameType.Join;

        vm.ConnectionInit();
        vm.InitializeGame();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(WordStr.Text))
            WordStr.Text = WordStr.Text.Trim().ToUpper();
    }

    private void PassVisibilityBtn_Clicked(object sender, EventArgs e)
    {
        WordStr.IsPassword = !WordStr.IsPassword;
    }
}
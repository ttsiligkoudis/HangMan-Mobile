namespace HangMan;
using HangMan.ViewModels;


public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }

    private void OnCheckBoxChanged(System.Object sender, System.EventArgs e)
    {
        var vm = (MainViewModel)BindingContext;
        vm.IsChecked = CheckBox.IsChecked;
        vm.Terminology = new(vm.Language, vm.IsChecked);
        StrEntry.Text = string.Empty;
        StartGame.IsEnabled = !vm.IsChecked;
        StartGame.Background = StartGame.IsEnabled ? Color.FromUint(0xFF512BD4) : Colors.Gray;
    }

    private void StrEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var vm = (MainViewModel)BindingContext;
        vm.WordStr = StrEntry.Text.Trim().ToUpper();
        StartGame.IsEnabled = !vm.IsChecked || !string.IsNullOrEmpty(vm.WordStr);
        StartGame.Background = StartGame.IsEnabled ? Color.FromUint(0xFF512BD4) : Colors.Gray;
    }
}
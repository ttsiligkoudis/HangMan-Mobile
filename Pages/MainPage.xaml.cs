namespace HangMan;

using HangMan.Helpers;
using HangMan.ViewModels;


public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(WordStr.Text))
            WordStr.Text = WordStr.Text.Trim().ToUpper();
    }

    private void LngButton_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        var vm = BindingContext as MainViewModel;
        vm.Language = Enum.Parse<Language>(btn.StyleId);
        vm.Terminology = new(vm.Language);

        if (Preferences.ContainsKey(nameof(Language)))
        {
            Preferences.Remove(nameof(Language));
        }
        Preferences.Set(nameof(Language), btn.StyleId);
    }
}
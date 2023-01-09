using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangMan.Helpers;

namespace HangMan.ViewModels
{
    [QueryProperty(nameof(Terminology), nameof(Terminology))]
    [QueryProperty(nameof(Language), nameof(Language))]
    [QueryProperty(nameof(IsChecked), nameof(IsChecked))]
    [QueryProperty(nameof(WordStr), nameof(WordStr))]
    public partial class GameViewModel : ObservableObject
    {
        [ObservableProperty]
        private Terminology terminology;

        [ObservableProperty]
        private Language language;

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private string wordStr;

        [RelayCommand]
        static Task Back() => Shell.Current.GoToAsync("..");
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangMan.Helpers;

namespace HangMan.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private Terminology terminology;

        [ObservableProperty]
        private Language language;

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private string wordStr;

        public MainViewModel()
        {
            Language = Language.English;
            Terminology = new(Language, false);
        }

        [RelayCommand]
        void LanguageToEnglish()
        {
            Language = Language.English;
            Terminology = new(Language, Terminology.Checked);
        }

        [RelayCommand]
        void LanguageToGreek()
        {
            Language = Language.Greek;
            Terminology = new(Language, Terminology.Checked);
        }

        [RelayCommand]
        Task Navigate() =>
            Shell.Current.GoToAsync(nameof(GamePage),
                new Dictionary<string, object>
                {
                    [nameof(Terminology)] = Terminology,
                    [nameof(Language)] = Language,
                    [nameof(IsChecked)] = IsChecked,
                    [nameof(WordStr)] = WordStr
                });

        [RelayCommand]
        Task NavigateOnline() =>
            Shell.Current.GoToAsync(nameof(OnlinePage),
                new Dictionary<string, object>
                {
                    [nameof(Terminology)] = Terminology,
                    [nameof(Language)] = Language
                });
    }
}

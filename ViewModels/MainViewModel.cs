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
        private string wordStr;

        [ObservableProperty]
        private GameType gameType;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private List<string> wordList;

        public MainViewModel()
        {
            var language = Preferences.Get(nameof(Language), null);
            Language = !string.IsNullOrEmpty(language) ? Enum.Parse<Language>(language) : Language.English;
            Terminology = new(Language);
        }

        [RelayCommand]
        async Task Navigate()
        {
            IsLoading = true;
            WordList = StreamReaderHelper.GetWordList(Terminology);
            IsLoading = false;
            await Shell.Current.GoToAsync(nameof(GamePage),
                new Dictionary<string, object>
                {
                    [nameof(Terminology)] = Terminology,
                    [nameof(Language)] = Language,
                    [nameof(WordStr)] = WordStr,
                    [nameof(GameType)] = GameType.Local,
                    [nameof(WordList)] = WordList
                });
        }
            

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

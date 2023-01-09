using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangMan.Helpers;

namespace HangMan.ViewModels
{
    [QueryProperty(nameof(Terminology), nameof(Terminology))]
    [QueryProperty(nameof(Language), nameof(Language))]
    public partial class OnlineViewModel : ObservableObject
    {
        [ObservableProperty]
        private Terminology terminology;

        [ObservableProperty]
        private Language language;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string connectionID;

        [RelayCommand]
        Task JoinGame() =>
            Shell.Current.GoToAsync(nameof(OnlineGamePage),
                new Dictionary<string, object>
                {
                    [nameof(Terminology)] = Terminology,
                    [nameof(Language)] = Language,
                    [nameof(UserName)] = UserName,
                    [nameof(ConnectionID)] = ConnectionID
                });

        [RelayCommand]
        Task CreateGame() =>
            Shell.Current.GoToAsync(nameof(OnlineGamePage),
                new Dictionary<string, object>
                {
                    [nameof(Terminology)] = Terminology,
                    [nameof(Language)] = Language,
                    [nameof(UserName)] = UserName
                });
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using HangMan.Helpers;

namespace HangMan.ViewModels
{
    [QueryProperty(nameof(Terminology), nameof(Terminology))]
    [QueryProperty(nameof(Language), nameof(Language))]
    [QueryProperty(nameof(UserName), nameof(UserName))]
    [QueryProperty(nameof(ConnectionID), nameof(ConnectionID))]
    public partial class OnlineGameViewModel : ObservableObject
    {
        [ObservableProperty]
        private Terminology terminology;

        [ObservableProperty]
        private Language language;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string connectionID;
    }
}

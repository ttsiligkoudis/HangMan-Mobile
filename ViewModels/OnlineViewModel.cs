using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangMan.Helpers;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

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
        private string wordStr;

        [ObservableProperty]
        private GameType gameType;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string connectionID;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private List<string> wordList;

        public bool ConnectionEstablished { get; set; }

        private readonly string baseUrl = "https://chat.myportofolio.eu";

        private HubConnection _connection;

        private readonly IAlertService _alert;

        public OnlineViewModel(IAlertService alert)
        {
            _alert = alert;

            //#if DEBUG
            //    baseUrl = DeviceInfo.Current.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
            //#endif
        }

        public async Task ConnectionInit()
        {
            if (!ConnectionHelper.ConnectionEstablished(_connection))
            {
                _connection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/chatHub")
                .Build();

                _connection.On<string, string, string>("CheckIfConnectionMatchAnswer", (connectionID, word, language) =>
                {
                    if (ConnectionID == connectionID)
                    {
                        WordStr = word;
                        Language = Enum.Parse<Language>(language);
                        Terminology = new Terminology(Language);
                        ConnectionEstablished = true;
                        var action = () => JoinGameAsync();
                        MainThread.BeginInvokeOnMainThread(action);
                    }
                });

                try
                {
                    await _connection.StartAsync();
                }
                catch (Exception ex)
                {
                    await _alert.DisplayAlert("Operation failed", ex.Message, "Ok", "Cancel");
                }
            }
        }

        [RelayCommand]
        async Task CreateGame()
        {
            if (await CheckNameifValid())
            {
                IsLoading = true;
                _connection.DisposeAsync();
                _connection = null;
                WordList = StreamReaderHelper.GetWordList(Terminology);
                IsLoading = false;
                await Shell.Current.GoToAsync(nameof(GamePage),
                    new Dictionary<string, object>
                    {
                        [nameof(Terminology)] = Terminology,
                        [nameof(Language)] = Language,
                        [nameof(GameType)] = GameType.Host,
                        [nameof(UserName)] = UserName,
                        [nameof(WordList)] = WordList
                    });
            }
            
        }

        [RelayCommand]
        async Task JoinGame()
        {
            if (await CheckNameifValid() && await CheckConnectionIdIfValid())
            {
                IsLoading = true;
                if (ConnectionHelper.ConnectionEstablished(_connection))
                {
                    await _connection.InvokeCoreAsync("CheckConnectionID", args: new[] { ConnectionID });
                    await CheckStatus();
                }
                else
                {
                    await _alert.DisplayAlert("Operation failed", "There is a issue with the connection", "Ok", "Cancel");
                }
                
            }
        }

        async void JoinGameAsync()
        {
            if (ConnectionEstablished && ConnectionHelper.ConnectionEstablished(_connection))
            {
                await _connection.InvokeCoreAsync("SendMessageToRoom", args: new[] { UserName, "Joined the Room", ConnectionID });
                _connection.DisposeAsync();
                _connection = null;
                IsLoading = false;

                await Shell.Current.GoToAsync(nameof(GamePage),
                    new Dictionary<string, object>
                    {
                        [nameof(Terminology)] = Terminology,
                        [nameof(Language)] = Language,
                        [nameof(WordStr)] = WordStr,
                        [nameof(GameType)] = GameType.Join,
                        [nameof(UserName)] = UserName,
                        [nameof(ConnectionID)] = ConnectionID
                    });
            }
        }

        async Task CheckStatus()
        {
            await Task.Delay(3000);
            IsLoading = false;
            if (!ConnectionEstablished)
                await _alert.DisplayAlert("Operation failed", $"Room with ID {ConnectionID} wasn't found", "Ok", "Cancel");

        }

        async Task<bool> CheckNameifValid()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                await _alert.DisplayAlert("Operation failed", "Please fill your name first", "Ok", "Cancel");
                return false;
            }
            return true;
        }

        async Task<bool> CheckConnectionIdIfValid()
        {
            if (string.IsNullOrEmpty(ConnectionID))
            {
                await _alert.DisplayAlert("Operation failed", "Connection ID is required", "Ok", "Cancel");
                return false;
            }
            return true;
        }
    }
}

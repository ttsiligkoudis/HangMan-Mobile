using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangMan.Helpers;
using Microsoft.AspNetCore.SignalR.Client;

namespace HangMan.ViewModels
{
    [QueryProperty(nameof(Terminology), nameof(Terminology))]
    [QueryProperty(nameof(Language), nameof(Language))]
    [QueryProperty(nameof(WordStr), nameof(WordStr))]
    [QueryProperty(nameof(GameType), nameof(GameType))]
    [QueryProperty(nameof(UserName), nameof(UserName))]
    [QueryProperty(nameof(ConnectionID), nameof(ConnectionID))]
    [QueryProperty(nameof(WordList), nameof(WordList))]
    public partial class GameViewModel : ObservableObject
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
        private string chatMessages;
       
        [ObservableProperty]
        private string myChatMessage;

        [ObservableProperty]
        private string livesLabel;

        [ObservableProperty]
        private ImageSource imageSource;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private HorizontalStackLayout wordArea;

        [ObservableProperty]
        private HorizontalStackLayout firstRow;

        [ObservableProperty]
        private HorizontalStackLayout secondRow;

        [ObservableProperty]
        private HorizontalStackLayout thirdRow;

        [ObservableProperty]
        private List<string> wordList;

        private readonly string baseUrl = "https://chat.myportofolio.eu";

        private HubConnection _connection;

        private readonly IAlertService _alert;

        private string word;

        private List<string> chat;

        private List<Button> buttons;

        private int lives;

        private List<Label> wordLabels;

        public GameViewModel(IAlertService alert)
        {
            _alert = alert;
            wordLabels = new();
            WordList = new();
            chat = new();
            for (int i = 0; i < 8; i++)
            {
                chat.Add("<br>");
            }
            ChatMessages = string.Join("", chat.ToArray());
            buttons = new();

            //#if DEBUG
            //    baseUrl = DeviceInfo.Current.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
            //#endif
        }

        [RelayCommand]
        async void Back()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }
            WordStr = null;
            word = null;
            connectionID = null;
            await Shell.Current.GoToAsync("..");
        } 

        [RelayCommand]
        async void SendMessage()
        {
            if (_connection != null && !string.IsNullOrEmpty(_connection.ConnectionId) && _connection.State == HubConnectionState.Connected)
                await _connection.InvokeCoreAsync("SendMessageToRoom", args: new[] { UserName, MyChatMessage, ConnectionID });
            MyChatMessage = string.Empty;
        }

        [RelayCommand]
        async void ResetGame()
        {
            if (GameType != GameType.Join)
            {
                word = null;
                InitializeGame();
                if (_connection != null && !string.IsNullOrEmpty(_connection.ConnectionId) && _connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeCoreAsync("SendResetGame", args: new[] 
                        { connectionID, word });
                }
            }
        }

        [RelayCommand]
        async void CopyID()
        {
            await Clipboard.Default.SetTextAsync(ConnectionID);
            await _alert.DisplayAlert("Connection ID was copied", ConnectionID, "Ok", "Cancel");
        }

        public async Task ConnectionInit()
        {
            if (GameType != GameType.Local)
            {
                if (_connection == null || string.IsNullOrEmpty(_connection?.ConnectionId) || _connection?.State != HubConnectionState.Connected)
                {
                    _connection = new HubConnectionBuilder()
                                    .WithUrl($"{baseUrl}/chatHub")
                                    .Build();

                    _connection.On<string, string, string>("ReceiveMessageFromRoom", (user, message, connectionID) =>
                    {
                        if (ConnectionID == connectionID)
                        {
                            chat.Add($"<b>{user}</b>: {message}<br/>");
                            var maxNumber = 8;
                            if (chat.Count > maxNumber)
                            {
                                chat = chat.TakeLast(maxNumber).ToList();
                            }
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                ChatMessages = string.Join("", chat.ToArray());
                            });
                        }

                    });
                }
                if (GameType == GameType.Host)
                {
                    ConnectionID = Guid.NewGuid().ToString();

                    #if DEBUG
                        ConnectionID = "1234";
                    #endif

                    _connection.On<string>("CheckIfConnectionMatch", async (connectionID) =>
                    {
                        if (ConnectionID == connectionID)
                            await _connection.InvokeCoreAsync("CheckConnectionIDAnswer", args: new[] { connectionID, word, Language.ToString() });
                    });
                }
                else
                {
                    _connection.On<string, string>("ReceiveAlphabetLetter", (connectionID, letter) =>
                    {
                        if (ConnectionID == connectionID)
                        {
                            var button = buttons.FirstOrDefault(b => b.Text == letter);
                            if (button != null && button.IsEnabled)
                            {
                                var action = () => AlphabetBtn_Clicked(button, true);
                                MainThread.BeginInvokeOnMainThread(action);
                            }
                        }
                    });
                    _connection.On<string, string>("ResetGame", (connectionID, word) =>
                    {
                        if (connectionID == ConnectionID)
                        {
                            WordStr = word;
                            var action = () => InitializeGame();
                            MainThread.BeginInvokeOnMainThread(action);
                        }
                    });
                }
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

        public void InitializeGame()
        {
            lives = 7;
            LivesLabel = Terminology.getLives(Language, lives);
            ChangeImage();
            GetWord();
            CreateWord();
            CreateKeyboard();
            if (string.IsNullOrEmpty(word))
            {
                LivesLabel = Terminology.NoWord;
            }
        }

        public void ChangeImage()
        {
            ImageSource = ImageSource.FromFile($"hangman_{lives}_lives.png");
        }

        public void GetWord()
        {
            if (string.IsNullOrEmpty(wordStr) && WordList != null)
            {
                var rnd = new Random();
                word = WordList.Any() ? WordList[rnd.Next(WordList.Count)] : string.Empty;
            }
            else
            {
                word = wordStr;
            }
            WordStr = null;
        }

        public void CreateWord()
        {
            if (wordLabels.Any())
            {
                wordLabels.Clear();
                WordArea.Children.Clear();
            }
            var theme = Application.Current.RequestedTheme;
            for (int i = 0; i < word.Length; i++)
            {
                var label = new Label()
                {
                    StyleId = word[i] + "btn",
                    Text = "_",
                    FontSize = 17,
                    WidthRequest = 20,
                    HeightRequest = 25,
                    Padding = 0,
                    Margin = 0,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = theme == AppTheme.Light ? Colors.Black : Colors.White
                };

                wordLabels.Add(label);
                WordArea.Children.Add(label);
            }
        }

        public void CreateKeyboard()
        {
            if (buttons.Any())
            {
                buttons.ForEach(b => { b.IsEnabled = true; b.Background = Color.FromUint(0xFF512BD4); });
            }
            else
            {
                for (int i = 0; i < Terminology.Alphabet.Count; i++)
                {
                    var button = new Button()
                    {
                        Text = Terminology.Alphabet[i],
                        FontSize = 17,
                        WidthRequest = 35,
                        HeightRequest = 40,
                        Padding = 0,
                        Margin = new Thickness { Right = 5, Bottom = 5 },
                        CornerRadius = 10,
                        Background = Color.FromUint(0xFF512BD4),
                        TextColor = Colors.White
                    };
                    button.Clicked += AlphabetBtn_Clicked;
                    buttons.Add(button);
                    if (i <= 8)
                        FirstRow.Children.Add(button);
                    else if (i > 8 && i <= 17)
                        SecondRow.Children.Add(button);
                    else
                        ThirdRow.Children.Add(button);
                }
            }
        }

        private void AlphabetBtn_Clicked(object sender,EventArgs e)
        {
            if (sender is Button button)
            {
                AlphabetBtn_Clicked(button);
            }
        }

        private void AlphabetBtn_Clicked(Button button, bool canClick = false)
        {
            var qualified = button.IsEnabled && lives > 0 && wordLabels.Any(l => l.Text == "_") && word.Length > 0;
            var guess = button.Text;

            if ((GameType != GameType.Join || canClick) && qualified)
            {
                if (word.Contains(guess))
                {
                    wordLabels.ForEach(l => l.Text = l.StyleId == guess + "btn" ? guess : l.Text);
                    if (!wordLabels.Where(l => l.Text == "_").Any())
                    {
                        LivesLabel = Terminology.Victory;
                    }
                }
                else
                {
                    lives--;
                    LivesLabel = Terminology.getLives(Language, lives);
                    if (lives == 0)
                    {
                        LivesLabel = Terminology.Defeat;
                        for (int i = 0; i < word.Length; i++)
                        {
                            wordLabels[i].Text = word[i].ToString();
                        }
                    }
                    ChangeImage();
                }
                if (GameType == GameType.Host)
                {
                    Task.Run(async () => await _connection.InvokeCoreAsync("SendAlphabetLetter", args: new[] { connectionID, guess }));
                }
                button.Background = Colors.Gray;
                button.IsEnabled = false;
            } 
            else if (GameType == GameType.Join && qualified)
            {
                Task.Run(async () => await _connection.InvokeCoreAsync("SendMessageToRoom", args: new[] { UserName, $"select {guess}", ConnectionID }));
            }
        }
    }
}

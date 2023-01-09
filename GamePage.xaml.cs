namespace HangMan;

using HangMan.Helpers;
using HangMan.ViewModels;
using Microsoft.Maui.ApplicationModel;
using System;

public partial class GamePage : ContentPage
{
    public Terminology Terminology { get; set; }
    public Language Language { get; set; }
    public bool IsChecked { get; set; }
    public string WordStr { get; set; }
    public int Lives { get; set; }
    public string Word { get; set; }
    public List<Label> WordLabels { get; set; }
    public GamePage(GameViewModel vm)
    {
        InitializeComponent();
        WordLabels = new();
        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = (GameViewModel)BindingContext;
        Terminology = vm.Terminology;
        Language = vm.Language;
        IsChecked = vm.IsChecked;
        WordStr = vm.WordStr;
        InitializeGame();
    }

    private void NewGameBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        Lives = 7;
        LivesLabel.Text = Terminology.getLives(Language, Lives);
        ChangeImage();
        GetWord();
        if (string.IsNullOrEmpty(Word))
        {
            LivesLabel.Text = IsChecked ? Terminology.NoWord : Terminology.NoMatch;
        }
        CreateWord();
        CreateKeyboard();
    }

    public void ChangeImage()
    {
        HangManImage.Source = ImageSource.FromFile($"hangman_{Lives}_lives.png");
    }

    public void GetWord()
    {
        if (!IsChecked)
        {
            using var stream = Task.Run(async () => await FileSystem.OpenAppPackageFileAsync(Terminology.File)).Result;
            using var reader = new StreamReader(stream);
            var wordList = reader.ReadToEnd().Split(" ").ToList();
            wordList = !string.IsNullOrEmpty(WordStr) ? wordList.Where(w => w.Contains(WordStr)).ToList() : wordList;
            var rnd = new Random();
            Word = wordList.Any() ? wordList[rnd.Next(wordList.Count)] : string.Empty;
        }
        else
        {
            Word = WordStr;
        }
        
    }

    public void CreateWord()
    {
        var theme = Application.Current.RequestedTheme;
        if (WordArea.Children.Any())
        {
            WordArea.Children.Clear();
            WordLabels.Clear();
        }
        for (int i = 0; i < Word.Length; i++)
        {
            var label = new Label()
            {
                StyleId = Word[i] + "btn",
                Text = "_",
                FontSize = 23,
                Margin = 5,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = theme == AppTheme.Light ? Colors.Black : Colors.White
            };

            WordArea.Children.Add(label);
            WordLabels.Add(label);
        }
    }

    public void CreateKeyboard()
    {
        if (FirstRow.Children.Any() || SecondRow.Children.Any() || ThirdRow.Children.Any())
        {
            FirstRow.Children.Clear();
            SecondRow.Children.Clear();
            ThirdRow.Children.Clear();
        }
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
            if (i <= 8)
                FirstRow.Children.Add(button);
            else if (i > 8 && i <= 17)
                SecondRow.Children.Add(button);
            else
                ThirdRow.Children.Add(button);

        }
    }

    private void AlphabetBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (sender is Button button && button.IsEnabled == true && Lives > 0 && WordLabels.Where(l => l.Text == "_").Any() && Word.Length > 0)
        {
            button.Background = Colors.Gray;
            button.IsEnabled = false;
            var guess = button.Text;
            if (Word.Contains(guess))
            {
                WordLabels.ForEach(l => l.Text = l.StyleId == guess + "btn" ? guess : l.Text);
                if (!WordLabels.Where(l => l.Text == "_").Any())
                {
                    LivesLabel.Text = Terminology.Victory;
                }
            }
            else
            {
                Lives--;
                LivesLabel.Text = Terminology.getLives(Language, Lives);
                if (Lives == 0)
                {
                    LivesLabel.Text = Terminology.Defeat;
                    for (int i = 0; i < Word.Length; i++)
                    {
                        WordLabels[i].Text = Word[i].ToString();
                    }
                }
                ChangeImage();
            }
        }
    }
}
using HangMan.Helpers;

namespace HangMan
{
    public class Terminology
    {
        public string Welcome { get; set; }
        public string Title { get; set; }
        public List<string> Alphabet { get; set; }
        public string File { get; set; }
        public string Victory { get; set; }
        public string Defeat { get; set; }
        public string Start { get; set; }
        public string English { get; set; }
        public string Greek { get; set; }
        public string LanguageLabel { get; set; }
        public string NoWord { get; set; }
        public string StrLabel { get; set; }
        public string NewGame { get; set; }
        public string UserNameLabel { get; set; }
        public string Create { get; set; }
        public string Join { get; set; }
        public string ConnectionLabel { get; set; }
        public string CreateLabel { get; set; }
        public string Or { get; set; }

        public Terminology(Language language)
        {
            if (language == Language.Greek)
            {
                Welcome = "Καλώς ήρθατε στην εφαρμογή μου";
                Title = "Κρεμάλα";
                Alphabet = new List<string>
                { 
                    "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι",
                    "Κ", "Λ", "Μ", "Ν", "Ξ", "Ο", "Π", "Ρ", "Σ",
                    "Τ", "Υ", "Φ", "Χ", "Ψ", "Ω" 
                };
                File = "GreekWords.txt";
                Victory = "Συγχαρητήρια, νίκησες!";
                Defeat = "Τέλος Παιχνιδιού";
                Start = "Έναρξη παιχνιδιού";
                English = "English";
                Greek = "Ελληνικά";
                LanguageLabel = "Επιλέξτε γλώσσα";
                NoWord = "Δεν δόθηκε λέξη";
                StrLabel = "Πληκτρολογίστε την λέξη σας";
                NewGame = "Νέο Παιχνίδι";
                UserNameLabel = "Συμπληρώστε παρακάτω το όνομά σας";
                Create = "Δημιουργία";
                Join = "Συμμετοχή";
                ConnectionLabel = "Βάλε κωδικό για συμμετοχή σε παιχνίδι";
                CreateLabel = "Πατήστε εδώ για δημιουργία νέου παιχνιδιού";
                Or = "Ή";
            }
            else
            {
                Welcome = "Welcome to my application";
                Title = "HangMan";
                Alphabet = new List<string>
                { 
                    "A", "B", "C", "D", "E", "F", "G", "H", "I",
                    "J", "K", "L", "M", "N", "O", "P", "Q", "R", 
                    "S", "T", "U", "V", "W", "X", "Y", "Z" 
                };
                File = "EnglishWords.txt";
                Victory = "Congratulations, You Won!";
                Defeat = "Game Over";
                Start = "Start Game";
                English = "English";
                Greek = "Ελληνικά";
                LanguageLabel = "Select a language";
                NoWord = "No word was given";
                StrLabel = "Enter your word";
                NewGame = "New Game";
                UserNameLabel = "Enter your name below";
                Create = "Create";
                Join = "Join";
                ConnectionLabel = "Fill this with an ID to join a room";
                CreateLabel = "Click here to Create a room";
                Or = "OR";
            }
        }

        public string getLives(Language language, int lives)
        {
            if (language == Language.Greek)
            {
                return $"Απομένουν {lives} ζωές";
            }
                return $"You have {lives} lives left";
        }
    }
}


namespace HangMan.Helpers
{
    public static class StreamReaderHelper
    {
        public static List<string> GetWordList(Terminology terminology)
        {
            using var stream = Task.Run(async () => await FileSystem.OpenAppPackageFileAsync(terminology.File)).Result;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd().Split(" ").ToList();
        }
    }
}

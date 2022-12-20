namespace Engine;

public class Tokenizer
{
    public static List<Sentence> Tokenize(string input)
    {
        var sentences = input.Split(new[] { '.', '?', '!', ';' });
        return sentences.Select(x => new Sentence()
        {
            Words = x.Split(' ').Select(x => new Word() { Content = x }).ToList()
        }).ToList();
    }

    public static string[] TokenizeStrings(string input)
    {
        return input.Split(new[] { ' ', '?', '!', ';' });
    }
}
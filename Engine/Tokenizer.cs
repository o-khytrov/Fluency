namespace Engine;

public class Tokenizer
{
    public List<string> Tokenize(string input)
    {
        return input.Split(' ').ToList();
    }
}

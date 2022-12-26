namespace Engine;

public class Sentence
{
    public SentenceType Type { get; set; }
    public List<Word> Words { get; set; }
}

public class Word
{
    public string Content { get; set; }
}
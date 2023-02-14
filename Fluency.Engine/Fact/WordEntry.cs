namespace Fluency.Engine.Fact;

public class WordEntry
{
    public string Word { get; }

    public List<uint> Meanings { get; set; } = new();

    private HashSet<string> Concepts = new();

    public WordEntry(string word)
    {
        Word = word;
    }

    public void AddConcept(string concept)
    {
        Concepts.Add(concept);
    }
}

public class Fact
{
    public uint Subject { get; set; }

    public uint Verb { get; set; }

    public uint Object { get; set; }

    public uint SubjectHead { get; set; }

    public uint SubjectNext { get; set; }

    public uint VerbHead { get; set; }

    public uint VerbNext { get; set; }

    public uint ObjectHead { get; set; }

    public uint ObjectNext { get; set; }
}

public class Dictionary
{
    public Dictionary<uint, WordEntry> WordEntries { get; set; }

    public WordEntry Meaning2Word(uint meaning)
    {
        return WordEntries[meaning];
    }
}

public class FactRepository
{
    public List<Fact> Facts { get; set; } = new();

    private Dictionary _dictionary;

    public FactRepository(Dictionary dictionary)
    {
        _dictionary = dictionary;
    }

    public void VerifyField(Fact fact, uint field)
    {
    }
}
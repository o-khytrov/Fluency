using System.Diagnostics;
using Catalyst;
using Mosaik.Core;

namespace FluentConversation.Engine;

public class Tokenizer
{
    private readonly Pipeline _nlp;

    public Tokenizer()
    {
        Catalyst.Models.English.Register(); //You need to pre-register each language (and install the respective NuGet Packages)

        _nlp = Pipeline.For(Language.English);
    }

    public string[] TokenizeStrings(string input)
    {
        return input.Split(new[] { ' ', '?', '!', ';' });
    }

    public Document Tokenize(string text)
    {
        var doc = new Document(text, Language.English);
        _nlp.ProcessSingle(doc);
        return doc;
    }
}
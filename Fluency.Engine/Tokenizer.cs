using Catalyst;
using Mosaik.Core;

namespace FluentConversation.Engine;

public class Tokenizer
{
    private readonly Pipeline _nlp;

    public Tokenizer(Pipeline pipeline)
    {
        _nlp = pipeline;
    }

    public Document Tokenize(string text)
    {
        var doc = new Document(text, Language.English);
        _nlp.ProcessSingle(doc);
        return doc;
    }
}
using Catalyst;
using Mosaik.Core;

namespace Fluency.Engine;

public class Tokenizer
{
    private readonly Pipeline _nlp;

    public Tokenizer(Pipeline pipeline)
    {
        _nlp = pipeline;
    }

    public Document Tokenize(string text, Language language = Language.English)
    {
        var doc = new Document(text, language);
        _nlp.ProcessSingle(doc);
        return doc;
    }
}
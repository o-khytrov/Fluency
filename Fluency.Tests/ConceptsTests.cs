using System;
using System.IO;
using System.Threading.Tasks;
using Fluency.Engine.Fact;
using Xunit;

namespace ConversationDesigner.Tests;

public class ConceptsTests
{
    [Theory]
    [InlineData("file")]
    public async Task ConceptParserTests(string file)
    {
        var parser = new ConceptParser();
        var home = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        file = Path.Combine(home, "src/ja-chatscript/Pearl/RAWDATA/ONTOLOGY/ENGLISH/concepts.top");
        var concepts = await parser.Parse(file);
        Assert.NotNull(concepts);
        Assert.NotEmpty(concepts);

    }
}

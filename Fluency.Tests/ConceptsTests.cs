using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fluency.Engine.Fact;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ConversationDesigner.Tests;

public class ConceptsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ConceptsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ConceptParserTests()
    {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var conceptDirectory = Path.Combine(homeDirectory, "src/ja-chatscript/Pearl/RAWDATA/ONTOLOGY/ENGLISH");
        var parser = new ConceptParser();
        var concepts = new Dictionary<string, HashSet<string>>();
        var undefinedConcepts = new HashSet<string>();
        foreach (var file in Directory.EnumerateFiles(conceptDirectory,"*.top"))
        {
            var filePath = Path.Combine(homeDirectory, file);
            await parser.Parse(filePath, concepts);
        }
        var substitutions=
            Path.Combine(homeDirectory, "src/ja-chatscript/Pearl/LIVEDATA/ENGLISH/SUBSTITUTES/interjections.txt");
        await parser.ParseSubstitutions(substitutions, concepts);

        var json = JsonConvert.SerializeObject(concepts, new JsonSerializerSettings
            { Formatting = Formatting.Indented });
        await File.WriteAllTextAsync(Path.Combine(homeDirectory, "Downloads", "concepts.json"), json);
        foreach (var concept in concepts)
        {
            foreach (var member in concept.Value)
            {
                if (member.StartsWith("~") && !concepts.ContainsKey(member))
                {
                    undefinedConcepts.Add(member);
                }
            }
        }

        foreach (var undefinedConcept in undefinedConcepts)
        {
            _testOutputHelper.WriteLine(undefinedConcept);
        }

        Assert.NotNull(concepts);
        Assert.NotEmpty(concepts);
    }
}
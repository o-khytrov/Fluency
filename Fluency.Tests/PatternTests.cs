using System.Collections.Generic;
using System.Threading.Tasks;
using Catalyst;
using Fluency.Engine;
using Fluency.Engine.Fact;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.Tokenization;
using Microsoft.ML.Transforms.Text;
using Mosaik.Core;
using Xunit;

namespace ConversationDesigner.Tests;

public class PatternTests
{
    private readonly Tokenizer _tokenizer;

    public PatternTests()
    {
        Catalyst.Models.English.Register();
        _tokenizer = new Tokenizer(Pipeline.For(Language.English));
    }

    [Theory]
    [InlineData("I am at Sumy", "Sumy")]
    [InlineData("Today We are in Sumy", "Sumy")]
    public void WordPatternElementTest(string input, string extractedVariable)
    {
        var pattern = new PatternBuilder().Word("I", "we").Word("am", "are").Word("at", "in").Wildcard().Build();

        var patternEngine = new PatternEngine(_tokenizer);
        var result = patternEngine.Match(pattern, new BotInput(input) { ProcessedInput = _tokenizer.Tokenize(input) });
        Assert.True(result.Match);
        Assert.Equal(extractedVariable, result[0]);
    }

    [Theory]
    [InlineData("What nature of life?")]
    [InlineData("nature of life what?")]
    [InlineData("what life of nature")]
    [InlineData("of life of nature")]
    public void PhrasePatternElementTest(string input)
    {
        var pattern = new PatternBuilder().Phrase("what nature of life", ignoreOrder: true).Build();

        var patternEngine = new PatternEngine(_tokenizer);
        var result = patternEngine.Match(pattern, new BotInput(input) { ProcessedInput = _tokenizer.Tokenize(input) });
        Assert.True(result.Match);
    }
}
using System.Collections.Generic;
using Catalyst;
using Fluency.Engine;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.Tokenization;
using Microsoft.ML.Transforms.Text;
using Mosaik.Core;
using Xunit;

namespace ConversationDesigner.Tests;

public class TokenizerTests
{
    [Theory]
    [InlineData("I am at Sumy", "Sumy")]
    [InlineData("Today We are in Sumy", "Sumy")]
    public void PatterBuilderTest(string input, string extractedVariable)
    {
        var patternBuilder = new PatternBuilder();
        patternBuilder.Word("I", "we").Word("am", "are").Word("at", "in").Wildcard();
        var patternEngine = new PatternEngine(new Tokenizer(Pipeline.For(Language.English)));
        var result = patternEngine.Match(patternBuilder.Build(), new BotInput(input));
        Assert.True(result.Match);

        Assert.Equal(extractedVariable, result[0]);
    }
}
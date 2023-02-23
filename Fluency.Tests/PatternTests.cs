using System;
using Catalyst;
using Fluency.Engine;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.PatternSystem.Elements;
using Fluency.Engine.Tokenization;
using Mosaik.Core;
using Xunit;
using Xunit.Abstractions;

namespace ConversationDesigner.Tests;

public class PatternTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Tokenizer _englishTokenizer;
    private readonly Pipeline _germanTokenizer;

    public PatternTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        Catalyst.Models.English.Register();
        Catalyst.Models.German.Register();
        _englishTokenizer = new Tokenizer(Pipeline.For(Language.English));
        _germanTokenizer = Pipeline.For(Language.German);
    }

    [Theory]
    [InlineData("I am at Sumy", "Sumy")]
    [InlineData("Today We are in Sumy", "Sumy")]
    public void WordPatternElementTest(string input, string extractedVariable)
    {
        var pattern = new PatternBuilder<ChatContext>()
            .Word("I", "we").Word("am", "are").Word("at", "in").Wildcard()
            .Build();

        var patternEngine = new PatternEngine(_englishTokenizer);
        var conversation = new Conversation<ChatContext>();
        conversation.CurrentInput = new BotInput(input) { ProcessedInput = _englishTokenizer.Tokenize(input) };
        var result = patternEngine.Match(pattern, conversation);
        Assert.True(result.Match);
        Assert.Equal(extractedVariable, result[0]);
    }

    [Theory]
    [InlineData("What nature of life?", true, true)]
    [InlineData("nature of life what?", true, true)]
    [InlineData("what life of nature", true, true)]
    [InlineData("of life of nature", true, true)]
    public void PhrasePatternElementTest(string input, bool respectOrder, bool match)
    {
        var pattern = new PatternBuilder<ChatContext>()
            .Phrase("what nature of life", ignoreOrder: respectOrder)
            .Build();

        var patternEngine = new PatternEngine(_englishTokenizer);

        var conversation = new Conversation<ChatContext>();
        conversation.CurrentInput = new BotInput(input) { ProcessedInput = _englishTokenizer.Tokenize(input) };
        var result = patternEngine.Match(pattern, conversation);
        Assert.Equal(match, result.Match);
    }

    [Theory]
    [InlineData("I like apples", true)]
    [InlineData("She likes milk", true)]
    [InlineData("We want tea", true)]
    public void PronounPatternElementTest(string input, bool match)
    {
        var pattern = new PatternBuilder<ChatContext>()
            .Pos(PartOfSpeech.PRON).Pos(PartOfSpeech.VERB).Pos(PartOfSpeech.NOUN)
            .Build();

        var patternEngine = new PatternEngine(_englishTokenizer);

        var conversation = new Conversation<ChatContext>();
        conversation.CurrentInput = new BotInput(input) { ProcessedInput = _englishTokenizer.Tokenize(input) };
        var result = patternEngine.Match(pattern, conversation);
        Assert.Equal(match, result.Match);
    }

    [Theory]
    [InlineData("My dog ate chocolate", true)]
    [InlineData("Her cat ate chocolate", true)]
    [InlineData("Our cat ate chocolate", true)]
    [InlineData("Neighbour's dog ate my cat", false)]
    public void PossessivePronounPatternElementTest(string input, bool match)
    {
        var pattern = new PatternBuilder<ChatContext>()
            .Pos(PartOfSpeech.PRON).Noun().Pos(PartOfSpeech.VERB).Pos(PartOfSpeech.NOUN)
            .Build();

        var result = Match(pattern, input);

        Assert.Equal(match, result.Match);
    }

    private PatternMatchingResult Match(Pattern<ChatContext> pattern, string input)
    {
        var patternEngine = new PatternEngine(_englishTokenizer);
        var conversation = new Conversation<ChatContext>
        {
            CurrentInput = new BotInput(input) { ProcessedInput = _englishTokenizer.Tokenize(input) }
        };
        var result = patternEngine.Match(pattern, conversation);
        return result;
    }
}
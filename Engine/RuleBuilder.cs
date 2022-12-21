using System.Text.RegularExpressions;
using Engine.PatternSystem;
using Engine.Tokenization;

namespace Engine;

public class RuleBuilder
{
    private static readonly Func<BotInput, bool> DefaultCondition = x => true;
    private readonly BotRule _botRule;
    private static readonly PatternEngine PatternEngine = new PatternEngine();

    public RuleBuilder(BotRule botRule)
    {
        _botRule = botRule;
    }

    public RuleBuilder Keep(bool keep = true)
    {
        _botRule.Keep = keep;
        return this;
    }

    public RuleBuilder Repeat(bool repeat = true)
    {
        _botRule.Repeat = repeat;
        return this;
    }

    public RuleBuilder WithResponder(string output)
    {
        return this;
    }

    public RuleBuilder WithOutput(string template)
    {
        _botRule.Output = () => template;
        return this;
    }

    public RuleBuilder WithRegexPattern(string pattern)
    {
        _botRule.Conditions.Add((input) =>
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input.RawInput);
            return matches.Count > 0;
        });
        return this;
    }

    public RuleBuilder WithPattern(Action<PatternBuilder> patternBuilderAction)
    {
        var builder = new PatternBuilder();
        patternBuilderAction.Invoke(builder);
        _botRule.Pattern = builder.Build();
        _botRule.Conditions.Add(x => PatternEngine.Match(_botRule.Pattern, x).Match);
        return this;
    }

    public RuleBuilder WithRegexPattern(string pattern, Action<MatchCollection> action)
    {
        _botRule.Conditions.Add((input) =>
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input.RawInput);
            var isMatch = matches.Count > 0;
            if (isMatch)
            {
                action(matches);
            }

            return isMatch;
        });
        return this;
    }

    public RuleBuilder When(Func<BotInput, bool> condition)
    {
        _botRule.Conditions.Add(condition);
        return this;
    }

    public RuleBuilder WithOutput(Func<string> outputRenderer)
    {
        _botRule.Output = outputRenderer;
        return this;
    }

    public RuleBuilder WithTest(string input, string expectedResponse)
    {
        _botRule.Tests.Add(new RuleTest() { Input = input, ExpectedResponse = expectedResponse });
        return this;
    }
}
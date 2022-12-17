using System.Text.RegularExpressions;

namespace ConversationDesigner;

public class RuleBuilder
{
    private static readonly Func<string, bool> DefaultCondition = x => true;
    private readonly BotRule _botRule;

    public RuleBuilder(BotRule botRule)
    {
        _botRule = botRule;
        _botRule.Condition = DefaultCondition;
    }

    public RuleBuilder WithName(string name)
    {
        _botRule.Name = name;
        return this;
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

    public RuleBuilder WithPattern(string pattern)
    {
        _botRule.Condition = (input) =>
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input);
            return matches.Count > 0;
        };
        return this;
    }

    public RuleBuilder WithPattern(string pattern, Action<MatchCollection> action)
    {
        _botRule.Condition = (input) =>
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input);
            var isMatch = matches.Count > 0;
            if (isMatch)
            {
                action(matches);
            }

            return isMatch;
        };
        return this;
    }

    public RuleBuilder When(Func<string, bool> condition)
    {
        this._botRule.Condition = condition;
        return this;
    }

    public RuleBuilder WithOutput(Func<string> outputRenderer)
    {
        _botRule.Output = outputRenderer;
        return this;
    }
}
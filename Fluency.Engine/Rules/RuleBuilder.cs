using System.Text.RegularExpressions;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.PatternSystem.Elements;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.Rules;

public class RuleBuilder<T>
    : IRuleBuilderInitialStage<T>,
        IRuleBuilderFinalStage<T>,
        IRuleBuilderOutputStage<T>,
        IIntermediateStageBuilder<T>
{
    private static readonly Func<BotInput, bool> DefaultCondition = x => true;
    private readonly BotRule<T> _botRule;
    private readonly Topic<T> _topic;


    public RuleBuilder(BotRule<T> botRule, Topic<T> parent)
    {
        _botRule = botRule;
        _topic = parent;
    }

    public IRuleBuilderFinalStage<T> Output(string output)
    {
        _botRule.RenderOutput = (x) => output;
        return this;
    }

    public RuleBuilder<T> WithRegexPattern(string pattern)
    {
        _botRule.Conditions.Add((c, input) =>
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input.RawInput);
            return matches.Count > 0;
        });
        return this;
    }

    public IRuleBuilderOutputStage<T> Pattern(Action<PatternBuilder> patternBuilderAction)
    {
        var builder = new PatternBuilder();
        patternBuilderAction.Invoke(builder);
        _botRule.Pattern = builder.Build();
        return this;
    }

    public IRuleBuilderOutputStage<T> Always()
    {
        return this;
    }

    public RuleBuilder<T> WithRegexPattern(string pattern, Action<MatchCollection> action)
    {
        _botRule.Conditions.Add((context, input) =>
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


    public IIntermediateStageBuilder<T> When(Func<T, BotInput, bool> condition)
    {
        _botRule.Conditions.Add(condition);
        return this;
    }

    public IIntermediateStageBuilder<T> Do(Action<BotInput, T> action)
    {
        _botRule.PreActions.Add(action);
        return this;
    }

    public IRuleBuilderOutputStage<T> Then(Action<T, PatternMatchingResult> postAction)
    {
        _botRule.PostActions.Add(postAction);
        return this;
    }

    public RuleBuilder<T> Output(Func<string> outputRenderer)
    {
        return this;
    }

    public IRuleBuilderFinalStage<T> Output(Func<T, string> outputRenderer)
    {
        _botRule.RenderOutput = outputRenderer;
        return this;
    }

    public RuleBuilder<T> WithTest(string input, string expectedResponse)
    {
        _botRule.Tests.Add(new RuleTest() { Input = input, ExpectedResponse = expectedResponse });
        return this;
    }

    public void NexTopic(string topicName)
    {
        _botRule.NexTopic = topicName;
    }

    public void Rejoinder(Action action)
    {
        var dependencyContainer = new List<BotRule<T>>();
        // Capture any rules added to the parent validator inside this delegate.
        using (_topic.BotRules.Capture(dependencyContainer.Add))
        {
            action();
        }

        _botRule.Rejoinders.AddRange(dependencyContainer);
    }
}

public interface IRuleBuilderInitialStage<T>
{
    /// <summary>
    /// Defines an action which is always executed before evaluating precondition and pattern
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    IIntermediateStageBuilder<T> Do(Action<BotInput, T> action);

    /// <summary>
    /// Sets a precondition under which the rule will be executed is checked before the pattern is evaluated
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    IIntermediateStageBuilder<T> When(Func<T, BotInput, bool> condition);

    /// <summary>
    /// Builds a pipeline for pattern scanner
    /// </summary>
    /// <param name="patternBuilderAction"></param>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Pattern(Action<PatternBuilder> patternBuilderAction);

    /// <summary>
    /// Skips all conditions
    /// </summary>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Always();
}

public interface IRuleBuilderFinalStage<T>
{
    /// <summary>
    /// Add nested rules aka 'Rejoinders'. They are used to anticipate how a user might respond to output and give direct feedback based on that response
    /// </summary>
    /// <param name="action"></param>
    public void Rejoinder(Action action);

    public void NexTopic(string topicName);
}

public interface IRuleBuilderOutputStage<T>
{
    /// <summary>
    /// Set post action, which will be executed after if pattern matches
    /// </summary>
    /// <param name="postAction"></param>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Then(Action<T, PatternMatchingResult> postAction);

    /// <summary>
    /// Sets static output
    /// </summary>
    /// <param name="output"></param>
    /// <returns></returns>
    IRuleBuilderFinalStage<T> Output(string output);

    /// <summary>
    /// Sets function for rendering output
    /// </summary>
    /// <param name="outputRenderer"></param>
    /// <returns></returns>
    IRuleBuilderFinalStage<T> Output(Func<T, string> outputRenderer);
}

public interface IIntermediateStageBuilder<T>
{
    /// <summary>
    /// Builds a pipeline for pattern scanner
    /// </summary>
    /// <param name="patternBuilderAction"></param>
    /// <returns></returns>
    IRuleBuilderOutputStage<T> Pattern(Action<PatternBuilder> patternBuilderAction);

    IRuleBuilderFinalStage<T> Output(string output);

    IRuleBuilderFinalStage<T> Output(Func<T, string> outputRenderer);
}
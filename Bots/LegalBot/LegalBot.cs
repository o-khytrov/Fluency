using FluentConversation.Engine;
using FluentConversation.Engine.Tokenization;
using Mosaik.Core;

namespace LegalBot;

public class LegalBot : Bot<LegalConversationContext>
{
    private static readonly Dictionary<string, string> CategoryTopicMap = new()
    {
        { "traffic-law", "traffic law" }
    };

    public LegalBot()
    {
        R("LEGAL_START")
            .When(LegalStartCondition).Output((c) => $"Welcome! How can I help with your {c.Topic} question?").Keep();
    }

    private bool LegalStartCondition(LegalConversationContext conversation, BotInput botInput)
    {
        if (conversation.Category is not null && CategoryTopicMap.ContainsKey(conversation.Category))
        {
            conversation.Topic = CategoryTopicMap[conversation.Category];
        }

        return true;
    }

    public override Language Language => Language.English;

    public override string Name => "Lawyer";
}
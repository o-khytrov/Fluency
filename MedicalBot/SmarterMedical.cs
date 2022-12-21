using Engine;

namespace MedicalBot;

public class SmarterMedical : GenericBot<MedicalBotContext>
{
    public string Sympathy { get; set; } = string.Empty;

    public string QuestionType { get; set; }

    public SmarterMedical()
    {
        Gambit("MEDICAL_START")
            .WithOutput($"Welcome! How can I help you with your {QuestionType} question?");
    }
}
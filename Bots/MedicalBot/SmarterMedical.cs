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

        Gambit("COST_REJOINDER")
            .WithPattern(x =>
            {
                x.Disjunction(d =>
                {
                    d.Add().Word("is", "are", "will").Word("this", "you").Word("free", "paid");
                    d.Add().Word("how", "are", "will").Word("this", "you").Word("free", "paid");
                });
            }).WithOutput("You pay 5$");
    }
}
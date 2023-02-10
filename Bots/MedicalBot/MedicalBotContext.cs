using Fluency.Engine;

namespace MedicalBot;

public class MedicalBotContext : ChatContext
{
    public string Speciality { get; set; } = string.Empty;

    public string Expert { get; set; } = string.Empty;

    public string? Theme { get; set; }

    public bool Variability { get; set; }
}

public static class Themes
{
    public const string Rash = "rash";
}
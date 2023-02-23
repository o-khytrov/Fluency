namespace Fluency.Engine.Fact;

public class Concept : List<string>
{
}

public class ConceptParser
{
    public async Task ParseSubstitutions(string file, Dictionary<string, HashSet<string>> dictionary)
    {
        var lines = await File.ReadAllLinesAsync(file);
        foreach (var line in lines)
        {
            var trimmedLine= line.Trim();
            if (trimmedLine.StartsWith("#"))
            {
                continue;
            } 
            var parts = trimmedLine.Split(' ');
            var conceptName = parts[1].ToLower();
            var member = parts[0];

            if (dictionary.ContainsKey(conceptName))
            {
                dictionary[conceptName].Add(member.Trim());
            }
            else
            {
                dictionary.Add(conceptName, new HashSet<string>() { member });
            }
        }
    }

    public async Task Parse(string file, Dictionary<string, HashSet<string>> dictionary)
    {
        var lines = await File.ReadAllLinesAsync(file);
        var currentConcept = string.Empty;
        foreach (var part in lines)
        {
            var isFirstLine = false;
            var line = part.Replace("##<<ENGLISH ", "").Replace("##>>", "").Trim();
            if (line.StartsWith("concept: "))
            {
                line = line.Replace("concept: ", string.Empty);
                isFirstLine = true;
            }

            line = line.Replace("(", string.Empty).Replace(")", string.Empty);

            var result = line.Split('"')
                .Select((element, index) => index % 2 == 0 // If even index
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) // Split the item
                    : new string[] { element }) // Keep the entire item
                .SelectMany(element => element).ToList();
            if (isFirstLine)
            {
                currentConcept = result.First().ToLower();
                dictionary.Add(currentConcept, result.Skip(1).Select(PrepareMember).ToHashSet());
            }
            else
            {
                foreach (var member in result)
                {
                    var conceptMember = PrepareMember(member);

                    dictionary[currentConcept].Add(conceptMember);
                }
            }
        }
    }

    private static string PrepareMember(string member)
    {
        var conceptMember = member;
        if (conceptMember.StartsWith("~"))
        {
            conceptMember = conceptMember.ToLower();
        }

        return conceptMember;
    }

    public void AddChildConcepts(string parentConceptName, string childConcept)
    {
    }
}
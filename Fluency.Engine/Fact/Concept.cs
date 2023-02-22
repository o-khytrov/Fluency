namespace Fluency.Engine.Fact;

public class Concept : List<string>
{
}

public class ConceptParser
{
    public async Task<Dictionary<string, WordEntry>> Parse(string file)
    {
        var dictionary = new Dictionary<string, WordEntry>();
        var lines = await File.ReadAllTextAsync(file);
        var conceptLines = lines.Split("concept:")
            .Select(x => x.Trim().Replace("##<<ENGLISH ","").Replace("##>>","").Split(new[] { '(', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
        foreach (var conceptLine in conceptLines)
        {
            if (conceptLine.Length == 0)
            {
                continue;
            }

            var conceptName = conceptLine[0].Replace("~", "");
            for (int i = 1; i < conceptLine.Length; i++)
            {
                var anotherConcept = false;
                var member = conceptLine[1];
                if (member.StartsWith("~"))
                {
                    AddChildConcepts(conceptName, member);
                    continue;
                }

                if (!dictionary.ContainsKey(member))
                {
                    dictionary.Add(member, new WordEntry(member));
                }

                dictionary[member].AddConcept(conceptName);
            }
        }

        return dictionary;
    }

    public void AddChildConcepts(string parentConceptName, string childConcept)
    {
        
    }
}
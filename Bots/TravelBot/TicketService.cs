namespace TravelBot;

public class TicketService
{
    private Dictionary<string, int> Routes = new Dictionary<string, int>()
    {
        { "Sumy:Kyiv", 100 }
    };

    public static string[] Cities = { "Sumy", "Poltava", "Kharkiv", "Kyiv" };

    public int GetTicketPrice(string source, string destination)
    {
        return 1;
    }
}
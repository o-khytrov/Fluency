namespace Fluency.Testing;

public class TestConversation : List<TestVolley>
{
    public string UserId { get; }

    public TestConversation()
    {
        UserId = Guid.NewGuid().ToString();
    }
}
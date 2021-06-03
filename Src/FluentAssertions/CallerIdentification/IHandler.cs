namespace FluentAssertions.CallerIdentification
{
    public interface IHandler
    {
        HandlerResult Handle(char symbol);
    }
}

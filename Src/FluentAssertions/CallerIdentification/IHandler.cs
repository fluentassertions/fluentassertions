namespace FluentAssertions.CallerIdentification
{
    internal interface IHandler
    {
        HandlerResult Handle(char symbol);
    }
}

namespace FluentAssertions.Types
{
    public static class AnAttribute
    {
        public static AttributeConstraints<TAttribute> OfType<TAttribute>()
        {
            return new AttributeConstraints<TAttribute>();
        }
    }
}
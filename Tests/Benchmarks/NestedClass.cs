namespace Benchmarks;

public sealed class Nested
{
    public int A { get; set; }

    public Nested B { get; set; }

    public Nested C { get; set; }

    public static Nested Create(int i, ref int objectCount)
    {
        if (i < 0)
        {
            return null;
        }

        if (i == 0)
        {
            return new Nested();
        }

        return new Nested
        {
            A = ++objectCount,
            B = Create(i - 1, ref objectCount),
            C = Create(i - 2, ref objectCount),
        };
    }
}

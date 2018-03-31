namespace Benchmarks
{
    public sealed class Nested
    {
        public int A { get; set; }

        public Nested B { get; set; }

        public static Nested Create(int i)
        {
            if (i == 0)
            {
                return new Nested();
            }

            return new Nested
            {
                A = i,
                B = Create(i - 1)
            };
        }
    }

}

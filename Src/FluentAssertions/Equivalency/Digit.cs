using System.Collections.Generic;

namespace FluentAssertionsAsync.Equivalency;

internal class Digit
{
    private readonly int length;
    private readonly Digit nextDigit;
    private int index;

    public Digit(int length, Digit nextDigit)
    {
        this.length = length;
        this.nextDigit = nextDigit;
    }

    public int[] GetIndices()
    {
        var indices = new List<int>();

        Digit digit = this;

        while (digit is not null)
        {
            indices.Add(digit.index);
            digit = digit.nextDigit;
        }

        return indices.ToArray();
    }

    public bool Increment()
    {
        bool success = nextDigit?.Increment() == true;

        if (!success)
        {
            if (index < (length - 1))
            {
                index++;
                success = true;
            }
            else
            {
                index = 0;
            }
        }

        return success;
    }
}

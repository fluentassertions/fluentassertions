using System.Collections.Generic;

namespace FluentAssertions.Equivalency;

internal class Digit
{
    private readonly int length;
    private readonly Digit next;
    private int index;

    public Digit(int length, Digit next)
    {
        this.length = length;
        this.next = next;
    }

    public int[] GetIndices()
    {
        var indices = new List<int>();

        Digit digit = this;

        while (digit is not null)
        {
            indices.Add(digit.index);
            digit = digit.next;
        }

        return indices.ToArray();
    }

    public bool Increment()
    {
        bool success = next?.Increment() == true;

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

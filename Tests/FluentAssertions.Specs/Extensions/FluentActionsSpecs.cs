using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static FluentAssertionsAsync.FluentActions;

namespace FluentAssertionsAsync.Specs.Extensions;

public class FluentActionsSpecs
{
    [Fact]
    public void Invoking_works_with_action()
    {
        // Arrange / Act / Assert
        Invoking(() => Throws()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Invoking_works_with_func()
    {
        // Arrange / Act / Assert
        Invoking(() => Throws(0)).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task Awaiting_works_with_action()
    {
        // Arrange / Act / Assert
        await Awaiting(() => ThrowsAsync()).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Awaiting_works_with_func()
    {
        // Arrange / Act / Assert
        await Awaiting(() => ThrowsAsync(0)).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public void Enumerating_works_with_general()
    {
        // Arrange / Act / Assert
        Enumerating(() => ThrowsAfterFirst()).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Enumerating_works_with_specialized()
    {
        // Arrange / Act / Assert
        Enumerating(() => ThrowsAfterFirst(0)).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Enumerating_works_with_enumerable_func()
    {
        // Arrange
        var actual = new Example();

        // Act / Assert
        actual.Enumerating(x => x.DoSomething()).Should().Throw<InvalidOperationException>();
    }

    private class Example
    {
        public IEnumerable<int> DoSomething()
        {
            var range = Enumerable.Range(0, 10);

            return range.Select(Twice);
        }

        private int Twice(int arg)
        {
            if (arg == 5)
            {
                throw new InvalidOperationException();
            }

            return 2 * arg;
        }
    }

    private static void Throws()
    {
        throw new InvalidOperationException();
    }

    private static int Throws(int _)
    {
        throw new InvalidOperationException();
    }

    private static async Task ThrowsAsync()
    {
        await Task.Yield();
        throw new InvalidOperationException();
    }

    private static async Task<int> ThrowsAsync(int _)
    {
        await Task.Yield();
        throw new InvalidOperationException();
    }

    private static IEnumerable ThrowsAfterFirst()
    {
        yield return 0;
        throw new InvalidOperationException();
    }

    private static IEnumerable<int> ThrowsAfterFirst(int number)
    {
        yield return number;
        throw new InvalidOperationException();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static FluentAssertions.FluentActions;

namespace FluentAssertions.Specs
{
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
        public void Awaiting_works_with_action()
        {
            // Arrange / Act / Assert
            Awaiting(() => ThrowsAsync()).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Awaiting_works_with_func()
        {
            // Arrange / Act / Assert
            Awaiting(() => ThrowsAsync(0)).Should().Throw<InvalidOperationException>();
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

        private static void Throws()
        {
            throw new InvalidOperationException();
        }

        private static int Throws(int number)
        {
            throw new InvalidOperationException();
        }

        private static async Task ThrowsAsync()
        {
            await Task.Yield();
            throw new InvalidOperationException();
        }

        private static async Task<int> ThrowsAsync(int number)
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
}

using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace FluentAssertions.Specs
{
    public class DictionarySpecs
    {
        [Fact]
        public void When_adapting_to_readonly_interface_has_same_count()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            var readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            readonlyDict.Count.Should().Be(1);
        }

        [Fact]
        public void When_adapting_to_readonly_contains_key_should_be_consistent_with_original_dict()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            var readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            readonlyDict.ContainsKey(0).Should().BeTrue();
            readonlyDict.ContainsKey(1).Should().BeFalse();
        }

        [Fact]
        public void When_adapting_to_readonly_try_get_value_should_be_consistent_with_original_dict()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            var readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            var result1 = readonlyDict.TryGetValue(0, out int v1);
            result1.Should().BeTrue();
            v1.Should().Be(1);

            var result2 = readonlyDict.TryGetValue(1, out int v2);
            result2.Should().BeFalse();
            
        }

        [Fact]
        public void When_adapting_to_readonly_indexer_should_be_consistent_with_original_dict()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            var readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            readonlyDict[0].Should().Be(1);
        }

        [Fact]
        public void When_adapting_to_readonly_keys_should_be_consistent_with_original_dict()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            var readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            readonlyDict.Keys.Should().Equal(new int[] { 0 });
        }

        [Fact]
        public void When_adapting_to_readonly_values_should_be_consistent_with_original_dict()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            var readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            readonlyDict.Values.Should().Equal(new int[] { 1 });
        }

        [Fact]
        public void When_adapting_to_readonly_get_enumerator_should_be_consistent_with_original_dict()
        {
            // Arrange
            var dict = new Dictionary<int, int> { { 0, 1 } };

            // Act
            IEnumerable readonlyDict = dict.AsReadOnlyDictionary();

            // Assert
            IEnumerator enumerator = readonlyDict.GetEnumerator();
            bool next1 = enumerator.MoveNext();
            next1.Should().BeTrue();
            enumerator.Current.Should().BeOfType<KeyValuePair<int, int>>();
            bool next2 = enumerator.MoveNext();
            next2.Should().BeFalse();
        }


    }
}

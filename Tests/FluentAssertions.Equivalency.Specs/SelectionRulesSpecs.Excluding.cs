using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class Excluding
    {
        [Fact]
        public async Task A_member_excluded_by_path_is_described_in_the_failure_message()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 13
            };

            var customer = new
            {
                Name = "Jack",
                Age = 37
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(customer, options => options
                .Excluding(d => d.Age));

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*Exclude*Age*");
        }

        [Fact]
        public async Task A_member_excluded_by_predicate_is_described_in_the_failure_message()
        {
            // Arrange
            var subject = new
            {
                Name = "John",
                Age = 13
            };

            var customer = new
            {
                Name = "Jack",
                Age = 37
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(customer, options => options
                .Excluding(ctx => ctx.Path == "Age"));

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*Exclude member when*Age*");
        }

        [Fact]
        public async Task When_only_the_excluded_property_doesnt_match_it_should_not_throw()
        {
            // Arrange
            var dto = new CustomerDto
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            // Act / Assert
            await dto.Should().BeEquivalentToAsync(customer, options => options
                .Excluding(d => d.Name)
                .Excluding(d => d.Id));
        }

        [Fact]
        public async Task When_only_the_excluded_property_doesnt_match_it_should_not_throw_if_root_is_a_collection()
        {
            // Arrange
            var dto = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new Customer
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "Dennis"
            };

            // Act / Assert
            await new[] { dto }.Should().BeEquivalentToAsync(new[] { customer }, options => options
                .Excluding(d => d.Name)
                .Excluding(d => d.Id));
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task When_excluding_members_it_should_pass_if_only_the_excluded_members_are_different()
        {
            // Arrange
            var class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit"
            };

            var class2 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum"
            };

            // Act
            Func<Task> act =
                () =>
                    class1.Should().BeEquivalentToAsync(class2,
                        opts => opts.Excluding(o => o.Field3).Excluding(o => o.Property1));

            // Assert
            await act.Should().NotThrowAsync("the non-excluded fields have the same value");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task When_excluding_members_it_should_fail_if_any_non_excluded_members_are_different()
        {
            // Arrange
            var class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit"
            };

            var class2 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum"
            };

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.Excluding(o => o.Property1));

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected*Field3*");
        }

        [Fact]
        public async Task When_all_shared_properties_match_it_should_not_throw()
        {
            // Arrange
            var dto = new CustomerDto
            {
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            var customer = new Customer
            {
                Id = 1,
                Age = 36,
                Birthdate = new DateTime(1973, 9, 20),
                Name = "John"
            };

            // Act
            Func<Task> act = () => dto.Should().BeEquivalentToAsync(customer, options => options.ExcludingMissingMembers());

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_a_deeply_nested_property_with_a_value_mismatch_is_excluded_it_should_not_throw()
        {
            // Arrange
            var subject = new Root
            {
                Text = "Root",
                Level = new Level1
                {
                    Text = "Level1",
                    Level = new Level2
                    {
                        Text = "Mismatch"
                    }
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level1",
                    Level = new Level2Dto
                    {
                        Text = "Level2"
                    }
                }
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected,
                options => options.Excluding(r => r.Level.Level.Text));

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_a_deeply_nested_property_with_a_value_mismatch_is_excluded_it_should_not_throw_if_root_is_a_collection()
        {
            // Arrange
            var subject = new Root
            {
                Text = "Root",
                Level = new Level1
                {
                    Text = "Level1",
                    Level = new Level2
                    {
                        Text = "Mismatch"
                    }
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level1",
                    Level = new Level2Dto
                    {
                        Text = "Level2"
                    }
                }
            };

            // Act / Assert
            await new[] { subject }.Should().BeEquivalentToAsync(new[] { expected },
                options => options.Excluding(r => r.Level.Level.Text));
        }

        [Fact]
        public async Task When_a_property_with_a_value_mismatch_is_excluded_using_a_predicate_it_should_not_throw()
        {
            // Arrange
            var subject = new Root
            {
                Text = "Root",
                Level = new Level1
                {
                    Text = "Level1",
                    Level = new Level2
                    {
                        Text = "Mismatch"
                    }
                }
            };

            var expected = new RootDto
            {
                Text = "Root",
                Level = new Level1Dto
                {
                    Text = "Level1",
                    Level = new Level2Dto
                    {
                        Text = "Level2"
                    }
                }
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected, config =>
                config.Excluding(ctx => ctx.Path == "Level.Level.Text"));

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_members_are_excluded_by_the_access_modifier_of_the_getter_using_a_predicate_they_should_be_ignored()
        {
            // Arrange
            var subject = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "internal", "protected-internal", "private", "private-protected");

            var expected = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "ignored-internal", "ignored-protected-internal", "private", "private-protected");

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected, config => config
                .IncludingInternalFields()
                .Excluding(ctx =>
                    ctx.WhichGetterHas(CSharpAccessModifier.Internal) ||
                    ctx.WhichGetterHas(CSharpAccessModifier.ProtectedInternal)));

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_members_are_excluded_by_the_access_modifier_of_the_setter_using_a_predicate_they_should_be_ignored()
        {
            // Arrange
            var subject = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "internal", "protected-internal", "private", "private-protected");

            var expected = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "ignored-internal", "ignored-protected-internal", "ignored-private", "private-protected");

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(expected, config => config
                .IncludingInternalFields()
                .Excluding(ctx =>
                    ctx.WhichSetterHas(CSharpAccessModifier.Internal) ||
                    ctx.WhichSetterHas(CSharpAccessModifier.ProtectedInternal) ||
                    ctx.WhichSetterHas(CSharpAccessModifier.Private)));

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task When_excluding_properties_it_should_still_compare_fields()
        {
            // Arrange
            var class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            var class2 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "color"
            };

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.ExcludingProperties());

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("*color*dolor*");
        }

        [Fact]
        public async Task When_excluding_fields_it_should_still_compare_properties()
        {
            // Arrange
            var class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            var class2 = new ClassWithSomeFieldsAndProperties
            {
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "different"
            };

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.ExcludingFields());

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage("*Property3*consectetur*");
        }

        [Fact]
        public async Task When_excluding_properties_via_non_array_indexers_it_should_exclude_the_specified_paths()
        {
            // Arrange
            var subject = new
            {
                List = new[]
                {
                    new
                    {
                        Foo = 1,
                        Bar = 2
                    },
                    new
                    {
                        Foo = 3,
                        Bar = 4
                    }
                }.ToList(),
                Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
                {
                    ["Foo"] = new()
                    {
                        Value = 1
                    },
                    ["Bar"] = new()
                    {
                        Value = 2
                    }
                }
            };

            var expected = new
            {
                List = new[]
                {
                    new
                    {
                        Foo = 1,
                        Bar = 2
                    },
                    new
                    {
                        Foo = 2,
                        Bar = 4
                    }
                }.ToList(),
                Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
                {
                    ["Foo"] = new()
                    {
                        Value = 1
                    },
                    ["Bar"] = new()
                    {
                        Value = 3
                    }
                }
            };

            // Act
            Func<Task> act = () =>
                subject.Should().BeEquivalentToAsync(expected,
                    options => options
                        .Excluding(x => x.List[1].Foo)
                        .Excluding(x => x.Dictionary["Bar"].Value));

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task
            When_excluding_properties_via_non_array_indexers_it_should_exclude_the_specified_paths_if_root_is_a_collection()
        {
            // Arrange
            var subject = new
            {
                List = new[]
                {
                    new
                    {
                        Foo = 1,
                        Bar = 2
                    },
                    new
                    {
                        Foo = 3,
                        Bar = 4
                    }
                }.ToList(),
                Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
                {
                    ["Foo"] = new()
                    {
                        Value = 1
                    },
                    ["Bar"] = new()
                    {
                        Value = 2
                    }
                }
            };

            var expected = new
            {
                List = new[]
                {
                    new
                    {
                        Foo = 1,
                        Bar = 2
                    },
                    new
                    {
                        Foo = 2,
                        Bar = 4
                    }
                }.ToList(),
                Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
                {
                    ["Foo"] = new()
                    {
                        Value = 1
                    },
                    ["Bar"] = new()
                    {
                        Value = 3
                    }
                }
            };

            // Act / Assert
            await new[] { subject }.Should().BeEquivalentToAsync(new[] { expected },
                options => options
                    .Excluding(x => x.List[1].Foo)
                    .Excluding(x => x.Dictionary["Bar"].Value));
        }

        [Fact]
        public async Task When_excluding_properties_via_non_array_indexers_it_should_not_exclude_paths_with_different_indexes()
        {
            // Arrange
            var subject = new
            {
                List = new[]
                {
                    new
                    {
                        Foo = 1,
                        Bar = 2
                    },
                    new
                    {
                        Foo = 3,
                        Bar = 4
                    }
                }.ToList(),
                Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
                {
                    ["Foo"] = new()
                    {
                        Value = 1
                    },
                    ["Bar"] = new()
                    {
                        Value = 2
                    }
                }
            };

            var expected = new
            {
                List = new[]
                {
                    new
                    {
                        Foo = 5,
                        Bar = 2
                    },
                    new
                    {
                        Foo = 2,
                        Bar = 4
                    }
                }.ToList(),
                Dictionary = new Dictionary<string, ClassWithOnlyAProperty>
                {
                    ["Foo"] = new()
                    {
                        Value = 6
                    },
                    ["Bar"] = new()
                    {
                        Value = 3
                    }
                }
            };

            // Act
            Func<Task> act = () =>
                subject.Should().BeEquivalentToAsync(expected,
                    options => options
                        .Excluding(x => x.List[1].Foo)
                        .Excluding(x => x.Dictionary["Bar"].Value));

            // Assert
            await act.Should().ThrowAsync<XunitException>();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task
            When_configured_for_runtime_typing_and_properties_are_excluded_the_runtime_type_should_be_used_and_properties_should_be_ignored()
        {
            // Arrange
            object class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            object class2 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor"
            };

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.ExcludingProperties().RespectingRuntimeTypes());

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_excluding_virtual_or_abstract_property_exclusion_works_properly()
        {
            var obj1 = new Derived
            {
                DerivedProperty1 = "Something",
                DerivedProperty2 = "A"
            };

            var obj2 = new Derived
            {
                DerivedProperty1 = "Something",
                DerivedProperty2 = "B"
            };

            await obj1.Should().BeEquivalentToAsync(obj2, opt => opt
                .Excluding(o => o.AbstractProperty)
                .Excluding(o => o.VirtualProperty)
                .Excluding(o => o.DerivedProperty2));
        }
    }
}

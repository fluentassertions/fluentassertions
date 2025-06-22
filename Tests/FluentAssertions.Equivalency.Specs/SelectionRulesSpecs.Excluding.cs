using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class Excluding
    {
        [Fact]
        public void A_member_excluded_by_path_is_described_in_the_failure_message()
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
            Action act = () => subject.Should().BeEquivalentTo(customer, options => options
                .Excluding(d => d.Age));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Exclude*Age*");
        }

        [Fact]
        public void A_member_excluded_by_predicate_is_described_in_the_failure_message()
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
            Action act = () => subject.Should().BeEquivalentTo(customer, options => options
                .Excluding(ctx => ctx.Path == "Age"));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Exclude member when*Age*");
        }

        [Fact]
        public void When_only_the_excluded_property_doesnt_match_it_should_not_throw()
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
            dto.Should().BeEquivalentTo(customer, options => options
                .Excluding(d => d.Name)
                .Excluding(d => d.Id));
        }

        [Fact]
        public void When_only_the_excluded_property_doesnt_match_it_should_not_throw_if_root_is_a_collection()
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
            new[] { dto }.Should().BeEquivalentTo(new[] { customer }, options => options
                .Excluding(d => d.Name)
                .Excluding(d => d.Id));
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_excluding_members_it_should_pass_if_only_the_excluded_members_are_different()
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
            Action act =
                () =>
                    class1.Should().BeEquivalentTo(class2,
                        opts => opts.Excluding(o => o.Field3).Excluding(o => o.Property1));

            // Assert
            act.Should().NotThrow("the non-excluded fields have the same value");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_excluding_members_it_should_fail_if_any_non_excluded_members_are_different()
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
            Action act =
                () => class1.Should().BeEquivalentTo(class2, opts => opts.Excluding(o => o.Property1));

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*Field3*");
        }

        [Fact]
        public void When_all_shared_properties_match_it_should_not_throw()
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
            Action act = () => dto.Should().BeEquivalentTo(customer, options => options.ExcludingMissingMembers());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_deeply_nested_property_with_a_value_mismatch_is_excluded_it_should_not_throw()
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
            Action act = () => subject.Should().BeEquivalentTo(expected,
                options => options.Excluding(r => r.Level.Level.Text));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_deeply_nested_property_with_a_value_mismatch_is_excluded_it_should_not_throw_if_root_is_a_collection()
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
            new[] { subject }.Should().BeEquivalentTo(new[] { expected },
                options => options.Excluding(r => r.Level.Level.Text));
        }

        [Fact]
        public void When_a_property_with_a_value_mismatch_is_excluded_using_a_predicate_it_should_not_throw()
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
            Action act = () => subject.Should().BeEquivalentTo(expected, config =>
                config.Excluding(ctx => ctx.Path == "Level.Level.Text"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_members_are_excluded_by_the_access_modifier_of_the_getter_using_a_predicate_they_should_be_ignored()
        {
            // Arrange
            var subject = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "internal", "protected-internal", "private", "private-protected");

            var expected = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "ignored-internal", "ignored-protected-internal", "private", "private-protected");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected, config => config
                .IncludingInternalFields()
                .Excluding(ctx =>
                    ctx.WhichGetterHas(CSharpAccessModifier.Internal) ||
                    ctx.WhichGetterHas(CSharpAccessModifier.ProtectedInternal)));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_members_are_excluded_by_the_access_modifier_of_the_setter_using_a_predicate_they_should_be_ignored()
        {
            // Arrange
            var subject = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "internal", "protected-internal", "private", "private-protected");

            var expected = new ClassWithAllAccessModifiersForMembers("public", "protected",
                "ignored-internal", "ignored-protected-internal", "ignored-private", "private-protected");

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected, config => config
                .IncludingInternalFields()
                .Excluding(ctx =>
                    ctx.WhichSetterHas(CSharpAccessModifier.Internal) ||
                    ctx.WhichSetterHas(CSharpAccessModifier.ProtectedInternal) ||
                    ctx.WhichSetterHas(CSharpAccessModifier.Private)));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void When_excluding_properties_it_should_still_compare_fields()
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
            Action act =
                () => class1.Should().BeEquivalentTo(class2, opts => opts.ExcludingProperties());

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*color*dolor*");
        }

        [Fact]
        public void When_excluding_fields_it_should_still_compare_properties()
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
            Action act =
                () => class1.Should().BeEquivalentTo(class2, opts => opts.ExcludingFields());

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Property3*consectetur*");
        }

        [Fact]
        public void When_excluding_properties_via_non_array_indexers_it_should_exclude_the_specified_paths()
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
            Action act = () =>
                subject.Should().BeEquivalentTo(expected,
                    options => options
                        .Excluding(x => x.List[1].Foo)
                        .Excluding(x => x.Dictionary["Bar"].Value));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void
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
            new[] { subject }.Should().BeEquivalentTo(new[] { expected },
                options => options
                    .Excluding(x => x.List[1].Foo)
                    .Excluding(x => x.Dictionary["Bar"].Value));
        }

        [Fact]
        public void When_excluding_properties_via_non_array_indexers_it_should_not_exclude_paths_with_different_indexes()
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
            Action act = () =>
                subject.Should().BeEquivalentTo(expected,
                    options => options
                        .Excluding(x => x.List[1].Foo)
                        .Excluding(x => x.Dictionary["Bar"].Value));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void
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
            Action act =
                () => class1.Should().BeEquivalentTo(class2, opts => opts.ExcludingProperties().PreferringRuntimeMemberTypes());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_excluding_virtual_or_abstract_property_exclusion_works_properly()
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

            obj1.Should().BeEquivalentTo(obj2, opt => opt
                .Excluding(o => o.AbstractProperty)
                .Excluding(o => o.VirtualProperty)
                .Excluding(o => o.DerivedProperty2));
        }

        [Fact]
        public void Abstract_properties_cannot_be_excluded()
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

            Action act = () => obj1.Should().BeEquivalentTo(obj2, opt => opt
                .Excluding(o => o.AbstractProperty + "B"));

            act.Should().Throw<ArgumentException>()
                .WithMessage("*(o.AbstractProperty + \"B\")*cannot be used to select a member*");
        }

#if NETCOREAPP3_0_OR_GREATER
        [Fact]
        public void Can_exclude_a_default_interface_property_using_an_expression()
        {
            // Arrange
            IHaveDefaultProperty subject = new ClassReceivedDefaultInterfaceProperty
            {
                NormalProperty = "Value"
            };

            IHaveDefaultProperty expectation = new ClassReceivedDefaultInterfaceProperty
            {
                NormalProperty = "Another Value"
            };

            // Act
            var act = () => subject.Should().BeEquivalentTo(expectation,
                x => x.Excluding(p => p.DefaultProperty));

            // Assert
            act.Should().Throw<XunitException>().Which.Message.Should().NotContain("subject.DefaultProperty");
        }

        [Fact]
        public void Can_exclude_a_default_interface_property_using_a_name()
        {
            // Arrange
            IHaveDefaultProperty subject = new ClassReceivedDefaultInterfaceProperty
            {
                NormalProperty = "Value"
            };

            IHaveDefaultProperty expectation = new ClassReceivedDefaultInterfaceProperty
            {
                NormalProperty = "Another Value"
            };

            // Act
            var act = () => subject.Should().BeEquivalentTo(expectation,
                x => x.Excluding(info => info.Name.Contains("DefaultProperty")));

            // Assert
            act.Should().Throw<XunitException>().Which.Message.Should().NotContain("subject.DefaultProperty");
        }

        private class ClassReceivedDefaultInterfaceProperty : IHaveDefaultProperty
        {
            public string NormalProperty { get; set; }
        }

        private interface IHaveDefaultProperty
        {
            string NormalProperty { get; set; }

            int DefaultProperty => NormalProperty.Length;
        }
#endif

        [Fact]
        public void An_anonymous_object_with_multiple_fields_excludes_correctly()
        {
            // Arrange
            var subject = new
            {
                FirstName = "John",
                MiddleName = "X",
                LastName = "Doe",
                Age = 34
            };

            var expectation = new
            {
                FirstName = "John",
                MiddleName = "W.",
                LastName = "Smith",
                Age = 29
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { p.MiddleName, p.LastName, p.Age }));
        }

        [Fact]
        public void An_empty_anonymous_object_excludes_nothing()
        {
            // Arrange
            var subject = new
            {
                FirstName = "John",
                MiddleName = "X",
                LastName = "Doe",
                Age = 34
            };

            var expectation = new
            {
                FirstName = "John",
                MiddleName = "W.",
                LastName = "Smith",
                Age = 29
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { }));

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void An_anonymous_object_can_exclude_collections()
        {
            // Arrange
            var subject = new
            {
                Names = new[]
                {
                    "John",
                    "X.",
                    "Doe"
                },
                Age = 34
            };

            var expectation = new
            {
                Names = new[]
                {
                    "John",
                    "W.",
                    "Smith"
                },
                Age = 34
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { p.Names }));
        }

        [Fact]
        public void An_anonymous_object_can_exclude_nested_objects()
        {
            // Arrange
            var subject = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "X",
                    LastName = "Doe",
                },
                Age = 34
            };

            var expectation = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "W.",
                    LastName = "Smith",
                },
                Age = 34
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { p.Names.MiddleName, p.Names.LastName }));
        }

        [Fact]
        public void An_anonymous_object_can_exclude_nested_objects_inside_collections()
        {
            // Arrange
            var subject = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "X",
                    LastName = "Doe",
                },
                Pets = new[]
                {
                    new
                    {
                        Name = "Dog",
                        Age = 1,
                        Color = "Black"
                    },
                    new
                    {
                        Name = "Cat",
                        Age = 1,
                        Color = "Black"
                    },
                    new
                    {
                        Name = "Bird",
                        Age = 1,
                        Color = "Black"
                    },
                }
            };

            var expectation = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "W.",
                    LastName = "Smith",
                },
                Pets = new[]
                {
                    new
                    {
                        Name = "Dog",
                        Age = 1,
                        Color = "Black"
                    },
                    new
                    {
                        Name = "Dog",
                        Age = 2,
                        Color = "Gray"
                    },
                    new
                    {
                        Name = "Bird",
                        Age = 3,
                        Color = "Black"
                    },
                }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { p.Names.MiddleName, p.Names.LastName })
                .For(p => p.Pets)
                .Exclude(p => new { p.Age, p.Name }));

            // Assert
            act.Should().Throw<XunitException>().Which.Message.Should()
                .NotMatch("*Pets[1].Age*").And
                .NotMatch("*Pets[1].Name*").And
                .Match("*Pets[1].Color*");
        }

        [Fact]
        public void An_anonymous_object_can_exclude_nested_objects_inside_nested_collections()
        {
            // Arrange
            var subject = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "W.",
                    LastName = "Smith",
                },
                Pets = new[]
                {
                    new
                    {
                        Name = "Dog",
                        Fleas = new[]
                        {
                            new
                            {
                                Name = "Flea 1",
                                Age = 1,
                            },
                            new
                            {
                                Name = "Flea 2",
                                Age = 2,
                            },
                        },
                    },
                    new
                    {
                        Name = "Dog",
                        Fleas = new[]
                        {
                            new
                            {
                                Name = "Flea 10",
                                Age = 1,
                            },
                            new
                            {
                                Name = "Flea 21",
                                Age = 3,
                            },
                        },
                    },
                    new
                    {
                        Name = "Dog",
                        Fleas = new[]
                        {
                            new
                            {
                                Name = "Flea 1",
                                Age = 1,
                            },
                            new
                            {
                                Name = "Flea 2",
                                Age = 2,
                            },
                        },
                    },
                },
            };

            var expectation = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "W.",
                    LastName = "Smith",
                },
                Pets = new[]
                {
                    new
                    {
                        Name = "Dog",
                        Fleas = new[]
                        {
                            new
                            {
                                Name = "Flea 1",
                                Age = 1,
                            },
                            new
                            {
                                Name = "Flea 2",
                                Age = 2,
                            },
                        },
                    },
                    new
                    {
                        Name = "Dog",
                        Fleas = new[]
                        {
                            new
                            {
                                Name = "Flea 1",
                                Age = 1,
                            },
                            new
                            {
                                Name = "Flea 2",
                                Age = 1,
                            },
                        },
                    },
                    new
                    {
                        Name = "Bird",
                        Fleas = new[]
                        {
                            new
                            {
                                Name = "Flea 1",
                                Age = 1,
                            },
                            new
                            {
                                Name = "Flea 2",
                                Age = 2,
                            },
                        },
                    },
                },
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { p.Names.MiddleName, p.Names.LastName })
                .For(person => person.Pets)
                .For(pet => pet.Fleas)
                .Exclude(flea => new { flea.Name, flea.Age }));

            // Assert
            act.Should().Throw<XunitException>().Which.Message.Should()
                .NotMatch("*Pets[*].Fleas[*].Age*").And
                .NotMatch("*Pets[*].Fleas[*].Name*").And
                .Match("*- Exclude*Pets[]Fleas[]Age*").And
                .Match("*- Exclude*Pets[]Fleas[]Name*");
        }

        [Fact]
        public void An_empty_anonymous_object_excludes_nothing_inside_collections()
        {
            // Arrange
            var subject = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "X",
                    LastName = "Doe",
                },
                Pets = new[]
                {
                    new
                    {
                        Name = "Dog",
                        Age = 1
                    },
                    new
                    {
                        Name = "Cat",
                        Age = 1
                    },
                    new
                    {
                        Name = "Bird",
                        Age = 1
                    },
                }
            };

            var expectation = new
            {
                Names = new
                {
                    FirstName = "John",
                    MiddleName = "W.",
                    LastName = "Smith",
                },
                Pets = new[]
                {
                    new
                    {
                        Name = "Dog",
                        Age = 1
                    },
                    new
                    {
                        Name = "Dog",
                        Age = 2
                    },
                    new
                    {
                        Name = "Bird",
                        Age = 1
                    },
                }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(p => new { p.Names.MiddleName, p.Names.LastName })
                .For(p => p.Pets)
                .Exclude(p => new { }));

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Pets[1].Name*Pets[1].Age*");
        }

        [Fact]
        public void Can_exclude_root_properties_by_name()
        {
            // Arrange
            var subject = new
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };

            var expectation = new
            {
                FirstName = "John",
                LastName = "Smith",
                Age = 35
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .ExcludingMembersNamed("LastName", "Age"));
        }

        [Fact]
        public void Can_exclude_properties_deeper_in_the_graph_by_name()
        {
            // Arrange
            var subject = new
            {
                Person = new
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 30
                },
                Address = new
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    ZipCode = "12345"
                }
            };

            var expectation = new
            {
                Person = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Age = 35
                },
                Address = new
                {
                    Street = "123 Main St",
                    City = "Othertown",
                    ZipCode = "54321"
                }
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .ExcludingMembersNamed("LastName", "Age", "City", "ZipCode"));
        }

        [Fact]
        public void Must_provide_property_names_when_excluding_by_name()
        {
            // Arrange
            var subject = new
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(subject, options => options
                .ExcludingMembersNamed());

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*least one*name*");
        }

        [Fact]
        public void Cannot_provide_null_as_a_property_name()
        {
            // Arrange
            var subject = new
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(subject, options => options
                .ExcludingMembersNamed(null));

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Member names cannot be null*");
        }
    }
}

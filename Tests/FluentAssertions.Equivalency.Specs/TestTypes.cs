using System;
using System.Collections.Generic;

// ReSharper disable NotAccessedField.Global
// ReSharper disable NotAccessedField.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
#pragma warning disable SA1649

namespace FluentAssertionsAsync.Equivalency.Specs;

public enum EnumULong : ulong
{
    Int64Max = long.MaxValue,
    UInt64LessOne = ulong.MaxValue - 1,
    UInt64Max = ulong.MaxValue
}

public enum EnumLong : long
{
    Int64Max = long.MaxValue,
    Int64LessOne = long.MaxValue - 1
}

#region Test Classes

public class ClassOne
{
    public ClassTwo RefOne { get; set; } = new();

    public int ValOne { get; set; } = 1;
}

public class ClassTwo
{
    public int ValTwo { get; set; } = 3;
}

public class ClassWithWriteOnlyProperty
{
    private int writeOnlyPropertyValue;

    public int WriteOnlyProperty
    {
        set => writeOnlyPropertyValue = value;
    }

    public string SomeOtherProperty { get; set; }
}

internal enum EnumOne
{
    One = 0,
    Two = 3
}

internal enum EnumCharOne
{
    A = 'A',
    B = 'B'
}

internal enum EnumCharTwo
{
    A = 'Z',
    ValueB = 'B'
}

internal enum EnumTwo
{
    One = 0,
    Two = 3
}

internal enum EnumThree
{
    ValueZero = 0,
    Two = 3
}

internal enum EnumFour
{
    Three = 3
}

internal class ClassWithEnumCharOne
{
    public EnumCharOne Enum { get; set; }
}

internal class ClassWithEnumCharTwo
{
    public EnumCharTwo Enum { get; set; }
}

internal class ClassWithEnumOne
{
    public EnumOne Enum { get; set; }
}

internal class ClassWithEnumTwo
{
    public EnumTwo Enum { get; set; }
}

internal class ClassWithEnumThree
{
    public EnumThree Enum { get; set; }
}

internal class ClassWithEnumFour
{
    public EnumFour Enum { get; set; }
}

internal class ClassWithNoMembers
{
}

internal class ClassWithOnlyAField
{
    public int Value;
}

internal class ClassWithAPrivateField : ClassWithOnlyAField
{
    private readonly int value;

    public ClassWithAPrivateField(int value)
    {
        this.value = value;
    }
}

internal class ClassWithOnlyAProperty
{
    public int Value { get; set; }
}

internal struct StructWithNoMembers
{
}

internal class ClassWithSomeFieldsAndProperties
{
    public string Field1;

    public string Field2;

    public string Field3;

    public string Property1 { get; set; }

    public string Property2 { get; set; }

    public string Property3 { get; set; }
}

internal class ClassWithCctor
{
    // ReSharper disable once EmptyConstructor
    static ClassWithCctor()
    {
    }
}

internal class ClassWithCctorAndNonDefaultConstructor
{
    // ReSharper disable once EmptyConstructor
    static ClassWithCctorAndNonDefaultConstructor() { }

    public ClassWithCctorAndNonDefaultConstructor(int _) { }
}

internal class MyCompanyLogo
{
    public string Url { get; set; }

    public MyCompany Company { get; set; }

    public MyUser CreatedBy { get; set; }
}

internal class MyUser
{
    public string Name { get; set; }

    public MyCompany Company { get; set; }
}

internal class MyCompany
{
    public string Name { get; set; }

    public MyCompanyLogo Logo { get; set; }

    public List<MyUser> Users { get; set; }
}

public class Customer : Entity
{
    private string PrivateProperty { get; set; }

    protected string ProtectedProperty { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public DateTime Birthdate { get; set; }

    public long Id { get; set; }

    public void SetProtected(string value)
    {
        ProtectedProperty = value;
    }

    public Customer()
    {
    }

    public Customer(string privateProperty)
    {
        PrivateProperty = privateProperty;
    }
}

public class Entity
{
    internal long Version { get; set; }
}

public class CustomerDto
{
    public string Name { get; set; }

    public int Age { get; set; }

    public DateTime Birthdate { get; set; }
}

public class CustomerType
{
    public CustomerType(string code)
    {
        Code = code;
    }

    public string Code { get; }

    public override bool Equals(object obj)
    {
        return obj is CustomerType other && Code == other.Code;
    }

    public override int GetHashCode()
    {
        return Code?.GetHashCode() ?? 0;
    }

    public static bool operator ==(CustomerType a, CustomerType b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Code == b.Code;
    }

    public static bool operator !=(CustomerType a, CustomerType b)
    {
        return !(a == b);
    }
}

public class DerivedCustomerType : CustomerType
{
    public string DerivedInfo { get; set; }

    public DerivedCustomerType(string code)
        : base(code)
    {
    }
}

public class CustomConvertible : IConvertible
{
    private readonly string convertedStringValue;

    public CustomConvertible(string convertedStringValue)
    {
        this.convertedStringValue = convertedStringValue;
    }

    public TypeCode GetTypeCode()
    {
        throw new InvalidCastException();
    }

    public bool ToBoolean(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public char ToChar(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public sbyte ToSByte(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public byte ToByte(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public short ToInt16(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public ushort ToUInt16(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public int ToInt32(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public uint ToUInt32(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public long ToInt64(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public ulong ToUInt64(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public float ToSingle(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public double ToDouble(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public decimal ToDecimal(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public DateTime ToDateTime(IFormatProvider provider)
    {
        throw new InvalidCastException();
    }

    public string ToString(IFormatProvider provider)
    {
        return convertedStringValue;
    }

    public object ToType(Type conversionType, IFormatProvider provider)
    {
        throw new InvalidCastException();
    }
}

public abstract class Base
{
    public abstract string AbstractProperty { get; }

    public virtual string VirtualProperty => "Foo";

    public virtual string NonExcludedBaseProperty => "Foo";
}

public class Derived : Base
{
    public string DerivedProperty1 { get; set; }

    public string DerivedProperty2 { get; set; }

    public override string AbstractProperty => $"{DerivedProperty1} {DerivedProperty2}";

    public override string VirtualProperty => "Bar";

    public override string NonExcludedBaseProperty => "Bar";

    public virtual string NonExcludedDerivedProperty => "Foo";
}

#endregion

#region Nested classes for comparison

public class ClassWithAllAccessModifiersForMembers
{
    public string PublicField;
    protected string protectedField;
    internal string InternalField;
    protected internal string ProtectedInternalField;
    private readonly string privateField;
    private protected string privateProtectedField;

    public string PublicProperty { get; set; }

    public string ReadOnlyProperty { get; private set; }

    public string WriteOnlyProperty { private get; set; }

    protected string ProtectedProperty { get; set; }

    internal string InternalProperty { get; set; }

    protected internal string ProtectedInternalProperty { get; set; }

    private string PrivateProperty { get; set; }

    private protected string PrivateProtectedProperty { get; set; }

    public ClassWithAllAccessModifiersForMembers(string publicValue, string protectedValue, string internalValue,
        string protectedInternalValue, string privateValue, string privateProtectedValue)
    {
        PublicField = publicValue;
        protectedField = protectedValue;
        InternalField = internalValue;
        ProtectedInternalField = protectedInternalValue;
        privateField = privateValue;
        privateProtectedField = privateProtectedValue;

        PublicProperty = publicValue;
        ReadOnlyProperty = privateValue;
        WriteOnlyProperty = privateValue;
        ProtectedProperty = protectedValue;
        InternalProperty = internalValue;
        ProtectedInternalProperty = protectedInternalValue;
        PrivateProperty = privateValue;
        PrivateProtectedProperty = privateProtectedValue;
    }
}

public class ClassWithValueSemanticsOnSingleProperty
{
    public string Key { get; set; }

    public string NestedProperty { get; set; }

    protected bool Equals(ClassWithValueSemanticsOnSingleProperty other)
    {
        return Key == other.Key;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((ClassWithValueSemanticsOnSingleProperty)obj);
    }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}

public class Root
{
    public string Text { get; set; }

    public Level1 Level { get; set; }
}

public class Level1
{
    public string Text { get; set; }

    public Level2 Level { get; set; }
}

public class Level2
{
    public string Text { get; set; }
}

public class RootDto
{
    public string Text { get; set; }

    public Level1Dto Level { get; set; }
}

public class Level1Dto
{
    public string Text { get; set; }

    public Level2Dto Level { get; set; }
}

public class Level2Dto
{
    public string Text { get; set; }
}

public class CyclicRoot
{
    public string Text { get; set; }

    public CyclicLevel1 Level { get; set; }
}

public class CyclicRootWithValueObject
{
    public ValueObject Value { get; set; }

    public CyclicLevelWithValueObject Level { get; set; }
}

public class ValueObject
{
    public ValueObject(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override bool Equals(object obj)
    {
        return ((ValueObject)obj).Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class CyclicLevel1
{
    public string Text { get; set; }

    public CyclicRoot Root { get; set; }
}

public class CyclicLevelWithValueObject
{
    public ValueObject Value { get; set; }

    public CyclicRootWithValueObject Root { get; set; }
}

public class CyclicRootDto
{
    public string Text { get; set; }

    public CyclicLevel1Dto Level { get; set; }
}

public class CyclicLevel1Dto
{
    public string Text { get; set; }

    public CyclicRootDto Root { get; set; }
}

#endregion

#region Interfaces for verifying inheritance of properties

public class Car : Vehicle, ICar
{
    public int Wheels { get; set; }
}

public class ExplicitCar : ExplicitVehicle, ICar
{
    public int Wheels { get; set; }
}

public class Vehicle : IVehicle
{
    public int VehicleId { get; set; }
}

public class VehicleWithField
{
    public int VehicleId;
}

public class ExplicitVehicle : IVehicle
{
    int IVehicle.VehicleId { get; set; }

    public int VehicleId { get; set; }
}

public interface IReadOnlyVehicle
{
    int VehicleId { get; }
}

public class ExplicitReadOnlyVehicle : IReadOnlyVehicle
{
    private readonly int explicitValue;

    public ExplicitReadOnlyVehicle(int explicitValue)
    {
        this.explicitValue = explicitValue;
    }

    int IReadOnlyVehicle.VehicleId => explicitValue;

    public int VehicleId { get; set; }
}

public interface ICar : IVehicle
{
    int Wheels { get; set; }
}

public interface IVehicle
{
    int VehicleId { get; set; }
}

public class SomeDto
{
    public string Name { get; set; }

    public int Age { get; set; }

    public DateTime Birthdate { get; set; }
}

#endregion

#pragma warning restore SA1649

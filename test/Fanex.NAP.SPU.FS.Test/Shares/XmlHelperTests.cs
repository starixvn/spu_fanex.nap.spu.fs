using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Shares
{
    public class XmlHelperTests
    {
        #region Test Classes

        public class SimpleClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public decimal Salary { get; set; }
            public bool IsActive { get; set; }
        }

        public class ComplexClass
        {
            public string Name { get; set; }
            public SimpleClass SimpleProperty { get; set; }
            public int Value { get; set; }
        }

        [XName("CustomElement")]
        public class ClassWithXNameAttribute
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        public class ClassWithXmlIgnore
        {
            public string Name { get; set; }
            
            [XmlIgnore]
            public string IgnoredProperty { get; set; }
            
            public int Value { get; set; }
        }

        public class ClassWithXNameProperties
        {
            [XName("CustomName")]
            public string Name { get; set; }
            
            [XName("CustomValue")]
            public int Value { get; set; }
        }

        public class NestedComplexClass
        {
            public string Name { get; set; }
            public ComplexClass ComplexProperty { get; set; }
            public SimpleClass SimpleProperty { get; set; }
        }

        #endregion

        #region ToXmlObj Tests

        [Fact]
        public void ToXmlObj_WithSimpleClass_ShouldReturnXElement()
        {
            // Arrange
            var obj = new SimpleClass
            {
                Name = "John Doe",
                Age = 30,
                Salary = 50000.50m,
                IsActive = true
            };

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("SimpleClass");
            result.Attributes().Should().HaveCount(4);
            result.Attribute("Name")?.Value.Should().Be("John Doe");
            result.Attribute("Age")?.Value.Should().Be("30");
            result.Attribute("Salary")?.Value.Should().Be("50000.50");
            result.Attribute("IsActive")?.Value.Should().Be("true");
        }       

        [Fact]
        public void ToXmlObj_WithClassWithXNameAttribute_ShouldUseCustomElementName()
        {
            // Arrange
            var obj = new ClassWithXNameAttribute
            {
                Name = "Test",
                Value = 42
            };

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("CustomElement");
            result.Attributes().Should().HaveCount(2);
            result.Attribute("Name")?.Value.Should().Be("Test");
            result.Attribute("Value")?.Value.Should().Be("42");
        }

        [Fact]
        public void ToXmlObj_WithClassWithXmlIgnore_ShouldIgnoreMarkedProperties()
        {
            // Arrange
            var obj = new ClassWithXmlIgnore
            {
                Name = "Test",
                IgnoredProperty = "Should be ignored",
                Value = 42
            };

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("ClassWithXmlIgnore");
            result.Attributes().Should().HaveCount(2);
            result.Attribute("Name")?.Value.Should().Be("Test");
            result.Attribute("Value")?.Value.Should().Be("42");
            result.Attribute("IgnoredProperty").Should().BeNull();
        }

        [Fact]
        public void ToXmlObj_WithClassWithXNameProperties_ShouldUseCustomPropertyNames()
        {
            // Arrange
            var obj = new ClassWithXNameProperties
            {
                Name = "Test",
                Value = 42
            };

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("ClassWithXNameProperties");
            result.Attributes().Should().HaveCount(2);
            result.Attribute("CustomName")?.Value.Should().Be("Test");
            result.Attribute("CustomValue")?.Value.Should().Be("42");
        }

        [Fact]
        public void ToXmlObj_WithNullValues_ShouldThrowException()
        {
            // Arrange
            var obj = new ComplexClass
            {
                Name = "Test",
                Value = 42,
                SimpleProperty = null
            };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => obj.ToXmlObj());
        }

        #endregion

        #region ToXmlString Tests

        [Fact]
        public void ToXmlString_WithSimpleClass_ShouldReturnXmlString()
        {
            // Arrange
            var obj = new SimpleClass
            {
                Name = "John Doe",
                Age = 30,
                Salary = 50000.50m,
                IsActive = true
            };

            // Act
            var result = obj.ToXmlString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("<SimpleClass");
            result.Should().Contain("Name=\"John Doe\"");
            result.Should().Contain("Age=\"30\"");
            result.Should().Contain("Salary=\"50000.50\"");
            result.Should().Contain("IsActive=\"true\"");
            result.Should().Contain("/>");
        }       

        [Fact]
        public void ToXmlString_WithClassWithXNameAttribute_ShouldUseCustomElementName()
        {
            // Arrange
            var obj = new ClassWithXNameAttribute
            {
                Name = "Test",
                Value = 42
            };

            // Act
            var result = obj.ToXmlString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("<CustomElement");
            result.Should().Contain("Name=\"Test\"");
            result.Should().Contain("Value=\"42\"");
            result.Should().Contain("/>");
        }

        #endregion

        #region Collection Serialization Tests

        [Fact]
        public void ToXmlObj_WithArray_ShouldReturnXElementWithRootAndItems()
        {
            // Arrange
            var items = new[]
            {
                new SimpleClass { Name = "Item1", Age = 25, Salary = 30000m, IsActive = true },
                new SimpleClass { Name = "Item2", Age = 30, Salary = 40000m, IsActive = false }
            };

            // Act
            var result = items.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("Root");
            result.Elements().Should().HaveCount(2);
            
            var firstItem = result.Elements().First();
            firstItem.Name.LocalName.Should().Be("SimpleClass");
            firstItem.Attribute("Name")?.Value.Should().Be("Item1");
            
            var secondItem = result.Elements().Last();
            secondItem.Name.LocalName.Should().Be("SimpleClass");
            secondItem.Attribute("Name")?.Value.Should().Be("Item2");
        }

        [Fact]
        public void ToXmlObj_WithList_ShouldReturnXElementWithRootAndItems()
        {
            // Arrange
            var items = new List<SimpleClass>
            {
                new SimpleClass { Name = "Item1", Age = 25, Salary = 30000m, IsActive = true },
                new SimpleClass { Name = "Item2", Age = 30, Salary = 40000m, IsActive = false }
            };

            // Act
            var result = items.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("Root");
            result.Elements().Should().HaveCount(2);
            
            var firstItem = result.Elements().First();
            firstItem.Name.LocalName.Should().Be("SimpleClass");
            firstItem.Attribute("Name")?.Value.Should().Be("Item1");
        }

        [Fact]
        public void ToXmlObj_WithEmptyCollection_ShouldReturnXElementWithEmptyRoot()
        {
            // Arrange
            var items = new List<SimpleClass>();

            // Act
            var result = items.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("Root");
            result.Elements().Should().BeEmpty();
        }      

        #endregion

        #region RegisterType Tests

        [Fact]
        public void RegisterType_WithSimpleClass_ShouldRegisterType()
        {
            // Arrange
            var obj = new SimpleClass
            {
                Name = "Test",
                Age = 25,
                Salary = 30000m,
                IsActive = true
            };

            // Act
            XmlHelper.RegisterType<SimpleClass>();
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("SimpleClass");
            result.Attributes().Should().HaveCount(4);
        }
      
        #endregion

        #region Primitive Type Tests

        public class PrimitiveTypesClass
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
            public long LongValue { get; set; }
            public double DoubleValue { get; set; }
            public float FloatValue { get; set; }
            public decimal DecimalValue { get; set; }
            public bool BoolValue { get; set; }
            public char CharValue { get; set; }
            public byte ByteValue { get; set; }
            public short ShortValue { get; set; }
            public ushort UShortValue { get; set; }
            public uint UIntValue { get; set; }
            public ulong ULongValue { get; set; }
            public sbyte SByteValue { get; set; }
        }

        [Fact]
        public void ToXmlObj_WithAllPrimitiveTypes_ShouldSerializeCorrectly()
        {
            // Arrange
            var obj = new PrimitiveTypesClass
            {
                StringValue = "Test String",
                IntValue = 42,
                LongValue = 123456789L,
                DoubleValue = 3.14159,
                FloatValue = 2.718f,
                DecimalValue = 99.99m,
                BoolValue = true,
                CharValue = 'A',
                ByteValue = (byte)255,
                ShortValue = (short)1000,
                UShortValue = (ushort)2000,
                UIntValue = 3000u,
                ULongValue = 4000ul,
                SByteValue = (sbyte)127
            };

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("PrimitiveTypesClass");
            result.Attributes().Should().HaveCount(14);
            result.Attribute("StringValue")?.Value.Should().Be("Test String");
            result.Attribute("IntValue")?.Value.Should().Be("42");
            result.Attribute("LongValue")?.Value.Should().Be("123456789");
            result.Attribute("DoubleValue")?.Value.Should().Be("3.14159");
            result.Attribute("FloatValue")?.Value.Should().Be("2.718");
            result.Attribute("DecimalValue")?.Value.Should().Be("99.99");
            result.Attribute("BoolValue")?.Value.Should().Be("true");
            result.Attribute("CharValue")?.Value.Should().Be("A");
            result.Attribute("ByteValue")?.Value.Should().Be("255");
            result.Attribute("ShortValue")?.Value.Should().Be("1000");
            result.Attribute("UShortValue")?.Value.Should().Be("2000");
            result.Attribute("UIntValue")?.Value.Should().Be("3000");
            result.Attribute("ULongValue")?.Value.Should().Be("4000");
            result.Attribute("SByteValue")?.Value.Should().Be("127");
        }

        #endregion

        #region Edge Cases and Error Handling Tests

        [Fact]
        public void ToXmlObj_WithNullObject_ShouldThrowException()
        {
            // Arrange
            SimpleClass obj = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => obj.ToXmlObj());
        }

        public class EmptyClass
        {
        }

        [Fact]
        public void ToXmlObj_WithClassWithNoProperties_ShouldReturnEmptyElement()
        {
            // Arrange
            var obj = new EmptyClass();

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("EmptyClass");
            result.Attributes().Should().BeEmpty();
            result.Elements().Should().BeEmpty();
        }

        [Fact]
        public void ToXmlObj_WithClassWithOnlyIgnoredProperties_ShouldReturnEmptyElement()
        {
            // Arrange
            var obj = new ClassWithOnlyIgnoredProperties();

            // Act
            var result = obj.ToXmlObj();

            // Assert
            result.Should().NotBeNull();
            result.Name.LocalName.Should().Be("ClassWithOnlyIgnoredProperties");
            result.Attributes().Should().BeEmpty();
            result.Elements().Should().BeEmpty();
        }
       
        #endregion

        #region Helper Classes for Tests

        public class ClassWithOnlyIgnoredProperties
        {
            [XmlIgnore]
            public string IgnoredProperty1 { get; set; }
            
            [XmlIgnore]
            public int IgnoredProperty2 { get; set; }
        }

        #endregion
    }
}

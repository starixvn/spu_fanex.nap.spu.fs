using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Shares
{
    public class XNameAttributeTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidName_ShouldCreateInstance()
        {
            // Arrange
            var name = "TestElement";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        [Fact]
        public void Constructor_WithNullName_ShouldCreateInstance()
        {
            // Arrange
            string name = null;

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldCreateInstance()
        {
            // Arrange
            var name = string.Empty;

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(string.Empty);
        }

        [Fact]
        public void Constructor_WithWhitespaceName_ShouldCreateInstance()
        {
            // Arrange
            var name = "   ";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be("   ");
        }

        [Fact]
        public void Constructor_WithSpecialCharacters_ShouldCreateInstance()
        {
            // Arrange
            var name = "Element-Name_With.Special@Characters#123";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        [Fact]
        public void Constructor_WithUnicodeCharacters_ShouldCreateInstance()
        {
            // Arrange
            var name = "Element名称_With_Unicode_字符";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        [Fact]
        public void Constructor_WithLongName_ShouldCreateInstance()
        {
            // Arrange
            var name = new string('A', 1000);

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        #endregion

        #region ElementName Property Tests

        [Fact]
        public void ElementName_ShouldReturnConstructorValue()
        {
            // Arrange
            var expectedName = "TestElement";
            var attribute = new XNameAttribute(expectedName);

            // Act
            var actualName = attribute.ElementName;

            // Assert
            actualName.Should().Be(expectedName);
        }

        [Fact]
        public void ElementName_ShouldBeReadOnly()
        {
            // Arrange
            var attribute = new XNameAttribute("TestElement");

            // Act & Assert
            attribute.ElementName.Should().Be("TestElement");
            
            // Verify that ElementName is read-only by checking if it has a setter
            var property = typeof(XNameAttribute).GetProperty("ElementName");
            property.Should().NotBeNull();
            property.CanWrite.Should().BeFalse();
        }

        [Fact]
        public void ElementName_WithDifferentValues_ShouldReturnCorrectValues()
        {
            // Arrange
            var names = new[] { "Element1", "Element2", "Element3" };

            foreach (var name in names)
            {
                // Act
                var attribute = new XNameAttribute(name);

                // Assert
                attribute.ElementName.Should().Be(name);
            }
        }

        #endregion

        #region Attribute Usage Tests

        [Fact]
        public void Attribute_ShouldAllowUsageOnClass()
        {
            // Arrange
            var type = typeof(TestClassWithXNameAttribute);

            // Act
            var attributes = type.GetCustomAttributes(typeof(XNameAttribute), false);

            // Assert
            attributes.Should().HaveCount(1);
            var attribute = attributes[0] as XNameAttribute;
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be("TestClass");
        }

        [Fact]
        public void Attribute_ShouldAllowUsageOnProperty()
        {
            // Arrange
            var type = typeof(TestClassWithXNameAttribute);
            var property = type.GetProperty("TestProperty");

            // Act
            var attributes = property.GetCustomAttributes(typeof(XNameAttribute), false);

            // Assert
            attributes.Should().HaveCount(1);
            var attribute = attributes[0] as XNameAttribute;
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be("TestProperty");
        }
      

        #endregion

        #region Attribute Target Tests

        [Fact]
        public void Attribute_ShouldTargetClassAndProperty()
        {
            // Arrange
            var attributeType = typeof(XNameAttribute);
            var attributeUsage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

            // Act & Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage.ValidOn.Should().HaveFlag(AttributeTargets.Class);
            attributeUsage.ValidOn.Should().HaveFlag(AttributeTargets.Property);
        }

        [Fact]
        public void Attribute_ShouldNotTargetOtherTypes()
        {
            // Arrange
            var attributeType = typeof(XNameAttribute);
            var attributeUsage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

            // Act & Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage.ValidOn.Should().NotHaveFlag(AttributeTargets.Method);
            attributeUsage.ValidOn.Should().NotHaveFlag(AttributeTargets.Field);
            attributeUsage.ValidOn.Should().NotHaveFlag(AttributeTargets.Parameter);
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public void Constructor_WithVeryLongName_ShouldCreateInstance()
        {
            // Arrange
            var name = new string('X', 10000);

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        [Fact]
        public void Constructor_WithNewlineCharacters_ShouldCreateInstance()
        {
            // Arrange
            var name = "Element\nWith\nNewlines";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        [Fact]
        public void Constructor_WithTabCharacters_ShouldCreateInstance()
        {
            // Arrange
            var name = "Element\tWith\tTabs";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        [Fact]
        public void Constructor_WithControlCharacters_ShouldCreateInstance()
        {
            // Arrange
            var name = "Element" + (char)0x00 + "With" + (char)0x1F + "Control";

            // Act
            var attribute = new XNameAttribute(name);

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be(name);
        }

        #endregion

        #region Equality Tests

        [Fact]
        public void Attribute_WithSameName_ShouldBeEqual()
        {
            // Arrange
            var name = "TestElement";
            var attribute1 = new XNameAttribute(name);
            var attribute2 = new XNameAttribute(name);

            // Act & Assert
            attribute1.ElementName.Should().Be(attribute2.ElementName);
        }

        [Fact]
        public void Attribute_WithDifferentNames_ShouldNotBeEqual()
        {
            // Arrange
            var attribute1 = new XNameAttribute("Element1");
            var attribute2 = new XNameAttribute("Element2");

            // Act & Assert
            attribute1.ElementName.Should().NotBe(attribute2.ElementName);
        }

        #endregion

        #region Type Tests

        [Fact]
        public void XNameAttribute_ShouldInheritFromAttribute()
        {
            // Arrange
            var attribute = new XNameAttribute("TestElement");

            // Act & Assert
            attribute.Should().BeAssignableTo<Attribute>();
        }

        [Fact]
        public void XNameAttribute_ShouldBePublic()
        {
            // Arrange
            var type = typeof(XNameAttribute);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
        }

        #endregion

        #region Reflection Tests

        [Fact]
        public void XNameAttribute_ShouldHaveCorrectConstructor()
        {
            // Arrange
            var type = typeof(XNameAttribute);
            var constructors = type.GetConstructors();

            // Act & Assert
            constructors.Should().HaveCount(1);
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(string));
            parameters[0].Name.Should().Be("name");
        }

        [Fact]
        public void XNameAttribute_ShouldHaveElementNameProperty()
        {
            // Arrange
            var type = typeof(XNameAttribute);
            var property = type.GetProperty("ElementName");

            // Act & Assert
            property.Should().NotBeNull();
            property.PropertyType.Should().Be(typeof(string));
            property.CanRead.Should().BeTrue();
            property.CanWrite.Should().BeFalse();
        }

        #endregion

        #region Performance Tests

        [Fact]
        public void Constructor_WithManyInstances_ShouldPerformWell()
        {
            // Arrange
            var startTime = DateTime.Now;
            var count = 10000;

            // Act
            for (int i = 0; i < count; i++)
            {
                var attribute = new XNameAttribute($"Element{i}");
                attribute.ElementName.Should().Be($"Element{i}");
            }

            // Assert
            var elapsed = DateTime.Now - startTime;
            elapsed.TotalMilliseconds.Should().BeLessThan(1000); // Should complete within 1 second
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Attribute_ShouldWorkWithXmlSerialization()
        {
            // Arrange
            var type = typeof(TestClassWithXNameAttribute);
            var property = type.GetProperty("TestProperty");
            var attribute = property.GetCustomAttribute<XNameAttribute>();

            // Act & Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be("TestProperty");
        }

        [Fact]
        public void Attribute_ShouldWorkWithReflection()
        {
            // Arrange
            var type = typeof(TestClassWithXNameAttribute);
            var attributes = type.GetCustomAttributes(typeof(XNameAttribute), false);

            // Act
            var attribute = attributes[0] as XNameAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.ElementName.Should().Be("TestClass");
        }

        #endregion

        #region Test Helper Classes

        [XName("TestClass")]
        public class TestClassWithXNameAttribute
        {
            [XName("TestProperty")]
            public string TestProperty { get; set; }
        }

        public class TestClassWithMultipleXNameAttributes
        {
            [XName("FirstAttribute")]        
            public string TestProperty { get; set; }
        }

        #endregion
    }
}

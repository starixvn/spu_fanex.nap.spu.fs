using Fanex.NAP.SPU.FS.JobService;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.ComponentModel;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.JobService
{
    public class AppSettingsTests
    {
        private readonly IConfiguration mockConfiguration;
        private readonly AppSettings appSettings;

        public AppSettingsTests()
        {
            mockConfiguration = Substitute.For<IConfiguration>();
            appSettings = new AppSettings(mockConfiguration);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();

            // Act
            var result = new AppSettings(configuration);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AppSettings>();
            result.Should().BeAssignableTo<IAppSettings>();
        }

        [Fact]
        public void Constructor_WithNullConfiguration_ShouldThrowInvalidCastException()
        {
            // Arrange
            IConfiguration configuration = null;

            // Act & Assert
            var exception = Assert.Throws<InvalidCastException>(() => new AppSettings(configuration));
            exception.Message.Should().Contain("Key: Environment");
        }

        #endregion

        #region Environment Property Tests

        [Theory]
        [InlineData("LOCAL", 1)]
        [InlineData("UAT", 2)]
        [InlineData("PROD", 3)]
        [InlineData("AWS", 3)]
        [InlineData("", 3)]
        [InlineData(null, 3)]
        [InlineData("INVALID", 3)]
        public void Environment_WithDifferentValues_ShouldReturnCorrectIntValue(string environmentValue, int expectedValue)
        {
            // Arrange
            mockConfiguration[nameof(AppSettings.Environment)].Returns(environmentValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.Environment;

            // Assert
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void Environment_ShouldCallConfigurationWithCorrectKey()
        {
            // Arrange
            mockConfiguration[nameof(AppSettings.Environment)].Returns("LOCAL");
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.Environment;

            // Assert
            result.Should().Be(1);
        }

        #endregion

        #region CustomerEvidenceAPI Property Tests

        [Theory]
        [InlineData("https://api.example.com")]
        [InlineData("https://api.test.com/evidence")]
        [InlineData("")]
        public void CustomerEvidenceAPI_WithDifferentValues_ShouldReturnCorrectValue(string expectedValue)
        {
            // Arrange
            mockConfiguration[nameof(AppSettings.CustomerEvidenceAPI)].Returns(expectedValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.CustomerEvidenceAPI;

            // Assert
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void CustomerEvidenceAPI_WithNullValue_ShouldReturnEmptyString()
        {
            // Arrange
            mockConfiguration[nameof(AppSettings.CustomerEvidenceAPI)].Returns((string)null);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.CustomerEvidenceAPI;

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void CustomerEvidenceAPI_ShouldCallConfigurationWithCorrectKey()
        {
            // Arrange
            var expectedValue = "https://api.example.com";
            mockConfiguration[nameof(AppSettings.CustomerEvidenceAPI)].Returns(expectedValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.CustomerEvidenceAPI;

            // Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region SPName Property Tests

        [Theory]
        [InlineData("GetCustomerData")]
        [InlineData("GetBettingHistory")]
        [InlineData("")]
        public void SPName_WithDifferentValues_ShouldReturnCorrectValue(string expectedValue)
        {
            // Arrange
            mockConfiguration[nameof(AppSettings.SPName)].Returns(expectedValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.SPName;

            // Assert
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void SPName_WithNullValue_ShouldReturnEmptyString()
        {
            // Arrange
            mockConfiguration[nameof(AppSettings.SPName)].Returns((string)null);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.SPName;

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void SPName_ShouldCallConfigurationWithCorrectKey()
        {
            // Arrange
            var expectedValue = "GetCustomerData";
            mockConfiguration[nameof(AppSettings.SPName)].Returns(expectedValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.SPName;

            // Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region SentryDsn Property Tests

        [Theory]
        [InlineData("https://sentry.io/123456")]
        [InlineData("https://sentry.example.com/dsn")]
        [InlineData("")]
        public void SentryDsn_WithDifferentValues_ShouldReturnCorrectValue(string expectedValue)
        {
            // Arrange
            mockConfiguration["Sentry:Dsn"].Returns(expectedValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.SentryDsn;

            // Assert
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void SentryDsn_WithNullValue_ShouldReturnEmptyString()
        {
            // Arrange
            mockConfiguration["Sentry:Dsn"].Returns((string)null);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.SentryDsn;

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void SentryDsn_ShouldCallConfigurationWithCorrectKey()
        {
            // Arrange
            var expectedValue = "https://sentry.io/123456";
            mockConfiguration["Sentry:Dsn"].Returns(expectedValue);
            var appSettings = new AppSettings(mockConfiguration);

            // Act
            var result = appSettings.SentryDsn;

            // Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region GetValue Method Tests

        [Theory]
        [InlineData("123", 123)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("3.14", 3.14)]
        [InlineData("test", "test")]
        [InlineData("", "")]
        public void GetValue_WithValidValues_ShouldReturnCorrectTypedValue<T>(string configValue, T expectedValue)
        {
            // Arrange
            var key = "TestKey";
            mockConfiguration[key].Returns(configValue);

            // Act
            var result = appSettings.GetValue<T>(key);

            // Assert
            result.Should().Be(expectedValue);
            result.Should().BeOfType<T>();
        }

        [Fact]
        public void GetValue_WithNullValue_ShouldThrowInvalidCastException()
        {
            // Arrange
            var key = "TestKey";
            mockConfiguration[key].Returns((string)null);

            // Act & Assert
            var exception = Assert.Throws<InvalidCastException>(() => appSettings.GetValue<int>(key));
            exception.Message.Should().Contain($"Key: {key}");
            exception.InnerException.Should().NotBeNull();
        }

        [Fact]
        public void GetValue_WithInvalidConversion_ShouldThrowInvalidCastException()
        {
            // Arrange
            var key = "TestKey";
            var invalidValue = "not-a-number";
            mockConfiguration[key].Returns(invalidValue);

            // Act & Assert
            var exception = Assert.Throws<InvalidCastException>(() => appSettings.GetValue<int>(key));
            exception.Message.Should().Contain($"Key: {key}");
            exception.InnerException.Should().NotBeNull();
        }

        [Fact]
        public void GetValue_ShouldCallConfigurationWithCorrectKey()
        {
            // Arrange
            var key = "TestKey";
            var value = "123";
            mockConfiguration[key].Returns(value);

            // Act
            var result = appSettings.GetValue<int>(key);

            // Assert
            result.Should().Be(123);
        }

        [Theory]
        [InlineData("TestKey1")]
        [InlineData("TestKey2")]
        [InlineData("Environment")]
        [InlineData("CustomerEvidenceAPI")]
        public void GetValue_WithDifferentKeys_ShouldCallConfigurationWithCorrectKey(string key)
        {
            // Arrange
            var value = "test-value";
            mockConfiguration[key].Returns(value);

            // Act
            var result = appSettings.GetValue<string>(key);

            // Assert
            result.Should().Be(value);
        }

        #endregion

        #region GetSection Method Tests

        [Fact]
        public void GetSection_WithValidKey_ShouldCallConfigurationGetSection()
        {
            // Arrange
            var key = "TestSection";
            var mockSection = Substitute.For<IConfigurationSection>();
            mockConfiguration.GetSection(key).Returns(mockSection);

            // Act
            appSettings.GetSection<object>(key);

            // Assert
            mockConfiguration.Received(1).GetSection(key);
        }

        [Fact]
        public void GetSection_WithNullKey_ShouldCallConfigurationWithNullKey()
        {
            // Arrange
            string key = null;
            var mockSection = Substitute.For<IConfigurationSection>();
            mockConfiguration.GetSection(key).Returns(mockSection);

            // Act
            appSettings.GetSection<object>(key);

            // Assert
            mockConfiguration.Received(1).GetSection(key);
        }

        [Theory]
        [InlineData("TestSection1")]
        [InlineData("TestSection2")]
        [InlineData("Sentry")]
        public void GetSection_WithDifferentKeys_ShouldCallConfigurationWithCorrectKey(string key)
        {
            // Arrange
            var mockSection = Substitute.For<IConfigurationSection>();
            mockConfiguration.GetSection(key).Returns(mockSection);

            // Act
            appSettings.GetSection<object>(key);

            // Assert
            mockConfiguration.Received(1).GetSection(key);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void AllProperties_WithCompleteConfiguration_ShouldReturnCorrectValues()
        {
            // Arrange
            var environmentValue = "UAT";
            var customerEvidenceAPIValue = "https://api.example.com/evidence";
            var spNameValue = "GetCustomerData";
            var sentryDsnValue = "https://sentry.io/123456";

            mockConfiguration[nameof(AppSettings.Environment)].Returns(environmentValue);
            mockConfiguration[nameof(AppSettings.CustomerEvidenceAPI)].Returns(customerEvidenceAPIValue);
            mockConfiguration[nameof(AppSettings.SPName)].Returns(spNameValue);
            mockConfiguration["Sentry:Dsn"].Returns(sentryDsnValue);

            var appSettings = new AppSettings(mockConfiguration);

            // Act & Assert
            appSettings.Environment.Should().Be(2); // UAT maps to 2
            appSettings.CustomerEvidenceAPI.Should().Be(customerEvidenceAPIValue);
            appSettings.SPName.Should().Be(spNameValue);
            appSettings.SentryDsn.Should().Be(sentryDsnValue);
        }

        [Fact]
        public void MultipleInstances_WithSameConfiguration_ShouldBeIndependent()
        {
            // Arrange
            var environmentValue = "LOCAL";
            mockConfiguration[nameof(AppSettings.Environment)].Returns(environmentValue);

            // Act
            var appSettings1 = new AppSettings(mockConfiguration);
            var appSettings2 = new AppSettings(mockConfiguration);

            // Assert
            appSettings1.Should().NotBeSameAs(appSettings2);
            appSettings1.Environment.Should().Be(appSettings2.Environment);
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void GetValue_WithEmptyString_ShouldHandleGracefully()
        {
            // Arrange
            var key = "EmptyKey";
            mockConfiguration[key].Returns(string.Empty);

            // Act
            var result = appSettings.GetValue<string>(key);

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void GetValue_WithWhitespaceString_ShouldHandleGracefully()
        {
            // Arrange
            var key = "WhitespaceKey";
            var whitespaceValue = "   ";
            mockConfiguration[key].Returns(whitespaceValue);

            // Act
            var result = appSettings.GetValue<string>(key);

            // Assert
            result.Should().Be(whitespaceValue);
        }

        [Fact]
        public void Environment_WithCaseSensitiveValues_ShouldHandleCorrectly()
        {
            // Arrange
            var testCases = new[]
            {
                ("local", 3),    // lowercase should default to 3
                ("uat", 3),      // lowercase should default to 3
                ("prod", 3),     // lowercase should default to 3
                ("LOCAL", 1),    // uppercase should work
                ("UAT", 2),      // uppercase should work
                ("PROD", 3)      // uppercase should work
            };

            foreach (var (environmentValue, expectedValue) in testCases)
            {
                // Arrange
                var config = Substitute.For<IConfiguration>();
                config[nameof(AppSettings.Environment)].Returns(environmentValue);
                var appSettings = new AppSettings(config);

                // Act
                var result = appSettings.Environment;

                // Assert
                result.Should().Be(expectedValue, $"Environment value '{environmentValue}' should map to {expectedValue}");
            }
        }

        #endregion
    }
}

using Fanex.NAP.SPU.FS.JobService;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.JobService
{
    public class StartupTests
    {
        private readonly IConfiguration mockConfiguration;
        private readonly Startup startup;

        public StartupTests()
        {
            mockConfiguration = Substitute.For<IConfiguration>();
            startup = new Startup(mockConfiguration);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();

            // Act
            var startup = new Startup(configuration);

            // Assert
            startup.Should().NotBeNull();
            startup.Configuration.Should().Be(configuration);
        }

        [Fact]
        public void Constructor_WithNullConfiguration_ShouldCreateInstance()
        {
            // Arrange
            IConfiguration configuration = null;

            // Act
            var startup = new Startup(configuration);

            // Assert
            startup.Should().NotBeNull();
            startup.Configuration.Should().BeNull();
        }

        [Fact]
        public void Configuration_Property_ShouldReturnProvidedConfiguration()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();

            // Act
            var startup = new Startup(configuration);

            // Assert
            startup.Configuration.Should().Be(configuration);
        }

        #endregion

        #region ConfigureServices Tests       

        [Fact]
        public void ConfigureServices_WithNullServices_ShouldThrowException()
        {
            // Arrange
            IServiceCollection services = null;
            var configuration = Substitute.For<IConfiguration>();
            var startup = new Startup(configuration);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => startup.ConfigureServices(services));
            exception.ParamName.Should().Be("services");
        }
      
        #endregion       

        #region Class Structure Tests

        [Fact]
        public void Startup_ShouldBePublicClass()
        {
            // Arrange & Act
            var startupType = typeof(Startup);

            // Assert
            startupType.IsPublic.Should().BeTrue();
            startupType.IsClass.Should().BeTrue();
        }

        [Fact]
        public void Startup_ShouldHaveConstructor()
        {
            // Arrange & Act
            var startupType = typeof(Startup);
            var constructors = startupType.GetConstructors();

            // Assert
            constructors.Should().HaveCount(1);
            constructors[0].GetParameters().Should().HaveCount(1);
            constructors[0].GetParameters()[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ShouldHaveConfigurationProperty()
        {
            // Arrange & Act
            var startupType = typeof(Startup);
            var configurationProperty = startupType.GetProperty("Configuration");

            // Assert
            configurationProperty.Should().NotBeNull();
            configurationProperty.PropertyType.Should().Be(typeof(IConfiguration));
            configurationProperty.CanRead.Should().BeTrue();
        }

        [Fact]
        public void Startup_ShouldHaveConfigureServicesMethod()
        {
            // Arrange & Act
            var startupType = typeof(Startup);
            var configureServicesMethod = startupType.GetMethod("ConfigureServices");

            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureServicesMethod.IsPublic.Should().BeTrue();
            configureServicesMethod.ReturnType.Should().Be(typeof(void));
            configureServicesMethod.GetParameters().Should().HaveCount(1);
            configureServicesMethod.GetParameters()[0].ParameterType.Should().Be(typeof(IServiceCollection));
        }

        [Fact]
        public void Startup_ShouldHaveConfigureMethod()
        {
            // Arrange & Act
            var startupType = typeof(Startup);
            var configureMethod = startupType.GetMethod("Configure");

            // Assert
            configureMethod.Should().NotBeNull();
            configureMethod.IsPublic.Should().BeTrue();
            configureMethod.ReturnType.Should().Be(typeof(void));
            configureMethod.GetParameters().Should().HaveCount(2);
            configureMethod.GetParameters()[0].ParameterType.Should().Be(typeof(IApplicationBuilder));
            configureMethod.GetParameters()[1].ParameterType.Should().Be(typeof(IWebHostEnvironment));
        }

        #endregion       
    }
}

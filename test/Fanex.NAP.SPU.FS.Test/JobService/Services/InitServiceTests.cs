using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
using Fanex.NAP.SPU.FS.Features.DelayedGoal;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
using Fanex.NAP.SPU.FS.JobService;
using Fanex.NAP.SPU.FS.JobService.Services;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.JobService.Services
{
    public class InitServiceTests
    {
        private readonly ServiceCollection serviceCollection;

        public InitServiceTests()
        {
            serviceCollection = new ServiceCollection();
        }

        #region RegisterService Extension Method Tests

        [Fact]
        public void RegisterService_WithValidServiceCollection_ShouldRegisterAllServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            services.Should().NotBeNull();
            services.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void RegisterService_WithEmptyServiceCollection_ShouldAddServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var initialCount = services.Count;

            // Act
            services.RegisterService();

            // Assert
            services.Count.Should().BeGreaterThan(initialCount);
        }

        [Fact]
        public void RegisterService_WithNullServiceCollection_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceCollection services = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => services.RegisterService());
            exception.ParamName.Should().Be("services");
        }

        #endregion

        #region Base Services Registration Tests

        [Fact]
        public void RegisterBaseServices_ShouldRegisterFraudCaseDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IFraudCaseDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(FraudCaseDataAccess));
        }

        [Fact]
        public void RegisterBaseServices_ShouldRegisterFraudCaseServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IFraudCaseServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(FraudCaseServices));
        }

        #endregion

        #region BigStateNearGoal Service Registration Tests

        [Fact]
        public void RegisterBigStateNearGoalService_ShouldRegisterDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IBigStateNearGoalDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(BigStateNearGoalDataAccess));
        }

        [Fact]
        public void RegisterBigStateNearGoalService_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(BigStateNearGoalServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(BigStateNearGoalServices));
        }

        [Fact]
        public void RegisterBigStateNearGoalService_ShouldRegisterJobService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IBigStateNearGoalJobService));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(BigStateNearGoalJobService));
        }

        #endregion

        #region CheckBet Service Registration Tests

        [Fact]
        public void RegisterCheckBetService_ShouldRegisterDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ICheckBetDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(CheckBetDataAccess));
        }

        [Fact]
        public void RegisterCheckBetService_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(CheckBetServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(CheckBetServices));
        }

        [Fact]
        public void RegisterCheckBetService_ShouldRegisterJobService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ICheckBetJobService));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(CheckBetJobService));
        }

        #endregion

        #region DelayedGoal Service Registration Tests

        [Fact]
        public void RegisterDelayedGoalService_ShouldRegisterDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDelayedGoalDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(DelayedGoalDataAccess));
        }

        [Fact]
        public void RegisterDelayedGoalService_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(DelayedGoalServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(DelayedGoalServices));
        }

        [Fact]
        public void RegisterDelayedGoalService_ShouldRegisterJobService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDelayedGoalJobService));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(DelayedGoalJobService));
        }

        #endregion

        #region FlaggedAccount Service Registration Tests

        [Fact]
        public void RegisterFlaggedAccountService_ShouldRegisterDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IFlaggedAccountDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(FlaggedAccountDataAccess));
        }

        [Fact]
        public void RegisterFlaggedAccountService_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(FlaggedAccountServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(FlaggedAccountServices));
        }

        [Fact]
        public void RegisterFlaggedAccountService_ShouldRegisterJobService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IFlaggedAccountJobService));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(FlaggedAccountJobService));
        }

        #endregion

        #region DailyDataCapture Service Registration Tests

        [Fact]
        public void RegisterDailyCaptureService_ShouldRegisterDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDailyDataCaptureDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(DailyDataCaptureDataAccess));
        }

        [Fact]
        public void RegisterDailyCaptureService_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(DailyDataCaptureServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(DailyDataCaptureServices));
        }

        [Fact]
        public void RegisterDailyCaptureService_ShouldRegisterJobService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDailyCaptureDataJobService));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(DailyCaptureDataJobService));
        }

        #endregion

        #region ScannedOfficeIP Service Registration Tests

        [Fact]
        public void RegisterScannedOfficeIpService_ShouldRegisterDataAccess()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IScannedOfficeIPDataAccess));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(ScannedOfficeIPDataAccess));
        }

        [Fact]
        public void RegisterScannedOfficeIpService_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ScannedOfficeIPServices));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(ScannedOfficeIPServices));
        }

        [Fact]
        public void RegisterScannedOfficeIpService_ShouldRegisterJobService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IScannedOfficeIPJobService));
            serviceDescriptor.Should().NotBeNull();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            serviceDescriptor.ImplementationType.Should().Be(typeof(ScannedOfficeIPJobService));
        }

        #endregion

        #region Service Lifetime Tests

        [Fact]
        public void RegisterService_ShouldRegisterAllServicesAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            var allServices = services.Where(s => s.ServiceType.Namespace?.Contains("Fanex.NAP.SPU.FS") == true);
            allServices.Should().NotBeEmpty();
            allServices.Should().OnlyContain(s => s.Lifetime == ServiceLifetime.Singleton);
        }

        #endregion

        #region Service Count Tests

        [Fact]
        public void RegisterService_ShouldRegisterExpectedNumberOfServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();

            // Assert
            // Expected services: 2 base + 3 per feature (6 features) = 2 + 18 = 20 services
            var fanexServices = services.Where(s => s.ServiceType.Namespace?.Contains("Fanex.NAP.SPU.FS") == true);
            fanexServices.Should().HaveCount(20);
        }

        [Fact]
        public void RegisterService_ShouldRegisterAllRequiredServiceTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            var expectedServiceTypes = new[]
            {
                typeof(IFraudCaseDataAccess),
                typeof(IFraudCaseServices),
                typeof(IBigStateNearGoalDataAccess),
                typeof(BigStateNearGoalServices),
                typeof(IBigStateNearGoalJobService),
                typeof(ICheckBetDataAccess),
                typeof(CheckBetServices),
                typeof(ICheckBetJobService),
                typeof(IDelayedGoalDataAccess),
                typeof(DelayedGoalServices),
                typeof(IDelayedGoalJobService),
                typeof(IFlaggedAccountDataAccess),
                typeof(FlaggedAccountServices),
                typeof(IFlaggedAccountJobService),
                typeof(IDailyDataCaptureDataAccess),
                typeof(DailyDataCaptureServices),
                typeof(IDailyCaptureDataJobService),
                typeof(IScannedOfficeIPDataAccess),
                typeof(ScannedOfficeIPServices),
                typeof(IScannedOfficeIPJobService)
            };

            // Act
            services.RegisterService();

            // Assert
            foreach (var expectedType in expectedServiceTypes)
            {
                var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == expectedType);
                serviceDescriptor.Should().NotBeNull($"Service {expectedType.Name} should be registered");
            }
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void RegisterService_WithMultipleCalls_ShouldAddDuplicateServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.RegisterService();
            services.RegisterService();

            // Assert
            var fraudCaseDataAccessServices = services.Where(s => s.ServiceType == typeof(IFraudCaseDataAccess));
            fraudCaseDataAccessServices.Should().HaveCount(2); // Multiple calls add duplicates
        }

        [Fact]
        public void RegisterService_ShouldAddMoreServicesOnMultipleCalls()
        {
            // Arrange
            var services1 = new ServiceCollection();
            var services2 = new ServiceCollection();

            // Act
            services1.RegisterService();
            services2.RegisterService();
            services2.RegisterService(); // Call twice

            // Assert
            var services1Count = services1.Count(s => s.ServiceType.Namespace?.Contains("Fanex.NAP.SPU.FS") == true);
            var services2Count = services2.Count(s => s.ServiceType.Namespace?.Contains("Fanex.NAP.SPU.FS") == true);
            services2Count.Should().Be(services1Count * 2); // Second call doubles the services
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public void RegisterService_WithExistingServices_ShouldAddNewServices()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IAppSettings, AppSettings>();

            // Act
            services.RegisterService();

            // Assert
            services.Should().Contain(s => s.ServiceType == typeof(IAppSettings));
            services.Should().Contain(s => s.ServiceType == typeof(IFraudCaseDataAccess));
        }

        [Fact]
        public void RegisterService_ShouldNotThrowException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            var exception = Record.Exception(() => services.RegisterService());
            exception.Should().BeNull();
        }

        #endregion
    }
}

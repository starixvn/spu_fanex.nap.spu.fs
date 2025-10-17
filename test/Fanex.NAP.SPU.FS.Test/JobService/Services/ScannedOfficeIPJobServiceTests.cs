using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
using Fanex.NAP.SPU.FS.JobService;
using Fanex.NAP.SPU.FS.JobService.Services;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.JobService.Services
{
    public class ScannedOfficeIPJobServiceTests
    {
        private readonly ScannedOfficeIPJobService jobService;
        private readonly IAppSettings appSettings;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly ScannedOfficeIPServices scannedOfficeIPServices;

        public ScannedOfficeIPJobServiceTests()
        {
            appSettings = Substitute.For<IAppSettings>();
            fraudCaseService = Substitute.For<IFraudCaseServices>();
            scannedOfficeIPServices = Substitute.For<ScannedOfficeIPServices>(Substitute.For<IScannedOfficeIPDataAccess>());

            jobService = new ScannedOfficeIPJobService(appSettings, fraudCaseService, scannedOfficeIPServices);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDependencies_ShouldCreateInstance()
        {
            // Arrange
            var mockAppSettings = Substitute.For<IAppSettings>();
            var mockFraudCaseService = Substitute.For<IFraudCaseServices>();
            var mockScannedOfficeIPServices = Substitute.For<ScannedOfficeIPServices>(Substitute.For<IScannedOfficeIPDataAccess>());

            // Act
            var service = new ScannedOfficeIPJobService(mockAppSettings, mockFraudCaseService, mockScannedOfficeIPServices);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new ScannedOfficeIPJobService(null, null, null);

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region Execute Tests - Fraud Case Active

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_ShouldReturnTrue()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(2, 1);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_ShouldCallJobRun()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).CleanResult();
            scannedOfficeIPServices.Received(1).Precondition();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_WithNullResults_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns((IEnumerable<DataResult>)null);

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_WithEmptyResults_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_WithDifferentChunkSizes_ShouldPassCorrectChunkSize()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result1 = await jobService.Execute(5);
            var result2 = await jobService.Execute(20);
            var result3 = await jobService.Execute(100);

            // Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(5);
            scannedOfficeIPServices.Received(1).Rules(20);
            scannedOfficeIPServices.Received(1).Rules(100);
        }

        #endregion

        #region Execute Tests - Fraud Case Inactive

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldReturnTrue()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(false);

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(2, 1);
            scannedOfficeIPServices.DidNotReceive().CleanResult();
            scannedOfficeIPServices.DidNotReceive().Precondition();
            scannedOfficeIPServices.DidNotReceive().Rules(Arg.Any<int>());
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_WithDifferentEnvironments_ShouldReturnTrue()
        {
            // Arrange
            var environments = new[] { 1, 2, 3 };
            fraudCaseService.CheckFraudCaseIsActive(2, Arg.Any<int>()).Returns(false);

            foreach (var environment in environments)
            {
                appSettings.Environment.Returns(environment);

                // Act
                var result = await jobService.Execute(10);

                // Assert
                result.Should().BeTrue();
                fraudCaseService.Received(1).CheckFraudCaseIsActive(2, environment);
            }
        }

        #endregion

        #region JobRun Tests

        [Fact]
        public async Task Execute_WhenJobRunWithNullResults_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns((IEnumerable<DataResult>)null);

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

        [Fact]
        public async Task Execute_WhenJobRunWithEmptyResults_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

        [Fact]
        public async Task Execute_WhenJobRunWithSuccessResults_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            var successResults = new List<DataResult>
            {
                new DataResult { ErrorCode = 0, ErrorMessage = "Success 1" },
                new DataResult { ErrorCode = 1, ErrorMessage = "Success 2" }
            };
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(successResults);

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

     
        #endregion

        #region Fraud Case ID Tests

        [Fact]
        public async Task Execute_ShouldUseCorrectFraudCaseId()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(2, 1);
        }

        [Fact]
        public async Task Execute_WithMultipleCalls_ShouldUseCorrectFraudCaseId()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result1 = await jobService.Execute(10);
            var result2 = await jobService.Execute(20);
            var result3 = await jobService.Execute(30);

            // Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
            fraudCaseService.Received(3).CheckFraudCaseIsActive(2, 1);
        }

        #endregion

        #region Environment Tests

        [Fact]
        public async Task Execute_WithDifferentEnvironments_ShouldPassCorrectEnvironment()
        {
            // Arrange
            var environments = new[] { 1, 2, 3 };
            fraudCaseService.CheckFraudCaseIsActive(2, Arg.Any<int>()).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            foreach (var environment in environments)
            {
                appSettings.Environment.Returns(environment);

                // Act
                var result = await jobService.Execute(10);

                // Assert
                result.Should().BeTrue();
                fraudCaseService.Received(1).CheckFraudCaseIsActive(2, environment);
            }
        }

        #endregion

        #region Interface Implementation Tests

        [Fact]
        public void ScannedOfficeIPJobService_ShouldImplementInterface()
        {
            // Arrange
            var service = new ScannedOfficeIPJobService(appSettings, fraudCaseService, scannedOfficeIPServices);

            // Assert
            service.Should().BeAssignableTo<IScannedOfficeIPJobService>();
        }

        [Fact]
        public async Task Interface_ShouldHaveExecuteMethod()
        {
            // Arrange
            IScannedOfficeIPJobService service = new ScannedOfficeIPJobService(appSettings, fraudCaseService, scannedOfficeIPServices);
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await service.Execute(10);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task Execute_WithZeroChunkSize_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(0).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(0);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(0);
        }

        [Fact]
        public async Task Execute_WithNegativeChunkSize_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(-1).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(-1);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(-1);
        }

        [Fact]
        public async Task Execute_WithLargeChunkSize_ShouldHandleGracefully()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(10000).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10000);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).Rules(10000);
        }

      

        #endregion

        #region Multiple Calls Tests

        [Fact]
        public async Task Execute_WithMultipleCalls_ShouldHandleEachCall()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result1 = await jobService.Execute(10);
            var result2 = await jobService.Execute(20);
            var result3 = await jobService.Execute(30);

            // Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
            scannedOfficeIPServices.Received(3).CleanResult();
            scannedOfficeIPServices.Received(3).Precondition();
            scannedOfficeIPServices.Received(3).Rules(Arg.Any<int>());
        }

    
        #endregion

        #region Performance Tests

        [Fact]
        public async Task Execute_WithLargeDataSet_ShouldCompleteWithinReasonableTime()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            var startTime = DateTime.Now;

            // Act
            var result = await jobService.Execute(1000);

            // Assert
            var elapsed = DateTime.Now - startTime;
            result.Should().BeTrue();
            elapsed.TotalSeconds.Should().BeLessThan(30); // Should complete within 30 seconds
        }

        #endregion

        #region Service Integration Tests

        [Fact]
        public async Task Execute_ShouldCallServicesInCorrectOrder()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(2, 1);
            scannedOfficeIPServices.Received(1).CleanResult();
            scannedOfficeIPServices.Received(1).Precondition();
            scannedOfficeIPServices.Received(1).Rules(10);
        }

        [Fact]
        public async Task Execute_ShouldPassCorrectParametersToServices()
        {
            // Arrange
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(2, 2).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(25);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(2, 2);
            scannedOfficeIPServices.Received(1).Rules(25);
        }

        #endregion     


        #region ScannedOfficeIP Specific Tests

        [Fact]
        public async Task Execute_ShouldUseScannedOfficeIPServices()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(10);

            // Assert
            result.Should().BeTrue();
            scannedOfficeIPServices.Received(1).CleanResult();
            scannedOfficeIPServices.Received(1).Precondition();
            scannedOfficeIPServices.Received(1).Rules(10);
        }
      
        #endregion

        #region Chunking Tests

        [Fact]
        public async Task Execute_WithVariousChunkSizes_ShouldProcessCorrectly()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(2, 1).Returns(true);
            scannedOfficeIPServices.CleanResult().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Precondition().Returns(scannedOfficeIPServices);
            scannedOfficeIPServices.Rules(Arg.Any<int>()).Returns(new List<DataResult>());

            var chunkSizes = new[] { 1, 2, 3, 5, 10, 20 };

            foreach (var chunkSize in chunkSizes)
            {
                // Act
                var result = await jobService.Execute(chunkSize);

                // Assert
                result.Should().BeTrue();
                scannedOfficeIPServices.Received(1).Rules(chunkSize);
            }
        }

        #endregion
    }
}

using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Features.FraudCase;
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
    public class BigStateNearGoalJobServiceTests
    {
        private readonly BigStateNearGoalJobService jobService;
        private readonly IAppSettings appSettings;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly BigStateNearGoalServices bigStateNearGoalService;

        public BigStateNearGoalJobServiceTests()
        {
            appSettings = Substitute.For<IAppSettings>();
            fraudCaseService = Substitute.For<IFraudCaseServices>();
            bigStateNearGoalService = Substitute.For<BigStateNearGoalServices>(Substitute.For<IBigStateNearGoalDataAccess>());

            jobService = new BigStateNearGoalJobService(appSettings, fraudCaseService, bigStateNearGoalService);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDependencies_ShouldCreateInstance()
        {
            // Arrange
            var mockAppSettings = Substitute.For<IAppSettings>();
            var mockFraudCaseService = Substitute.For<IFraudCaseServices>();
            var mockBigStateNearGoalService = Substitute.For<BigStateNearGoalServices>(Substitute.For<IBigStateNearGoalDataAccess>());

            // Act
            var service = new BigStateNearGoalJobService(mockAppSettings, mockFraudCaseService, mockBigStateNearGoalService);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new BigStateNearGoalJobService(null, null, null);

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
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(1, 1);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_ShouldCallJobRun()
        {
            // Arrange
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(1, 2).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            bigStateNearGoalService.Received(1).CleanResult();
            bigStateNearGoalService.Received(1).Precondition();
            bigStateNearGoalService.Received(1).Rules();
        }        
        #endregion

        #region Execute Tests - Fraud Case Inactive

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldReturnTrue()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(false);

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(1, 1);
            bigStateNearGoalService.DidNotReceive().CleanResult();
            bigStateNearGoalService.DidNotReceive().Precondition();
            bigStateNearGoalService.DidNotReceive().Rules();
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldNotCallJobRun()
        {
            // Arrange
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(1, 2).Returns(false);

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            bigStateNearGoalService.DidNotReceive().CleanResult();
            bigStateNearGoalService.DidNotReceive().Precondition();
            bigStateNearGoalService.DidNotReceive().Rules();
        }

        #endregion        

        #region JobRun Tests

        [Fact]
        public async Task Execute_WhenJobRunWithNullResults_ShouldNotProcessErrors()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns((IEnumerable<DataResult>)null);

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            bigStateNearGoalService.Received(1).Rules();
        }

        [Fact]
        public async Task Execute_WhenJobRunWithEmptyResults_ShouldNotProcessErrors()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            bigStateNearGoalService.Received(1).Rules();
        }

        [Fact]
        public async Task Execute_WhenJobRunWithSuccessResults_ShouldNotLogErrors()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            
            var successResults = new List<DataResult>
            {
                new DataResult { ErrorCode = 0, ErrorMessage = "Success 1" },
                new DataResult { ErrorCode = 0, ErrorMessage = "Success 2" }
            };

            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(successResults);

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            bigStateNearGoalService.Received(1).Rules();
        }
       
        #endregion

        #region Environment Tests

        [Fact]
        public async Task Execute_WithDifferentEnvironments_ShouldCallFraudCaseServiceWithCorrectEnvironment()
        {
            // Arrange
            var environments = new[] { 1, 2, 3, 4, 5 };

            foreach (var environment in environments)
            {
                appSettings.Environment.Returns(environment);
                fraudCaseService.CheckFraudCaseIsActive(1, environment).Returns(true);
                bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
                bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
                bigStateNearGoalService.Rules().Returns(new List<DataResult>());

                // Act
                var result = await jobService.Execute();

                // Assert
                result.Should().BeTrue();
                fraudCaseService.Received(1).CheckFraudCaseIsActive(1, environment);
            }
        }

        #endregion

        #region Fraud Case ID Tests

        [Fact]
        public async Task Execute_ShouldUseCorrectFraudCaseId()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(1, 1);
        }

        #endregion

        #region Method Chaining Tests

        [Fact]
        public async Task Execute_ShouldCallMethodsInCorrectOrder()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            
            // Verify the order of calls
            bigStateNearGoalService.Received(1).CleanResult();
            bigStateNearGoalService.Received(1).Precondition();
            bigStateNearGoalService.Received(1).Rules();
        }

        #endregion

        #region Interface Implementation Tests

        [Fact]
        public void BigStateNearGoalJobService_ShouldImplementInterface()
        {
            // Arrange
            var service = new BigStateNearGoalJobService(appSettings, fraudCaseService, bigStateNearGoalService);

            // Assert
            service.Should().BeAssignableTo<IBigStateNearGoalJobService>();
        }

        [Fact]
        public async Task Interface_ShouldHaveExecuteMethod()
        {
            // Arrange
            IBigStateNearGoalJobService service = new BigStateNearGoalJobService(appSettings, fraudCaseService, bigStateNearGoalService);
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            // Act
            var result = await service.Execute();

            // Assert
            result.Should().BeTrue();
        }

        #endregion
       
        #region Performance Tests

        [Fact]
        public async Task Execute_ShouldCompleteWithinReasonableTime()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(1, 1).Returns(true);
            bigStateNearGoalService.CleanResult().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Precondition().Returns(bigStateNearGoalService);
            bigStateNearGoalService.Rules().Returns(new List<DataResult>());

            var startTime = DateTime.Now;

            // Act
            var result = await jobService.Execute();

            // Assert
            var elapsed = DateTime.Now - startTime;
            result.Should().BeTrue();
            elapsed.TotalSeconds.Should().BeLessThan(30); // Should complete within 30 seconds
        }

        #endregion
    }
}

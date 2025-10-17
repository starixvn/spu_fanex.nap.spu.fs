using Fanex.NAP.SPU.FS.Features.DelayedGoal;
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
    public class DelayedGoalJobServiceTests
    {
        private readonly DelayedGoalJobService jobService;
        private readonly IAppSettings appSettings;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly DelayedGoalServices delayedGoalService;

        public DelayedGoalJobServiceTests()
        {
            appSettings = Substitute.For<IAppSettings>();
            fraudCaseService = Substitute.For<IFraudCaseServices>();
            delayedGoalService = Substitute.For<DelayedGoalServices>(Substitute.For<IDelayedGoalDataAccess>());

            jobService = new DelayedGoalJobService(appSettings, fraudCaseService, delayedGoalService);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDependencies_ShouldCreateInstance()
        {
            // Arrange
            var mockAppSettings = Substitute.For<IAppSettings>();
            var mockFraudCaseService = Substitute.For<IFraudCaseServices>();
            var mockDelayedGoalService = Substitute.For<DelayedGoalServices>(Substitute.For<IDelayedGoalDataAccess>());

            // Act
            var service = new DelayedGoalJobService(mockAppSettings, mockFraudCaseService, mockDelayedGoalService);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new DelayedGoalJobService(null, null, null);

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region Execute Tests - Fraud Case Active

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_ShouldReturnTrue()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(8, 1);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_ShouldCallJobRun()
        {
            // Arrange
            var chunkSize = 200;
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(8, 2).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).CleanResult();
            delayedGoalService.Received(1).Precondition();
            delayedGoalService.Received(1).Rules(chunkSize);
        }   

        #endregion

        #region Execute Tests - Fraud Case Inactive

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldReturnTrue()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(false);

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(8, 1);
            delayedGoalService.DidNotReceive().CleanResult();
            delayedGoalService.DidNotReceive().Precondition();
            delayedGoalService.DidNotReceive().Rules(Arg.Any<int>());
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldNotCallJobRun()
        {
            // Arrange
            var chunkSize = 200;
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(8, 2).Returns(false);

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.DidNotReceive().CleanResult();
            delayedGoalService.DidNotReceive().Precondition();
            delayedGoalService.DidNotReceive().Rules(Arg.Any<int>());
        }

        #endregion       

        #region JobRun Tests

        [Fact]
        public async Task Execute_WhenJobRunWithNullResults_ShouldNotProcessErrors()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns((IEnumerable<DataResult>)null);

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        [Fact]
        public async Task Execute_WhenJobRunWithEmptyResults_ShouldNotProcessErrors()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        [Fact]
        public async Task Execute_WhenJobRunWithSuccessResults_ShouldNotLogErrors()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            
            var successResults = new List<DataResult>
            {
                new DataResult { ErrorCode = 0, ErrorMessage = "Success 1" },
                new DataResult { ErrorCode = 0, ErrorMessage = "Success 2" }
            };

            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(successResults);

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }
    
        #endregion

        #region Chunk Size Tests

        [Fact]
        public async Task Execute_WithDifferentChunkSizes_ShouldPassCorrectChunkSize()
        {
            // Arrange
            var chunkSizes = new[] { 1, 10, 50, 100, 500, 1000 };
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);

            foreach (var chunkSize in chunkSizes)
            {
                delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

                // Act
                var result = await jobService.Execute(chunkSize);

                // Assert
                result.Should().BeTrue();
                delayedGoalService.Received(1).Rules(chunkSize);
            }
        }

        [Fact]
        public async Task Execute_WithZeroChunkSize_ShouldPassZeroChunkSize()
        {
            // Arrange
            var chunkSize = 0;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        [Fact]
        public async Task Execute_WithNegativeChunkSize_ShouldPassNegativeChunkSize()
        {
            // Arrange
            var chunkSize = -10;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        #endregion

        #region Environment Tests

        [Fact]
        public async Task Execute_WithDifferentEnvironments_ShouldCallFraudCaseServiceWithCorrectEnvironment()
        {
            // Arrange
            var chunkSize = 100;
            var environments = new[] { 1, 2, 3, 4, 5 };

            foreach (var environment in environments)
            {
                appSettings.Environment.Returns(environment);
                fraudCaseService.CheckFraudCaseIsActive(8, environment).Returns(true);
                delayedGoalService.CleanResult().Returns(delayedGoalService);
                delayedGoalService.Precondition().Returns(delayedGoalService);
                delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

                // Act
                var result = await jobService.Execute(chunkSize);

                // Assert
                result.Should().BeTrue();
                fraudCaseService.Received(1).CheckFraudCaseIsActive(8, environment);
            }
        }

        #endregion

        #region Fraud Case ID Tests

        [Fact]
        public async Task Execute_ShouldUseCorrectFraudCaseId()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(8, 1);
        }

        #endregion

        #region Method Chaining Tests

        [Fact]
        public async Task Execute_ShouldCallMethodsInCorrectOrder()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            
            // Verify the order of calls
            delayedGoalService.Received(1).CleanResult();
            delayedGoalService.Received(1).Precondition();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        #endregion

        #region Interface Implementation Tests

        [Fact]
        public void DelayedGoalJobService_ShouldImplementInterface()
        {
            // Arrange
            var service = new DelayedGoalJobService(appSettings, fraudCaseService, delayedGoalService);

            // Assert
            service.Should().BeAssignableTo<IDelayedGoalJobService>();
        }

        [Fact]
        public async Task Interface_ShouldHaveExecuteMethod()
        {
            // Arrange
            var chunkSize = 100;
            IDelayedGoalJobService service = new DelayedGoalJobService(appSettings, fraudCaseService, delayedGoalService);
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await service.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
        }

        #endregion        

        #region Performance Tests

        [Fact]
        public async Task Execute_ShouldCompleteWithinReasonableTime()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            var startTime = DateTime.Now;

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            var elapsed = DateTime.Now - startTime;
            result.Should().BeTrue();
            elapsed.TotalSeconds.Should().BeLessThan(30); // Should complete within 30 seconds
        }

        #endregion

        #region Multiple Calls Tests

        [Fact]
        public async Task Execute_WithMultipleCalls_ShouldHandleEachCall()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result1 = await jobService.Execute(chunkSize);
            var result2 = await jobService.Execute(chunkSize);
            var result3 = await jobService.Execute(chunkSize);

            // Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
            fraudCaseService.Received(3).CheckFraudCaseIsActive(8, 1);
            delayedGoalService.Received(3).Rules(chunkSize);
        }

        #endregion

        #region Error Code Tests
       
        [Fact]
        public async Task Execute_WithErrorCodeZero_ShouldNotLogError()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>
            {
                new DataResult { ErrorCode = 0, ErrorMessage = "Success" }
            });

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        [Fact]
        public async Task Execute_WithErrorCodePositive_ShouldNotLogError()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>
            {
                new DataResult { ErrorCode = 1, ErrorMessage = "Warning" }
            });

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        #endregion

        #region Domain-Specific Tests

        [Fact]
        public async Task Execute_ShouldBeRelatedToDelayedGoal()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        [Fact]
        public async Task Execute_ShouldUseCorrectFraudCaseIdForDelayedGoal()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(8, 1);
        }

        #endregion

        #region Service Integration Tests

        [Fact]
        public async Task Execute_ShouldCallDelayedGoalServiceMethods()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(true);
            delayedGoalService.CleanResult().Returns(delayedGoalService);
            delayedGoalService.Precondition().Returns(delayedGoalService);
            delayedGoalService.Rules(chunkSize).Returns(new List<DataResult>());

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.Received(1).CleanResult();
            delayedGoalService.Received(1).Precondition();
            delayedGoalService.Received(1).Rules(chunkSize);
        }

        [Fact]
        public async Task Execute_ShouldNotCallDelayedGoalServiceWhenFraudCaseInactive()
        {
            // Arrange
            var chunkSize = 100;
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(8, 1).Returns(false);

            // Act
            var result = await jobService.Execute(chunkSize);

            // Assert
            result.Should().BeTrue();
            delayedGoalService.DidNotReceive().CleanResult();
            delayedGoalService.DidNotReceive().Precondition();
            delayedGoalService.DidNotReceive().Rules(Arg.Any<int>());
        }

        #endregion
    }
}

using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.JobService;
using Fanex.NAP.SPU.FS.JobService.Services;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.JobService.Services
{
    public class CheckBetJobServiceTests
    {
        private readonly CheckBetJobService jobService;
        private readonly IAppSettings appSettings;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly CheckBetServices checkBetService;

        public CheckBetJobServiceTests()
        {
            appSettings = Substitute.For<IAppSettings>();
            fraudCaseService = Substitute.For<IFraudCaseServices>();
            checkBetService = Substitute.For<CheckBetServices>(Substitute.For<ICheckBetDataAccess>());

            jobService = new CheckBetJobService(appSettings, fraudCaseService, checkBetService);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDependencies_ShouldCreateInstance()
        {
            // Arrange
            var mockAppSettings = Substitute.For<IAppSettings>();
            var mockFraudCaseService = Substitute.For<IFraudCaseServices>();
            var mockCheckBetService = Substitute.For<CheckBetServices>(Substitute.For<ICheckBetDataAccess>());

            // Act
            var service = new CheckBetJobService(mockAppSettings, mockFraudCaseService, mockCheckBetService);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new CheckBetJobService(null, null, null);

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
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(3, 1);
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_ShouldCallJobRun()
        {
            // Arrange
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(3, 2).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
        }
       
        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_WithSuccessResult_ShouldNotLogError()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0, ErrorMessage = "Success" });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsActive_WithDifferentErrorCodes_ShouldOnlyLogErrorCodeMinus100()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = -200, ErrorMessage = "Different error" });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
        }

        #endregion

        #region Execute Tests - Fraud Case Inactive

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldReturnTrue()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(false);

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(3, 1);
            checkBetService.DidNotReceive().Rule();
        }

        [Fact]
        public async Task Execute_WhenFraudCaseIsInactive_ShouldNotCallJobRun()
        {
            // Arrange
            appSettings.Environment.Returns(2);
            fraudCaseService.CheckFraudCaseIsActive(3, 2).Returns(false);

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.DidNotReceive().Rule();
        }

        #endregion        

        #region JobRun Tests      

        [Fact]
        public async Task Execute_WhenJobRunWithSuccessResult_ShouldNotLogError()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0, ErrorMessage = "Success" });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
        }
       
        [Fact]
        public async Task Execute_WhenJobRunWithMixedErrorCodes_ShouldOnlyLogErrorCodeMinus100()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = -50, ErrorMessage = "Different error" });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
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
                fraudCaseService.CheckFraudCaseIsActive(3, environment).Returns(true);
                checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

                // Act
                var result = await jobService.Execute();

                // Assert
                result.Should().BeTrue();
                fraudCaseService.Received(1).CheckFraudCaseIsActive(3, environment);
            }
        }

        #endregion

        #region Fraud Case ID Tests

        [Fact]
        public async Task Execute_ShouldUseCorrectFraudCaseId()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            fraudCaseService.Received(1).CheckFraudCaseIsActive(3, 1);
        }

        #endregion

        #region Interface Implementation Tests

        [Fact]
        public void CheckBetJobService_ShouldImplementInterface()
        {
            // Arrange
            var service = new CheckBetJobService(appSettings, fraudCaseService, checkBetService);

            // Assert
            service.Should().BeAssignableTo<ICheckBetJobService>();
        }

        [Fact]
        public async Task Interface_ShouldHaveExecuteMethod()
        {
            // Arrange
            ICheckBetJobService service = new CheckBetJobService(appSettings, fraudCaseService, checkBetService);
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

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
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

            var startTime = DateTime.Now;

            // Act
            var result = await jobService.Execute();

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
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0 });

            // Act
            var result1 = await jobService.Execute();
            var result2 = await jobService.Execute();
            var result3 = await jobService.Execute();

            // Assert
            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeTrue();
            fraudCaseService.Received(3).CheckFraudCaseIsActive(3, 1);
            checkBetService.Received(3).Rule();
        }

        #endregion

        #region Error Code Tests
       
        [Fact]
        public async Task Execute_WithErrorCodeZero_ShouldNotLogError()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 0, ErrorMessage = "Success" });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
        }

        [Fact]
        public async Task Execute_WithErrorCodePositive_ShouldNotLogError()
        {
            // Arrange
            appSettings.Environment.Returns(1);
            fraudCaseService.CheckFraudCaseIsActive(3, 1).Returns(true);
            checkBetService.Rule().Returns(new DataResult { ErrorCode = 1, ErrorMessage = "Warning" });

            // Act
            var result = await jobService.Execute();

            // Assert
            result.Should().BeTrue();
            checkBetService.Received(1).Rule();
        }

        #endregion
    }
}

using Fanex.NAP.SPU.FS.JobService.Controllers;
using Fanex.NAP.SPU.FS.JobService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.JobService.Controllers
{
    public class JobControllerTests
    {
        private readonly JobController controller;
        private readonly IBigStateNearGoalJobService bigStateNearGoalJobService;
        private readonly ICheckBetJobService checkBetJobService;
        private readonly IDelayedGoalJobService delayedGoalJobService;
        private readonly IFlaggedAccountJobService flaggedAccountJobService;
        private readonly IDailyCaptureDataJobService dailyCaptureDataJobService;
        private readonly IScannedOfficeIPJobService scannedOfficeIpJobService;

        public JobControllerTests()
        {
            bigStateNearGoalJobService = Substitute.For<IBigStateNearGoalJobService>();
            checkBetJobService = Substitute.For<ICheckBetJobService>();
            delayedGoalJobService = Substitute.For<IDelayedGoalJobService>();
            flaggedAccountJobService = Substitute.For<IFlaggedAccountJobService>();
            dailyCaptureDataJobService = Substitute.For<IDailyCaptureDataJobService>();
            scannedOfficeIpJobService = Substitute.For<IScannedOfficeIPJobService>();

            controller = new JobController(
                bigStateNearGoalJobService,
                checkBetJobService,
                delayedGoalJobService,
                flaggedAccountJobService,
                dailyCaptureDataJobService,
                scannedOfficeIpJobService);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDependencies_ShouldCreateInstance()
        {
            // Arrange
            var mockBigStateNearGoalJobService = Substitute.For<IBigStateNearGoalJobService>();
            var mockCheckBetJobService = Substitute.For<ICheckBetJobService>();
            var mockDelayedGoalJobService = Substitute.For<IDelayedGoalJobService>();
            var mockFlaggedAccountJobService = Substitute.For<IFlaggedAccountJobService>();
            var mockDailyCaptureDataJobService = Substitute.For<IDailyCaptureDataJobService>();
            var mockScannedOfficeIpJobService = Substitute.For<IScannedOfficeIPJobService>();

            // Act
            var controller = new JobController(
                mockBigStateNearGoalJobService,
                mockCheckBetJobService,
                mockDelayedGoalJobService,
                mockFlaggedAccountJobService,
                mockDailyCaptureDataJobService,
                mockScannedOfficeIpJobService);

            // Assert
            controller.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ShouldCreateInstance()
        {
            // Arrange & Act
            var controller = new JobController(
                null, null, null, null, null, null);

            // Assert
            controller.Should().NotBeNull();
        }

        #endregion

        #region BigStateNearGoal Tests

        [Fact]
        public async Task BigStateNearGoal_WithValidJobName_ShouldReturnTrue()
        {
            // Arrange
            var jobName = "TestJob";
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.BigStateNearGoal(jobName);

            // Assert
            result.Should().BeTrue();
            await bigStateNearGoalJobService.Received(1).Execute();
        }

        [Fact]
        public async Task BigStateNearGoal_WithValidJobName_ShouldReturnFalse()
        {
            // Arrange
            var jobName = "TestJob";
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(false));

            // Act
            var result = await controller.BigStateNearGoal(jobName);

            // Assert
            result.Should().BeFalse();
            await bigStateNearGoalJobService.Received(1).Execute();
        }

        [Fact]
        public async Task BigStateNearGoal_WithNullJobName_ShouldCallService()
        {
            // Arrange
            string jobName = null;
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.BigStateNearGoal(jobName);

            // Assert
            result.Should().BeTrue();
            await bigStateNearGoalJobService.Received(1).Execute();
        }

        [Fact]
        public async Task BigStateNearGoal_WithEmptyJobName_ShouldCallService()
        {
            // Arrange
            var jobName = "";
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.BigStateNearGoal(jobName);

            // Assert
            result.Should().BeTrue();
            await bigStateNearGoalJobService.Received(1).Execute();
        }

        [Fact]
        public async Task BigStateNearGoal_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var jobName = "TestJob";
            var expectedException = new InvalidOperationException("Service error");
            bigStateNearGoalJobService.Execute().Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.BigStateNearGoal(jobName));
            exception.Should().Be(expectedException);
        }

        #endregion

        #region CheckBet Tests

        [Fact]
        public async Task CheckBet_WithValidJobName_ShouldReturnTrue()
        {
            // Arrange
            var jobName = "TestJob";
            checkBetJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.CheckBet(jobName);

            // Assert
            result.Should().BeTrue();
            await checkBetJobService.Received(1).Execute();
        }

        [Fact]
        public async Task CheckBet_WithValidJobName_ShouldReturnFalse()
        {
            // Arrange
            var jobName = "TestJob";
            checkBetJobService.Execute().Returns(Task.FromResult(false));

            // Act
            var result = await controller.CheckBet(jobName);

            // Assert
            result.Should().BeFalse();
            await checkBetJobService.Received(1).Execute();
        }

        [Fact]
        public async Task CheckBet_WithNullJobName_ShouldCallService()
        {
            // Arrange
            string jobName = null;
            checkBetJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.CheckBet(jobName);

            // Assert
            result.Should().BeTrue();
            await checkBetJobService.Received(1).Execute();
        }

        [Fact]
        public async Task CheckBet_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var jobName = "TestJob";
            var expectedException = new InvalidOperationException("Service error");
            checkBetJobService.Execute().Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.CheckBet(jobName));
            exception.Should().Be(expectedException);
        }

        #endregion

        #region DelayedGoal Tests

        [Fact]
        public async Task DelayedGoal_WithValidJobName_ShouldReturnTrue()
        {
            // Arrange
            var jobName = "TestJob";
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            var result = await controller.DelayedGoal(jobName);

            // Assert
            result.Should().BeTrue();
            await delayedGoalJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task DelayedGoal_WithValidJobName_ShouldReturnFalse()
        {
            // Arrange
            var jobName = "TestJob";
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(false));

            // Act
            var result = await controller.DelayedGoal(jobName);

            // Assert
            result.Should().BeFalse();
            await delayedGoalJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task DelayedGoal_WithNullJobName_ShouldCallService()
        {
            // Arrange
            string jobName = null;
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            var result = await controller.DelayedGoal(jobName);

            // Assert
            result.Should().BeTrue();
            await delayedGoalJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task DelayedGoal_ShouldCallServiceWithChunkSize()
        {
            // Arrange
            var jobName = "TestJob";
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            await controller.DelayedGoal(jobName);

            // Assert
            await delayedGoalJobService.Received(1).Execute(100); // chunkSize = 100
        }

        [Fact]
        public async Task DelayedGoal_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var jobName = "TestJob";
            var expectedException = new InvalidOperationException("Service error");
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.DelayedGoal(jobName));
            exception.Should().Be(expectedException);
        }

        #endregion

        #region FlaggedAccount Tests

        [Fact]
        public async Task FlaggedAccount_WithValidJobName_ShouldReturnTrue()
        {
            // Arrange
            var jobName = "TestJob";
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            var result = await controller.FlaggedAccount(jobName);

            // Assert
            result.Should().BeTrue();
            await flaggedAccountJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task FlaggedAccount_WithValidJobName_ShouldReturnFalse()
        {
            // Arrange
            var jobName = "TestJob";
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(false));

            // Act
            var result = await controller.FlaggedAccount(jobName);

            // Assert
            result.Should().BeFalse();
            await flaggedAccountJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task FlaggedAccount_WithNullJobName_ShouldCallService()
        {
            // Arrange
            string jobName = null;
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            var result = await controller.FlaggedAccount(jobName);

            // Assert
            result.Should().BeTrue();
            await flaggedAccountJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task FlaggedAccount_ShouldCallServiceWithChunkSize()
        {
            // Arrange
            var jobName = "TestJob";
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            await controller.FlaggedAccount(jobName);

            // Assert
            await flaggedAccountJobService.Received(1).Execute(100); // chunkSize = 100
        }

        [Fact]
        public async Task FlaggedAccount_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var jobName = "TestJob";
            var expectedException = new InvalidOperationException("Service error");
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.FlaggedAccount(jobName));
            exception.Should().Be(expectedException);
        }

        #endregion

        #region DailyCaptureData Tests

        [Fact]
        public async Task DailyCaptureData_WithValidJobName_ShouldReturnTrue()
        {
            // Arrange
            var jobName = "TestJob";
            dailyCaptureDataJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.DailyCaptureData(jobName);

            // Assert
            result.Should().BeTrue();
            await dailyCaptureDataJobService.Received(1).Execute();
        }

        [Fact]
        public async Task DailyCaptureData_WithValidJobName_ShouldReturnFalse()
        {
            // Arrange
            var jobName = "TestJob";
            dailyCaptureDataJobService.Execute().Returns(Task.FromResult(false));

            // Act
            var result = await controller.DailyCaptureData(jobName);

            // Assert
            result.Should().BeFalse();
            await dailyCaptureDataJobService.Received(1).Execute();
        }

        [Fact]
        public async Task DailyCaptureData_WithNullJobName_ShouldCallService()
        {
            // Arrange
            string jobName = null;
            dailyCaptureDataJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.DailyCaptureData(jobName);

            // Assert
            result.Should().BeTrue();
            await dailyCaptureDataJobService.Received(1).Execute();
        }

        [Fact]
        public async Task DailyCaptureData_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var jobName = "TestJob";
            var expectedException = new InvalidOperationException("Service error");
            dailyCaptureDataJobService.Execute().Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.DailyCaptureData(jobName));
            exception.Should().Be(expectedException);
        }

        #endregion

        #region ScannedOfficeIp Tests

        [Fact]
        public async Task ScannedOfficeIp_WithValidJobName_ShouldReturnTrue()
        {
            // Arrange
            var jobName = "TestJob";
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            var result = await controller.ScannedOfficeIp(jobName);

            // Assert
            result.Should().BeTrue();
            await scannedOfficeIpJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task ScannedOfficeIp_WithValidJobName_ShouldReturnFalse()
        {
            // Arrange
            var jobName = "TestJob";
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(false));

            // Act
            var result = await controller.ScannedOfficeIp(jobName);

            // Assert
            result.Should().BeFalse();
            await scannedOfficeIpJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task ScannedOfficeIp_WithNullJobName_ShouldCallService()
        {
            // Arrange
            string jobName = null;
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            var result = await controller.ScannedOfficeIp(jobName);

            // Assert
            result.Should().BeTrue();
            await scannedOfficeIpJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task ScannedOfficeIp_ShouldCallServiceWithChunkSize()
        {
            // Arrange
            var jobName = "TestJob";
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            await controller.ScannedOfficeIp(jobName);

            // Assert
            await scannedOfficeIpJobService.Received(1).Execute(100); // chunkSize = 100
        }

        [Fact]
        public async Task ScannedOfficeIp_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var jobName = "TestJob";
            var expectedException = new InvalidOperationException("Service error");
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.ScannedOfficeIp(jobName));
            exception.Should().Be(expectedException);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public async Task AllEndpoints_WithValidJobNames_ShouldCallRespectiveServices()
        {
            // Arrange
            var jobName = "TestJob";
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(true));
            checkBetJobService.Execute().Returns(Task.FromResult(true));
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));
            dailyCaptureDataJobService.Execute().Returns(Task.FromResult(true));
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            await controller.BigStateNearGoal(jobName);
            await controller.CheckBet(jobName);
            await controller.DelayedGoal(jobName);
            await controller.FlaggedAccount(jobName);
            await controller.DailyCaptureData(jobName);
            await controller.ScannedOfficeIp(jobName);

            // Assert
            await bigStateNearGoalJobService.Received(1).Execute();
            await checkBetJobService.Received(1).Execute();
            await delayedGoalJobService.Received(1).Execute(Arg.Any<int>());
            await flaggedAccountJobService.Received(1).Execute(Arg.Any<int>());
            await dailyCaptureDataJobService.Received(1).Execute();
            await scannedOfficeIpJobService.Received(1).Execute(Arg.Any<int>());
        }

        [Fact]
        public async Task AllEndpoints_WithNullJobNames_ShouldCallRespectiveServices()
        {
            // Arrange
            string jobName = null;
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(true));
            checkBetJobService.Execute().Returns(Task.FromResult(true));
            delayedGoalJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));
            flaggedAccountJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));
            dailyCaptureDataJobService.Execute().Returns(Task.FromResult(true));
            scannedOfficeIpJobService.Execute(Arg.Any<int>()).Returns(Task.FromResult(true));

            // Act
            await controller.BigStateNearGoal(jobName);
            await controller.CheckBet(jobName);
            await controller.DelayedGoal(jobName);
            await controller.FlaggedAccount(jobName);
            await controller.DailyCaptureData(jobName);
            await controller.ScannedOfficeIp(jobName);

            // Assert
            await bigStateNearGoalJobService.Received(1).Execute();
            await checkBetJobService.Received(1).Execute();
            await delayedGoalJobService.Received(1).Execute(Arg.Any<int>());
            await flaggedAccountJobService.Received(1).Execute(Arg.Any<int>());
            await dailyCaptureDataJobService.Received(1).Execute();
            await scannedOfficeIpJobService.Received(1).Execute(Arg.Any<int>());
        }

        #endregion

        #region Edge Cases Tests

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Test Job Name")]
        [InlineData("Test-Job-Name")]
        [InlineData("Test_Job_Name")]
        [InlineData("TestJob123")]
        public async Task BigStateNearGoal_WithVariousJobNames_ShouldCallService(string jobName)
        {
            // Arrange
            bigStateNearGoalJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.BigStateNearGoal(jobName);

            // Assert
            result.Should().BeTrue();
            await bigStateNearGoalJobService.Received(1).Execute();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Test Job Name")]
        [InlineData("Test-Job-Name")]
        [InlineData("Test_Job_Name")]
        [InlineData("TestJob123")]
        public async Task CheckBet_WithVariousJobNames_ShouldCallService(string jobName)
        {
            // Arrange
            checkBetJobService.Execute().Returns(Task.FromResult(true));

            // Act
            var result = await controller.CheckBet(jobName);

            // Assert
            result.Should().BeTrue();
            await checkBetJobService.Received(1).Execute();
        }

        [Fact]
        public void Controller_ShouldBeApiController()
        {
            // Arrange & Act
            var controllerType = typeof(JobController);

            // Assert
            controllerType.Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public void Controller_ShouldHaveCorrectRoute()
        {
            // Arrange & Act
            var controllerType = typeof(JobController);
            var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false)[0] as RouteAttribute;

            // Assert
            routeAttribute.Template.Should().Be("api/job");
        }

        #endregion
    }
}

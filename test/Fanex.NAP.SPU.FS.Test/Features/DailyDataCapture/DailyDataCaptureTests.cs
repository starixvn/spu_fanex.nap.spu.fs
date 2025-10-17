using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.DailyDataCapture
{
    public class DailyDataCaptureTests
    {
        private readonly DailyDataCaptureServices dailyDataCaptureServices;
        private readonly IDailyDataCaptureDataAccess dailyDataCaptureDataAccess;

        public DailyDataCaptureTests()
        {
            dailyDataCaptureDataAccess = Substitute.For<IDailyDataCaptureDataAccess>();
            dailyDataCaptureServices = new DailyDataCaptureServices(dailyDataCaptureDataAccess);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new DailyDataCaptureServices(dailyDataCaptureDataAccess);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new DailyDataCaptureServices(null);

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region Successful Rule Execution Tests

        [Fact]
        public void Rule_WithSuccessfulExecution_ShouldReturnDataResult()
        {
            // Arrange
            var expectedResult = new DataResult
            {
                ErrorCode = 0
            };

            dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().Be(expectedResult);
            result.ErrorCode.Should().Be(0);
        }

        [Fact]
        public void Rule_WithSuccessfulExecution_ShouldCallDataAccess()
        {
            // Arrange
            var expectedResult = new DataResult { ErrorCode = 0 };
            dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

            // Act
            dailyDataCaptureServices.Rule();

            // Assert
            dailyDataCaptureDataAccess.Received(1).Rule();
        }

        #endregion

        #region Failed Rule Execution Tests

        [Fact]
        public void Rule_WithFailedExecution_ShouldReturnDataResultWithErrorCode()
        {
            // Arrange
            var expectedResult = new DataResult
            {
                ErrorCode = -1,
                ErrorMessage = "Database error"
            };

            dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().Be(expectedResult);
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Database error");
        }

        [Fact]
        public void Rule_WithDifferentErrorCodes_ShouldReturnCorrectErrorCode()
        {
            // Arrange
            var testCases = new[]
            {
                new { ErrorCode = -100, ErrorMessage = "Critical error" },
                new { ErrorCode = -200, ErrorMessage = "Validation error" },
                new { ErrorCode = -500, ErrorMessage = "System error" }
            };

            foreach (var testCase in testCases)
            {
                // Arrange
                var expectedResult = new DataResult
                {
                    ErrorCode = testCase.ErrorCode,
                    ErrorMessage = testCase.ErrorMessage
                };

                dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

                // Act
                var result = dailyDataCaptureServices.Rule();

                // Assert
                result.ErrorCode.Should().Be(testCase.ErrorCode);
                result.ErrorMessage.Should().Be(testCase.ErrorMessage);
            }
        }

        #endregion

        #region Exception Handling Tests

        [Fact]
        public void Rule_WhenDataAccessThrowsException_ShouldReturnErrorResult()
        {
            // Arrange
            var exception = new Exception("Database connection failed");
            dailyDataCaptureDataAccess.Rule().Throws(exception);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(-100);
            result.ErrorMessage.Should().NotBeNullOrEmpty();
            result.ErrorMessage.Should().Contain("Database connection failed");
        }

        [Fact]
        public void Rule_WhenDataAccessThrowsArgumentException_ShouldReturnErrorResult()
        {
            // Arrange
            var exception = new ArgumentException("Invalid parameter");
            dailyDataCaptureDataAccess.Rule().Throws(exception);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(-100);
            result.ErrorMessage.Should().NotBeNullOrEmpty();
            result.ErrorMessage.Should().Contain("Invalid parameter");
        }

        [Fact]
        public void Rule_WhenDataAccessThrowsNullReferenceException_ShouldReturnErrorResult()
        {
            // Arrange
            var exception = new NullReferenceException("Object reference not set");
            dailyDataCaptureDataAccess.Rule().Throws(exception);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(-100);
            result.ErrorMessage.Should().NotBeNullOrEmpty();
            result.ErrorMessage.Should().Contain("Object reference not set");
        }

        [Fact]
        public void Rule_WhenDataAccessThrowsException_ShouldContainLoggingInformation()
        {
            // Arrange
            var exception = new Exception("Test exception message");
            dailyDataCaptureDataAccess.Rule().Throws(exception);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(-100);
            result.ErrorMessage.Should().NotBeNullOrEmpty();
            result.ErrorMessage.Should().Contain("Test exception message");
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public void Rule_WithDataAccessReturningNull_ShouldReturnNull()
        {
            // Arrange
            dailyDataCaptureDataAccess.Rule().Returns((DataResult)null);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().BeNull();
        }      

        #endregion

        #region Multiple Calls Tests

        [Fact]
        public void Rule_WithMultipleCalls_ShouldCallDataAccessEachTime()
        {
            // Arrange
            var expectedResult = new DataResult { ErrorCode = 0 };
            dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

            // Act
            dailyDataCaptureServices.Rule();
            dailyDataCaptureServices.Rule();
            dailyDataCaptureServices.Rule();

            // Assert
            dailyDataCaptureDataAccess.Received(3).Rule();
        }

        [Fact]
        public void Rule_WithMixedResults_ShouldReturnCorrectResults()
        {
            // Arrange
            var successResult = new DataResult { ErrorCode = 0 };
            var errorResult = new DataResult { ErrorCode = -100, ErrorMessage = "Error" };

            dailyDataCaptureDataAccess.Rule().Returns(successResult, errorResult, successResult);

            // Act
            var result1 = dailyDataCaptureServices.Rule();
            var result2 = dailyDataCaptureServices.Rule();
            var result3 = dailyDataCaptureServices.Rule();

            // Assert
            result1.ErrorCode.Should().Be(0);
            result2.ErrorCode.Should().Be(-100);
            result3.ErrorCode.Should().Be(0);
        }

        #endregion

        #region Data Capture Specific Tests

        [Fact]
        public void Rule_WithDataCaptureSuccess_ShouldReturnSuccessResult()
        {
            // Arrange
            var expectedResult = new DataResult
            {
                ErrorCode = 0,
                ErrorMessage = "Daily data capture completed successfully"
            };

            dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().Be(expectedResult);
            result.ErrorCode.Should().Be(0);
            result.ErrorMessage.Should().Be("Daily data capture completed successfully");
        }

        [Fact]
        public void Rule_WithDataCaptureFailure_ShouldReturnErrorResult()
        {
            // Arrange
            var expectedResult = new DataResult
            {
                ErrorCode = -1,
                ErrorMessage = "Daily data capture failed"
            };

            dailyDataCaptureDataAccess.Rule().Returns(expectedResult);

            // Act
            var result = dailyDataCaptureServices.Rule();

            // Assert
            result.Should().Be(expectedResult);
            result.ErrorCode.Should().Be(-1);
            result.ErrorMessage.Should().Be("Daily data capture failed");
        }

        #endregion
    }
}

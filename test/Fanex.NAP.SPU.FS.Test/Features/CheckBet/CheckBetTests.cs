using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.CheckBet
{
    public class CheckBetTests
    {
        private readonly CheckBetServices checkBetServices;
        private readonly ICheckBetDataAccess checkBetDataAccess;

        public CheckBetTests()
        {
            checkBetDataAccess = Substitute.For<ICheckBetDataAccess>();
            checkBetServices = new CheckBetServices(checkBetDataAccess);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new CheckBetServices(checkBetDataAccess);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new CheckBetServices(null);

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

            checkBetDataAccess.Rule().Returns(expectedResult);

            // Act
            var result = checkBetServices.Rule();

            // Assert
            result.Should().Be(expectedResult);
            result.ErrorCode.Should().Be(0);
        }

        [Fact]
        public void Rule_WithSuccessfulExecution_ShouldCallDataAccess()
        {
            // Arrange
            var expectedResult = new DataResult { ErrorCode = 0 };
            checkBetDataAccess.Rule().Returns(expectedResult);

            // Act
            checkBetServices.Rule();

            // Assert
            checkBetDataAccess.Received(1).Rule();
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

            checkBetDataAccess.Rule().Returns(expectedResult);

            // Act
            var result = checkBetServices.Rule();

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

                checkBetDataAccess.Rule().Returns(expectedResult);

                // Act
                var result = checkBetServices.Rule();

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
            checkBetDataAccess.Rule().Throws(exception);

            // Act
            var result = checkBetServices.Rule();

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
            checkBetDataAccess.Rule().Throws(exception);

            // Act
            var result = checkBetServices.Rule();

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
            checkBetDataAccess.Rule().Throws(exception);

            // Act
            var result = checkBetServices.Rule();

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
            checkBetDataAccess.Rule().Throws(exception);

            // Act
            var result = checkBetServices.Rule();

            // Assert
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be(-100);
            result.ErrorMessage.Should().NotBeNullOrEmpty();                  
        }

        #endregion

        #region Edge Cases Tests      
        [Fact]
        public void Rule_WithDataAccessReturningNull_ShouldReturnNull()
        {
            // Arrange
            checkBetDataAccess.Rule().Returns((DataResult)null);

            // Act
            var result = checkBetServices.Rule();

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
            checkBetDataAccess.Rule().Returns(expectedResult);

            // Act
            checkBetServices.Rule();
            checkBetServices.Rule();
            checkBetServices.Rule();

            // Assert
            checkBetDataAccess.Received(3).Rule();
        }

        [Fact]
        public void Rule_WithMixedResults_ShouldReturnCorrectResults()
        {
            // Arrange
            var successResult = new DataResult { ErrorCode = 0 };
            var errorResult = new DataResult { ErrorCode = -100, ErrorMessage = "Error" };

            checkBetDataAccess.Rule().Returns(successResult, errorResult, successResult);

            // Act
            var result1 = checkBetServices.Rule();
            var result2 = checkBetServices.Rule();
            var result3 = checkBetServices.Rule();

            // Assert
            result1.ErrorCode.Should().Be(0);
            result2.ErrorCode.Should().Be(-100);
            result3.ErrorCode.Should().Be(0);
        }

        #endregion
    }
}

using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Shares
{
    public class BaseServiceTests
    {
        private readonly BaseService baseService;

        public BaseServiceTests()
        {
            baseService = new BaseService();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new BaseService();

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region Precondition Tests

        [Fact]
        public void Precondition_ShouldReturnSameInstance()
        {
            // Act
            var result = baseService.Precondition();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void Precondition_ShouldSupportMethodChaining()
        {
            // Act
            var result = baseService.Precondition().Precondition().Precondition();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        #endregion

        #region Rule() Tests

        [Fact]
        public void Rule_ShouldReturnNewDataResult()
        {
            // Act
            var result = baseService.Rule();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<DataResult>();
        }

        [Fact]
        public void Rule_ShouldReturnDataResultWithDefaultValues()
        {
            // Act
            var result = baseService.Rule();

            // Assert
            result.ErrorCode.Should().Be(0);
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void Rule_WithMultipleCalls_ShouldReturnDifferentInstances()
        {
            // Act
            var result1 = baseService.Rule();
            var result2 = baseService.Rule();

            // Assert
            result1.Should().NotBeSameAs(result2);
        }

        #endregion

        #region Rule(int chunkSize) Tests

        [Fact]
        public void Rule_WithChunkSize_ShouldReturnNewDataResult()
        {
            // Arrange
            var chunkSize = 100;

            // Act
            var result = baseService.Rule(chunkSize);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<DataResult>();
        }

        [Fact]
        public void Rule_WithChunkSize_ShouldReturnDataResultWithDefaultValues()
        {
            // Arrange
            var chunkSize = 50;

            // Act
            var result = baseService.Rule(chunkSize);

            // Assert
            result.ErrorCode.Should().Be(0);
            result.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void Rule_WithDifferentChunkSizes_ShouldReturnDataResult()
        {
            // Arrange
            var chunkSizes = new[] { 0, 1, 10, 100, 1000, -1, -100 };

            foreach (var chunkSize in chunkSizes)
            {
                // Act
                var result = baseService.Rule(chunkSize);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<DataResult>();
                result.ErrorCode.Should().Be(0);
            }
        }

        [Fact]
        public void Rule_WithMultipleCallsAndChunkSize_ShouldReturnDifferentInstances()
        {
            // Arrange
            var chunkSize = 25;

            // Act
            var result1 = baseService.Rule(chunkSize);
            var result2 = baseService.Rule(chunkSize);

            // Assert
            result1.Should().NotBeSameAs(result2);
        }

        #endregion

        #region Rules() Tests

        [Fact]
        public void Rules_ShouldReturnEmptyList()
        {
            // Act
            var result = baseService.Rules();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<DataResult>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public void Rules_WithMultipleCalls_ShouldReturnDifferentInstances()
        {
            // Act
            var result1 = baseService.Rules();
            var result2 = baseService.Rules();

            // Assert
            result1.Should().NotBeSameAs(result2);
        }

        [Fact]
        public void Rules_ShouldReturnEnumerableOfDataResult()
        {
            // Act
            var result = baseService.Rules();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<DataResult>>();
        }

        #endregion

        #region Rules(int chunkSize) Tests

        [Fact]
        public void Rules_WithChunkSize_ShouldReturnEmptyList()
        {
            // Arrange
            var chunkSize = 200;

            // Act
            var result = baseService.Rules(chunkSize);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<DataResult>>();
            result.Should().BeEmpty();
        }

        [Fact]
        public void Rules_WithDifferentChunkSizes_ShouldReturnEmptyList()
        {
            // Arrange
            var chunkSizes = new[] { 0, 1, 50, 200, 500, -1, -50 };

            foreach (var chunkSize in chunkSizes)
            {
                // Act
                var result = baseService.Rules(chunkSize);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeOfType<List<DataResult>>();
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public void Rules_WithChunkSize_ShouldReturnEnumerableOfDataResult()
        {
            // Arrange
            var chunkSize = 75;

            // Act
            var result = baseService.Rules(chunkSize);

            // Assert
            result.Should().BeAssignableTo<IEnumerable<DataResult>>();
        }

        [Fact]
        public void Rules_WithMultipleCallsAndChunkSize_ShouldReturnDifferentInstances()
        {
            // Arrange
            var chunkSize = 150;

            // Act
            var result1 = baseService.Rules(chunkSize);
            var result2 = baseService.Rules(chunkSize);

            // Assert
            result1.Should().NotBeSameAs(result2);
        }

        #endregion

        #region CleanResult Tests

        [Fact]
        public void CleanResult_ShouldReturnSameInstance()
        {
            // Act
            var result = baseService.CleanResult();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void CleanResult_ShouldSupportMethodChaining()
        {
            // Act
            var result = baseService.CleanResult().CleanResult().CleanResult();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void CleanResult_ShouldSupportChainingWithOtherMethods()
        {
            // Act
            var result = baseService.Precondition().CleanResult().Precondition();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        #endregion

        #region GetFlaggedCustomer Tests

        [Fact]
        public void GetFlaggedCustomer_WithValidApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrl = "https://api.example.com/flagged-customers";

            // Act
            var result = baseService.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void GetFlaggedCustomer_WithNullApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            string apiUrl = null;

            // Act
            var result = baseService.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void GetFlaggedCustomer_WithEmptyApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrl = string.Empty;

            // Act
            var result = baseService.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void GetFlaggedCustomer_WithDifferentApiUrls_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrls = new[]
            {
                "https://api.example.com/flagged-customers",
                "https://api.test.com/flagged-customers",
                "http://localhost:5000/api/flagged-customers",
                "https://api.prod.com/v1/flagged-customers",
                "invalid-url",
                ""
            };

            foreach (var apiUrl in apiUrls)
            {
                // Act
                var result = baseService.GetFlaggedCustomer(apiUrl);

                // Assert
                result.Should().BeSameAs(baseService);
            }
        }

        [Fact]
        public void GetFlaggedCustomer_ShouldSupportMethodChaining()
        {
            // Arrange
            var apiUrl = "https://api.example.com/flagged-customers";

            // Act
            var result = baseService.GetFlaggedCustomer(apiUrl).GetFlaggedCustomer(apiUrl).GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void GetFlaggedCustomer_ShouldSupportChainingWithOtherMethods()
        {
            // Arrange
            var apiUrl = "https://api.example.com/flagged-customers";

            // Act
            var result = baseService.Precondition().GetFlaggedCustomer(apiUrl).CleanResult();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        #endregion

        #region Method Chaining Tests

        [Fact]
        public void AllMethods_ShouldSupportMethodChaining()
        {
            // Arrange
            var apiUrl = "https://api.example.com/flagged-customers";
            var chunkSize = 100;

            // Act
            var result = baseService
                .Precondition()
                .CleanResult()
                .GetFlaggedCustomer(apiUrl)
                .Precondition()
                .CleanResult();

            // Assert
            result.Should().BeSameAs(baseService);
        }

        [Fact]
        public void AllMethods_ShouldReturnConsistentResults()
        {
            // Arrange
            var apiUrl = "https://api.example.com/flagged-customers";
            var chunkSize = 50;

            // Act
            var preconditionResult = baseService.Precondition();
            var cleanResult = baseService.CleanResult();
            var flaggedCustomerResult = baseService.GetFlaggedCustomer(apiUrl);
            var ruleResult = baseService.Rule();
            var ruleWithChunkResult = baseService.Rule(chunkSize);
            var rulesResult = baseService.Rules();
            var rulesWithChunkResult = baseService.Rules(chunkSize);

            // Assert
            preconditionResult.Should().BeSameAs(baseService);
            cleanResult.Should().BeSameAs(baseService);
            flaggedCustomerResult.Should().BeSameAs(baseService);
            ruleResult.Should().NotBeNull().And.BeOfType<DataResult>();
            ruleWithChunkResult.Should().NotBeNull().And.BeOfType<DataResult>();
            rulesResult.Should().NotBeNull().And.BeAssignableTo<IEnumerable<DataResult>>();
            rulesWithChunkResult.Should().NotBeNull().And.BeAssignableTo<IEnumerable<DataResult>>();
        }

        #endregion

        #region Virtual Method Behavior Tests

        [Fact]
        public void VirtualMethods_ShouldBeOverridable()
        {
            // Arrange
            var derivedService = new TestableBaseService();

            // Act
            var preconditionResult = derivedService.Precondition();
            var cleanResult = derivedService.CleanResult();
            var flaggedCustomerResult = derivedService.GetFlaggedCustomer("test-url");
            var ruleResult = derivedService.Rule();
            var ruleWithChunkResult = derivedService.Rule(10);
            var rulesResult = derivedService.Rules();
            var rulesWithChunkResult = derivedService.Rules(10);

            // Assert
            preconditionResult.Should().BeSameAs(derivedService);
            cleanResult.Should().BeSameAs(derivedService);
            flaggedCustomerResult.Should().BeSameAs(derivedService);
            ruleResult.Should().NotBeNull().And.BeOfType<DataResult>();
            ruleWithChunkResult.Should().NotBeNull().And.BeOfType<DataResult>();
            rulesResult.Should().NotBeNull().And.BeAssignableTo<IEnumerable<DataResult>>();
            rulesWithChunkResult.Should().NotBeNull().And.BeAssignableTo<IEnumerable<DataResult>>();
        }

        #endregion
    }

    #region Test Helper Classes

    public class TestableBaseService : BaseService
    {
        // This class is used to test that virtual methods can be overridden
        // The base implementation is sufficient for testing purposes
    }

    #endregion
}

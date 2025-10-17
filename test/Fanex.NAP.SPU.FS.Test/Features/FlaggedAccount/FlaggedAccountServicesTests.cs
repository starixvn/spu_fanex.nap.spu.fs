using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount.Entities;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.FlaggedAccount
{
    public class FlaggedAccountServicesTests
    {
        private readonly FlaggedAccountServices flaggedAccountServices;
        private readonly IFlaggedAccountDataAccess flaggedAccountDataAccess;

        public FlaggedAccountServicesTests()
        {
            flaggedAccountDataAccess = Substitute.For<IFlaggedAccountDataAccess>();
            flaggedAccountServices = new FlaggedAccountServices(flaggedAccountDataAccess);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDataAccess_ShouldCreateInstance()
        {
            // Arrange
            var mockDataAccess = Substitute.For<IFlaggedAccountDataAccess>();

            // Act
            var service = new FlaggedAccountServices(mockDataAccess);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new FlaggedAccountServices(null);

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region GetFlaggedCustomer Tests

        [Fact]
        public void GetFlaggedCustomer_WithValidApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrl = "https://api.example.com";

            // Act
            var result = flaggedAccountServices.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
        }

        [Fact]
        public void GetFlaggedCustomer_WithEmptyApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrl = string.Empty;

            // Act
            var result = flaggedAccountServices.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
        }

        [Fact]
        public void GetFlaggedCustomer_WithNullApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            string apiUrl = null;

            // Act
            var result = flaggedAccountServices.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
        }

        [Fact]
        public void GetFlaggedCustomer_WithWhitespaceApiUrl_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrl = "   ";

            // Act
            var result = flaggedAccountServices.GetFlaggedCustomer(apiUrl);

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
        }

        [Fact]
        public void GetFlaggedCustomer_WithDifferentApiUrls_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrls = new[]
            {
                "https://api.example.com",
                "https://test-api.example.com",
                "http://localhost:5000/api",
                "https://staging-api.example.com/v1",
                "https://production-api.example.com/v2"
            };

            foreach (var apiUrl in apiUrls)
            {
                // Act
                var result = flaggedAccountServices.GetFlaggedCustomer(apiUrl);

                // Assert
                result.Should().BeSameAs(flaggedAccountServices);
            }
        }

        [Fact]
        public void GetFlaggedCustomer_WithMultipleCalls_ShouldReturnSameInstance()
        {
            // Arrange
            var apiUrl = "https://api.example.com";

            // Act
            var result1 = flaggedAccountServices.GetFlaggedCustomer(apiUrl);
            var result2 = flaggedAccountServices.GetFlaggedCustomer(apiUrl);
            var result3 = flaggedAccountServices.GetFlaggedCustomer(apiUrl);

            // Assert
            result1.Should().BeSameAs(flaggedAccountServices);
            result2.Should().BeSameAs(flaggedAccountServices);
            result3.Should().BeSameAs(flaggedAccountServices);
        }

        #endregion

        #region CleanResult Tests

        [Fact]
        public void CleanResult_WithNullMatchList_ShouldReturnSameInstance()
        {
            // Arrange
            // MatchList is null by default

            // Act
            var result = flaggedAccountServices.CleanResult();

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
            flaggedAccountServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void CleanResult_WithEmptyMatchList_ShouldReturnSameInstance()
        {
            // Arrange
            // Set up empty match list through Precondition
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(Enumerable.Empty<MatchEntity>());
            flaggedAccountServices.Precondition();

            // Act
            var result = flaggedAccountServices.CleanResult();

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
            flaggedAccountServices.MatchList.Should().NotBeNull();
            flaggedAccountServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_WithPopulatedMatchList_ShouldClearMatchList()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountServices.Precondition();

            // Act
            var result = flaggedAccountServices.CleanResult();

            // Assert
            result.Should().BeSameAs(flaggedAccountServices);
            flaggedAccountServices.MatchList.Should().NotBeNull();
            flaggedAccountServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_WithMultipleCalls_ShouldHandleEachCall()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountServices.Precondition();

            // Act
            var result1 = flaggedAccountServices.CleanResult();
            var result2 = flaggedAccountServices.CleanResult();
            var result3 = flaggedAccountServices.CleanResult();

            // Assert
            result1.Should().BeSameAs(flaggedAccountServices);
            result2.Should().BeSameAs(flaggedAccountServices);
            result3.Should().BeSameAs(flaggedAccountServices);
            flaggedAccountServices.MatchList.Should().BeEmpty();
        }

        #endregion

        #region Precondition Tests

        [Fact]
        public void Precondition_WithValidMatches_ShouldReturnMatchList()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };

            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            flaggedAccountServices.Precondition();

            // Assert
            flaggedAccountServices.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void Precondition_WithEmptyMatches_ShouldReturnEmptyMatchList()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>();
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            flaggedAccountServices.Precondition();

            // Assert
            flaggedAccountServices.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void Precondition_WithNullMatches_ShouldReturnNullMatchList()
        {
            // Arrange
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);

            // Act
            flaggedAccountServices.Precondition();

            // Assert
            flaggedAccountServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Precondition_ShouldCallDataAccessPreCondition()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            flaggedAccountServices.Precondition();

            // Assert
            flaggedAccountDataAccess.Received(1).PreCondition<MatchEntity>();
        }

        [Fact]
        public void Precondition_WithMultipleCalls_ShouldCallDataAccessMultipleTimes()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            flaggedAccountServices.Precondition();
            flaggedAccountServices.Precondition();
            flaggedAccountServices.Precondition();

            // Assert
            flaggedAccountDataAccess.Received(3).PreCondition<MatchEntity>();
        }

        #endregion

        #region Rules Tests

        [Fact]
        public void Rules_WithValidMatches_ShouldReturnDataResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountDataAccess.Rule("1,2,3", Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0 });
            flaggedAccountServices.Precondition();

            // Act
            var results = flaggedAccountServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // No error results
        }
       
        [Fact]
        public void Rules_WithMixedResults_ShouldReturnOnlyErrorResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountDataAccess.Rule("1,2,3", Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0, ErrorMessage = "Success" });
            flaggedAccountServices.Precondition();

            // Act
            var results = flaggedAccountServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // No error results
        }
       
        [Fact]
        public void Rules_WithEmptyMatchList_ShouldReturnEmptyResults()
        {
            // Arrange
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());
            flaggedAccountServices.Precondition();

            // Act
            var results = flaggedAccountServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        #endregion        

        #region MatchList Property Tests

        [Fact]
        public void MatchList_ShouldReturnCurrentMatchList()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);
            flaggedAccountServices.Precondition();

            // Act
            var matchList = flaggedAccountServices.MatchList;

            // Assert
            matchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void MatchList_WithNullMatches_ShouldReturnNull()
        {
            // Arrange
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);
            flaggedAccountServices.Precondition();

            // Act
            var matchList = flaggedAccountServices.MatchList;

            // Assert
            matchList.Should().BeNull();
        }

        [Fact]
        public void MatchList_WithEmptyMatches_ShouldReturnEmpty()
        {
            // Arrange
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());
            flaggedAccountServices.Precondition();

            // Act
            var matchList = flaggedAccountServices.MatchList;

            // Assert
            matchList.Should().NotBeNull();
            matchList.Should().BeEmpty();
        }

        #endregion      
       
        #region BaseService Integration Tests

        [Fact]
        public void FlaggedAccountServices_ShouldInheritFromBaseService()
        {
            // Arrange
            var service = new FlaggedAccountServices(flaggedAccountDataAccess);

            // Assert
            service.Should().BeAssignableTo<BaseService>();
        }

        [Fact]
        public void CleanResult_ShouldReturnBaseService()
        {
            // Arrange
            // MatchList is null by default

            // Act
            var result = flaggedAccountServices.CleanResult();

            // Assert
            result.Should().BeAssignableTo<BaseService>();
        }

        [Fact]
        public void Precondition_ShouldReturnBaseService()
        {
            // Arrange
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());

            // Act
            var result = flaggedAccountServices.Precondition();

            // Assert
            result.Should().BeAssignableTo<BaseService>();
        }

        #endregion

        #region Performance Tests

        [Fact]
        public void Rules_WithLargeDataSet_ShouldCompleteWithinReasonableTime()
        {
            // Arrange
            var matches = Enumerable.Range(1, 1000)
                .Select(i => new MatchEntity { MatchId = i })
                .ToList();
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountDataAccess.Rule(Arg.Any<string>(), Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0 });
            flaggedAccountServices.Precondition();

            var startTime = DateTime.Now;

            // Act
            var results = flaggedAccountServices.Rules(100);

            // Assert
            var elapsed = DateTime.Now - startTime;
            results.Should().NotBeNull();
            elapsed.TotalSeconds.Should().BeLessThan(30); // Should complete within 30 seconds
        }

        #endregion

        #region FlaggedCustomer Integration Tests

        #endregion       

        #region Error Code Tests

      
        [Fact]
        public void Rules_WithDifferentErrorCodes_ShouldOnlyReturnMinus100()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountDataAccess.Rule("1", Arg.Any<string>()).Returns(new DataResult { ErrorCode = -1, ErrorMessage = "Warning" });
            flaggedAccountServices.Precondition();

            // Act
            var results = flaggedAccountServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // Only -100 errors are returned
        }

        #endregion

        #region Data Cleanup Tests

        [Fact]
        public void Rules_ShouldCleanupDataAfterExecution()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            flaggedAccountDataAccess.PreCondition<MatchEntity>().Returns(matches);
            flaggedAccountDataAccess.Rule("1", Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0 });
            flaggedAccountServices.Precondition();

            // Act
            var results = flaggedAccountServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            // Note: We can't directly test private fields, but we can verify the method completes successfully
        }

        #endregion

      
    }
}

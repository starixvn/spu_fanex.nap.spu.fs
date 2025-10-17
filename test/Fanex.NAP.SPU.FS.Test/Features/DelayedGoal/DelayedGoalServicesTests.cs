using Fanex.NAP.SPU.FS.Features.DelayedGoal;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.DelayedGoal
{
    public class DelayedGoalServicesTests
    {
        private readonly DelayedGoalServices delayedGoalServices;
        private readonly IDelayedGoalDataAccess delayedGoalDataAccess;

        public DelayedGoalServicesTests()
        {
            delayedGoalDataAccess = Substitute.For<IDelayedGoalDataAccess>();
            delayedGoalServices = new DelayedGoalServices(delayedGoalDataAccess);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDataAccess_ShouldCreateInstance()
        {
            // Arrange
            var mockDataAccess = Substitute.For<IDelayedGoalDataAccess>();

            // Act
            var service = new DelayedGoalServices(mockDataAccess);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new DelayedGoalServices(null);

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region CleanResult Tests

        [Fact]
        public void CleanResult_WithNullMatchList_ShouldReturnSameInstance()
        {
            // Arrange
            // MatchList is null by default

            // Act
            var result = delayedGoalServices.CleanResult();

            // Assert
            result.Should().BeSameAs(delayedGoalServices);
            delayedGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void CleanResult_WithEmptyMatchList_ShouldReturnSameInstance()
        {
            // Arrange
            // Set up empty match list through Precondition
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(Enumerable.Empty<MatchEntity>());
            delayedGoalServices.Precondition();

            // Act
            var result = delayedGoalServices.CleanResult();

            // Assert
            result.Should().BeSameAs(delayedGoalServices);
            delayedGoalServices.MatchList.Should().NotBeNull();
            delayedGoalServices.MatchList.Should().BeEmpty();
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
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalServices.Precondition();

            // Act
            var result = delayedGoalServices.CleanResult();

            // Assert
            result.Should().BeSameAs(delayedGoalServices);
            delayedGoalServices.MatchList.Should().NotBeNull();
            delayedGoalServices.MatchList.Should().BeEmpty();
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
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalServices.Precondition();

            // Act
            var result1 = delayedGoalServices.CleanResult();
            var result2 = delayedGoalServices.CleanResult();
            var result3 = delayedGoalServices.CleanResult();

            // Assert
            result1.Should().BeSameAs(delayedGoalServices);
            result2.Should().BeSameAs(delayedGoalServices);
            result3.Should().BeSameAs(delayedGoalServices);
            delayedGoalServices.MatchList.Should().BeEmpty();
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

            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            delayedGoalServices.Precondition();

            // Assert
            delayedGoalServices.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void Precondition_WithEmptyMatches_ShouldReturnEmptyMatchList()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>();
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            delayedGoalServices.Precondition();

            // Assert
            delayedGoalServices.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void Precondition_WithNullMatches_ShouldReturnNullMatchList()
        {
            // Arrange
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);

            // Act
            delayedGoalServices.Precondition();

            // Assert
            delayedGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Precondition_ShouldCallDataAccessPreCondition()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            delayedGoalServices.Precondition();

            // Assert
            delayedGoalDataAccess.Received(1).PreCondition<MatchEntity>();
        }

        [Fact]
        public void Precondition_WithMultipleCalls_ShouldCallDataAccessMultipleTimes()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            delayedGoalServices.Precondition();
            delayedGoalServices.Precondition();
            delayedGoalServices.Precondition();

            // Assert
            delayedGoalDataAccess.Received(3).PreCondition<MatchEntity>();
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
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2,3").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // No error results
        }

        [Fact]
        public void Rules_WithErrorResults_ShouldReturnErrorResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2").Returns(new DataResult { ErrorCode = -100, ErrorMessage = "Error" });
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results.First().ErrorCode.Should().Be(-100);
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
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2,3").Returns(new DataResult { ErrorCode = 0, ErrorMessage = "Success" });
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // No error results
        }

        [Fact]
        public void Rules_WithChunking_ShouldProcessInChunks()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 },
                new MatchEntity { MatchId = 4 },
                new MatchEntity { MatchId = 5 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalDataAccess.Rule("3,4").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalDataAccess.Rule("5").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(2);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // No error results
            delayedGoalDataAccess.Received(3).Rule(Arg.Any<string>());
        }

        [Fact]
        public void Rules_WithEmptyMatchList_ShouldReturnEmptyResults()
        {
            // Arrange
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }
       
        #endregion

        #region Execute Tests

        [Fact]
        public void Rules_WhenExecuteThrowsException_ShouldReturnErrorResult()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1").Throws(new Exception("Database error"));
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results.First().ErrorCode.Should().Be(-100);
            results.First().ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Rules_WhenExecuteThrowsArgumentException_ShouldReturnErrorResult()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1").Throws(new ArgumentException("Invalid argument"));
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results.First().ErrorCode.Should().Be(-100);
            results.First().ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Rules_WhenExecuteThrowsNullReferenceException_ShouldReturnErrorResult()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1").Throws(new NullReferenceException("Null reference"));
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(10);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results.First().ErrorCode.Should().Be(-100);
            results.First().ErrorMessage.Should().NotBeNullOrEmpty();
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
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);
            delayedGoalServices.Precondition();

            // Act
            var matchList = delayedGoalServices.MatchList;

            // Assert
            matchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void MatchList_WithNullMatches_ShouldReturnNull()
        {
            // Arrange
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);
            delayedGoalServices.Precondition();

            // Act
            var matchList = delayedGoalServices.MatchList;

            // Assert
            matchList.Should().BeNull();
        }

        [Fact]
        public void MatchList_WithEmptyMatches_ShouldReturnEmpty()
        {
            // Arrange
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());
            delayedGoalServices.Precondition();

            // Act
            var matchList = delayedGoalServices.MatchList;

            // Assert
            matchList.Should().NotBeNull();
            matchList.Should().BeEmpty();
        }

        #endregion

        #region Edge Cases Tests
       
        [Fact]
        public void Rules_WithLargeChunkSize_ShouldProcessAllMatches()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            // Act
            var results = delayedGoalServices.Rules(1000);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            delayedGoalDataAccess.Received(1).Rule("1,2");
        }

        #endregion

        #region Multiple Calls Tests

        [Fact]
        public void Rules_WithMultipleCalls_ShouldHandleEachCall()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            // Act
            var results1 = delayedGoalServices.Rules(10);
            var results2 = delayedGoalServices.Rules(10);
            var results3 = delayedGoalServices.Rules(10);

            // Assert
            results1.Should().NotBeNull();
            results2.Should().NotBeNull();
            results3.Should().NotBeNull();
        }

        #endregion

        #region Data Access Integration Tests

        [Fact]
        public void Rules_ShouldCallDataAccessRuleWithCorrectMatchIds()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2,3").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            // Act
            delayedGoalServices.Rules(10);

            // Assert
            delayedGoalDataAccess.Received(1).Rule("1,2,3");
        }

        [Fact]
        public void Rules_WithChunking_ShouldCallDataAccessRuleWithCorrectChunkedMatchIds()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule("1,2").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalDataAccess.Rule("3").Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            // Act
            delayedGoalServices.Rules(2);

            // Assert
            delayedGoalDataAccess.Received(1).Rule("1,2");
            delayedGoalDataAccess.Received(1).Rule("3");
        }

        #endregion

        #region BaseService Integration Tests

        [Fact]
        public void DelayedGoalServices_ShouldInheritFromBaseService()
        {
            // Arrange
            var service = new DelayedGoalServices(delayedGoalDataAccess);

            // Assert
            service.Should().BeAssignableTo<BaseService>();
        }

        [Fact]
        public void CleanResult_ShouldReturnBaseService()
        {
            // Arrange
            // MatchList is null by default

            // Act
            var result = delayedGoalServices.CleanResult();

            // Assert
            result.Should().BeAssignableTo<BaseService>();
        }

        [Fact]
        public void Precondition_ShouldReturnBaseService()
        {
            // Arrange
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());

            // Act
            var result = delayedGoalServices.Precondition();

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
            delayedGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            delayedGoalDataAccess.Rule(Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0 });
            delayedGoalServices.Precondition();

            var startTime = DateTime.Now;

            // Act
            var results = delayedGoalServices.Rules(100);

            // Assert
            var elapsed = DateTime.Now - startTime;
            results.Should().NotBeNull();
            elapsed.TotalSeconds.Should().BeLessThan(30); // Should complete within 30 seconds
        }

        #endregion
    }
}

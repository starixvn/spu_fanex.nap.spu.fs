using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.BigStateNearGoal
{
    public class BigStateNearGoalTests
    {
        private readonly BigStateNearGoalServices bigStateNearGoalServices;
        private readonly IBigStateNearGoalDataAccess bigStateNearGoalDataAccess;

        public BigStateNearGoalTests()
        {
            bigStateNearGoalDataAccess = Substitute.For<IBigStateNearGoalDataAccess>();
            bigStateNearGoalServices = new BigStateNearGoalServices(bigStateNearGoalDataAccess);
        }

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

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            bigStateNearGoalServices.Precondition();

            // Assert
            bigStateNearGoalServices.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void Precondition_WithEmptyMatches_ShouldReturnEmptyList()
        {
            // Arrange
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());

            // Act
            bigStateNearGoalServices.Precondition();

            // Assert
            bigStateNearGoalServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void Precondition_WithNullMatches_ShouldReturnNull()
        {
            // Arrange
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);

            // Act
            bigStateNearGoalServices.Precondition();

            // Assert
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        #endregion

        #region CleanResult Tests

        [Fact]
        public void CleanResult_WithValidMatches_ShouldClearMatchList()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalServices.Precondition();

            // Act
            bigStateNearGoalServices.CleanResult();

            // Assert
            bigStateNearGoalServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_WithEmptyMatches_ShouldReturnSameInstance()
        {
            // Arrange
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());
            bigStateNearGoalServices.Precondition();

            // Act
            var result = bigStateNearGoalServices.CleanResult();

            // Assert
            result.Should().BeSameAs(bigStateNearGoalServices);
            bigStateNearGoalServices.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_WithNullMatches_ShouldReturnSameInstance()
        {
            // Arrange
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);
            bigStateNearGoalServices.Precondition();

            // Act
            var result = bigStateNearGoalServices.CleanResult();

            // Assert
            result.Should().BeSameAs(bigStateNearGoalServices);
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        #endregion

        #region Rules Tests

        [Fact]
        public void Rules_WithSuccessfulMatches_ShouldReturnEmptyResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };

            var successfulResult = new DataResult { ErrorCode = 0 };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule(Arg.Any<string>()).Returns(successfulResult);

            bigStateNearGoalServices.Precondition();

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().BeEmpty();
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Rules_WithFailedMatches_ShouldReturnErrorResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };

            var failedResult = new DataResult { ErrorCode = -100, ErrorMessage = "Test error" };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule(Arg.Any<string>()).Returns(failedResult);

            bigStateNearGoalServices.Precondition();

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().HaveCount(2);          
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Rules_WithMixedResults_ShouldReturnOnlyFailedResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };

            var successfulResult = new DataResult { ErrorCode = 0 };
            var failedResult = new DataResult { ErrorCode = -100, ErrorMessage = "Test error" };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule("1").Returns(successfulResult);
            bigStateNearGoalDataAccess.Rule("2").Returns(failedResult);
            bigStateNearGoalDataAccess.Rule("3").Returns(successfulResult);

            bigStateNearGoalServices.Precondition();

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().HaveCount(1);
            results.Should().ContainSingle(r => r.ErrorCode == -100);
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Rules_WithEmptyMatches_ShouldReturnEmptyResults()
        {
            // Arrange
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(new List<MatchEntity>());

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().BeEmpty();
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Rules_WithNullMatches_ShouldReturnEmptyResults()
        {
            // Arrange
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns((IEnumerable<MatchEntity>)null);

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().BeEmpty();
            bigStateNearGoalServices.MatchList.Should().BeNull();
        }

        [Fact]
        public void Rules_ShouldCallDataAccessWithCorrectMatchIds()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 123 },
                new MatchEntity { MatchId = 456 }
            };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule(Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0 });

            bigStateNearGoalServices.Precondition();

            // Act
            bigStateNearGoalServices.Rules();

            // Assert
            bigStateNearGoalDataAccess.Received(1).Rule("123");
            bigStateNearGoalDataAccess.Received(1).Rule("456");
        }

        #endregion

        #region Exception Handling Tests

        [Fact]
        public void Rules_WhenDataAccessThrowsException_ShouldReturnErrorResult()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };

            var exception = new Exception("Database connection failed");
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule(Arg.Any<string>()).Throws(exception);

            bigStateNearGoalServices.Precondition();

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().HaveCount(1);
            results.Should().ContainSingle(r => r.ErrorCode == -100);
            results.Should().ContainSingle(r => !string.IsNullOrEmpty(r.ErrorMessage));
        }

        [Fact]
        public void Rules_WhenMultipleExceptionsOccur_ShouldReturnMultipleErrorResults()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };

            var exception = new Exception("Database connection failed");
            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule(Arg.Any<string>()).Throws(exception);

            bigStateNearGoalServices.Precondition();

            // Act
            var results = bigStateNearGoalServices.Rules();

            // Assert
            results.Should().HaveCount(2);           
        }

        #endregion

        #region Performance Tests

        [Fact]
        public void Rules_ShouldIncludeWaitingTime()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 }
            };

            bigStateNearGoalDataAccess.PreCondition<MatchEntity>().Returns(matches);
            bigStateNearGoalDataAccess.Rule(Arg.Any<string>()).Returns(new DataResult { ErrorCode = 0 });

            bigStateNearGoalServices.Precondition();

            // Act
            var stopwatch = Stopwatch.StartNew();
            bigStateNearGoalServices.Rules();
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(2000); // 2 seconds waiting time
        }

        #endregion
    }
}

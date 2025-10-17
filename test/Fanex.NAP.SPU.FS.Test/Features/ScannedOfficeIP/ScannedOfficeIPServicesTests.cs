using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.ScannedOfficeIP
{
    public class ScannedOfficeIPServicesTests
    {
        private readonly ScannedOfficeIPServices services;
        private readonly IScannedOfficeIPDataAccess mockDataAccess;

        public ScannedOfficeIPServicesTests()
        {
            mockDataAccess = Substitute.For<IScannedOfficeIPDataAccess>();
            services = new ScannedOfficeIPServices(mockDataAccess);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDataAccess_ShouldCreateInstance()
        {
            // Arrange
            var dataAccess = Substitute.For<IScannedOfficeIPDataAccess>();

            // Act
            var service = new ScannedOfficeIPServices(dataAccess);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ScannedOfficeIPServices>();
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new ScannedOfficeIPServices(null);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInheritFromBaseService()
        {
            // Arrange
            var dataAccess = Substitute.For<IScannedOfficeIPDataAccess>();

            // Act
            var service = new ScannedOfficeIPServices(dataAccess);

            // Assert
            service.Should().BeAssignableTo<BaseService>();
        }

        #endregion

        #region MatchList Property Tests

        [Fact]
        public void MatchList_Initially_ShouldBeNull()
        {
            // Act & Assert
            services.MatchList.Should().BeNull();
        }

        [Fact]
        public void MatchList_AfterPrecondition_ShouldReturnMatchList()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            services.Precondition();

            // Assert
            services.MatchList.Should().NotBeNull();
            services.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        #endregion

        #region CleanResult Tests

        [Fact]
        public void CleanResult_WhenMatchListIsNull_ShouldReturnSameInstance()
        {
            // Act
            var result = services.CleanResult();

            // Assert
            result.Should().BeSameAs(services);
            services.MatchList.Should().BeNull();
        }

        [Fact]
        public void CleanResult_WhenMatchListIsEmpty_ShouldReturnSameInstance()
        {
            // Arrange
            mockDataAccess.PreCondition<MatchEntity>().Returns(Enumerable.Empty<MatchEntity>());
            services.Precondition();

            // Act
            var result = services.CleanResult();

            // Assert
            result.Should().BeSameAs(services);
            services.MatchList.Should().NotBeNull();
            services.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_WhenMatchListHasItems_ShouldClearMatchList()
        {
            // Arrange
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var result = services.CleanResult();

            // Assert
            result.Should().BeSameAs(services);
            services.MatchList.Should().NotBeNull();
            services.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void CleanResult_ShouldSupportMethodChaining()
        {
            // Arrange
            var matches = new List<MatchEntity> { new MatchEntity { MatchId = 1 } };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var result = services.CleanResult().CleanResult().CleanResult();

            // Assert
            result.Should().BeSameAs(services);
            services.MatchList.Should().BeEmpty();
        }

        #endregion

        #region Precondition Tests

        [Fact]
        public void Precondition_ShouldCallDataAccessPreCondition()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            var result = services.Precondition();

            // Assert
            mockDataAccess.Received(1).PreCondition<MatchEntity>();
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void Precondition_ShouldSetMatchList()
        {
            // Arrange
            var expectedMatches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(expectedMatches);

            // Act
            services.Precondition();

            // Assert
            services.MatchList.Should().NotBeNull();
            services.MatchList.Should().HaveCount(3);
            services.MatchList.Should().BeEquivalentTo(expectedMatches);
        }

        [Fact]
        public void Precondition_WithEmptyResult_ShouldSetEmptyMatchList()
        {
            // Arrange
            mockDataAccess.PreCondition<MatchEntity>().Returns(Enumerable.Empty<MatchEntity>());

            // Act
            services.Precondition();

            // Assert
            services.MatchList.Should().NotBeNull();
            services.MatchList.Should().BeEmpty();
        }

        [Fact]
        public void Precondition_ShouldSupportMethodChaining()
        {
            // Arrange
            var matches = new List<MatchEntity> { new MatchEntity { MatchId = 1 } };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);

            // Act
            var result = services.Precondition().Precondition().Precondition();

            // Assert
            result.Should().BeSameAs(services);
            mockDataAccess.Received(3).PreCondition<MatchEntity>();
        }

        #endregion

        #region Rules Tests

        [Fact]
        public void Rules_WithNullMatchList_ShouldReturnEmptyResults()
        {
            // Arrange
            var chunkSize = 5;

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            mockDataAccess.DidNotReceive().Rule(Arg.Any<string>());
        }

        [Fact]
        public void Rules_WithEmptyMatchList_ShouldReturnEmptyResults()
        {
            // Arrange
            var chunkSize = 5;
            mockDataAccess.PreCondition<MatchEntity>().Returns(Enumerable.Empty<MatchEntity>());
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            mockDataAccess.DidNotReceive().Rule(Arg.Any<string>());
        }

        [Fact]
        public void Rules_WithValidMatchList_ShouldProcessChunks()
        {
            // Arrange
            var chunkSize = 2;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 },
                new MatchEntity { MatchId = 4 },
                new MatchEntity { MatchId = 5 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty(); // No errors expected
            mockDataAccess.Received(3).Rule(Arg.Any<string>()); // 3 chunks: [1,2], [3,4], [5]
        }

        [Fact]
        public void Rules_WithSingleChunk_ShouldProcessCorrectly()
        {
            // Arrange
            var chunkSize = 10;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            mockDataAccess.Received(1).Rule("1,2,3");
        }

        [Fact]
        public void Rules_WithMultipleChunks_ShouldProcessAllChunks()
        {
            // Arrange
            var chunkSize = 2;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 },
                new MatchEntity { MatchId = 4 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            mockDataAccess.Received(2).Rule(Arg.Any<string>());
            mockDataAccess.Received().Rule("1,2");
            mockDataAccess.Received().Rule("3,4");
        }

        [Fact]
        public void Rules_WithExceptionInDataAccess_ShouldReturnErrorResult()
        {
            // Arrange
            var chunkSize = 2;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            mockDataAccess.Rule(Arg.Any<string>()).Throws(new Exception("Database error"));
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            var errorResult = results.First();
            errorResult.ErrorCode.Should().Be(-100);
            errorResult.ErrorMessage.Should().Be("Database error");
        }

        [Fact]
        public void Rules_WithMultipleChunksAndException_ShouldReturnErrorForFailedChunk()
        {
            // Arrange
            var chunkSize = 1;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            mockDataAccess.Rule("1").Returns(new DataResult { ErrorCode = 0 });
            mockDataAccess.Rule("2").Throws(new Exception("Chunk 2 error"));
            mockDataAccess.Rule("3").Returns(new DataResult { ErrorCode = 0 });
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            var errorResult = results.First();
            errorResult.ErrorCode.Should().Be(-100);
            errorResult.ErrorMessage.Should().Be("Chunk 2 error");
        }

        [Fact]
        public void Rules_ShouldSleepForWaitingTime()
        {
            // Arrange
            var chunkSize = 1;
            var matches = new List<MatchEntity> { new MatchEntity { MatchId = 1 } };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            var startTime = DateTime.Now;

            // Act
            services.Rules(chunkSize);

            // Assert
            var elapsed = DateTime.Now - startTime;
            elapsed.TotalSeconds.Should().BeGreaterOrEqualTo(2); // waitingTimeInSecond = 2
        }

        [Fact]
        public void Rules_WithZeroChunkSize_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var chunkSize = 0;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => services.Rules(chunkSize));
        }

        [Fact]
        public void Rules_WithNegativeChunkSize_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var chunkSize = -1;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => services.Rules(chunkSize));
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void FullWorkflow_CleanResult_Precondition_Rules_ShouldWorkCorrectly()
        {
            // Arrange
            var chunkSize = 2;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);

            // Act
            var results = services
                .CleanResult()
                .Precondition()
                .Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            services.MatchList.Should().NotBeNull();
            services.MatchList.Should().HaveCount(3);
            mockDataAccess.Received(1).PreCondition<MatchEntity>();
            mockDataAccess.Received(2).Rule(Arg.Any<string>()); // 2 chunks: [1,2], [3]
        }

        [Fact]
        public void MultipleWorkflowExecutions_ShouldWorkIndependently()
        {
            // Arrange
            var chunkSize = 1;
            var matches1 = new List<MatchEntity> { new MatchEntity { MatchId = 1 } };
            var matches2 = new List<MatchEntity> { new MatchEntity { MatchId = 2 } };
            
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches1, matches2);

            // Act
            var results1 = services.CleanResult().Precondition().Rules(chunkSize);
            var results2 = services.CleanResult().Precondition().Rules(chunkSize);

            // Assert
            results1.Should().NotBeNull();
            results2.Should().NotBeNull();
            mockDataAccess.Received(2).PreCondition<MatchEntity>();
            mockDataAccess.Received().Rule("1");
            mockDataAccess.Received().Rule("2");
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Rules_WithVeryLargeChunkSize_ShouldProcessAllInOneChunk()
        {
            // Arrange
            var chunkSize = int.MaxValue;
            var matches = new List<MatchEntity>
            {
                new MatchEntity { MatchId = 1 },
                new MatchEntity { MatchId = 2 },
                new MatchEntity { MatchId = 3 }
            };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            mockDataAccess.Received(1).Rule("1,2,3");
        }

        [Fact]
        public void Rules_WithSingleMatch_ShouldProcessCorrectly()
        {
            // Arrange
            var chunkSize = 5;
            var matches = new List<MatchEntity> { new MatchEntity { MatchId = 42 } };
            mockDataAccess.PreCondition<MatchEntity>().Returns(matches);
            services.Precondition();

            // Act
            var results = services.Rules(chunkSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
            mockDataAccess.Received(1).Rule("42");
        }

        #endregion
    }
}

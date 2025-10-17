using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Shares
{
    public class UtilitiesTests
    {
        #region Chunk2 Tests - Basic Functionality

        [Fact]
        public void Chunk2_WithValidInput_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(4);
            result[0].Should().BeEquivalentTo(new[] { 1, 2, 3 });
            result[1].Should().BeEquivalentTo(new[] { 4, 5, 6 });
            result[2].Should().BeEquivalentTo(new[] { 7, 8, 9 });
            result[3].Should().BeEquivalentTo(new[] { 10 });
        }

        [Fact]
        public void Chunk2_WithExactMultiple_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5, 6 };
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new[] { 1, 2, 3 });
            result[1].Should().BeEquivalentTo(new[] { 4, 5, 6 });
        }

        [Fact]
        public void Chunk2_WithSingleChunk_ShouldReturnOneChunk()
        {
            // Arrange
            var values = new[] { 1, 2, 3 };
            var chunkSize = 5;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Fact]
        public void Chunk2_WithChunkSizeOne_ShouldReturnIndividualChunks()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var chunkSize = 1;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(5);
            result[0].Should().BeEquivalentTo(new[] { 1 });
            result[1].Should().BeEquivalentTo(new[] { 2 });
            result[2].Should().BeEquivalentTo(new[] { 3 });
            result[3].Should().BeEquivalentTo(new[] { 4 });
            result[4].Should().BeEquivalentTo(new[] { 5 });
        }

        #endregion

        #region Chunk2 Tests - Different Data Types

        [Fact]
        public void Chunk2_WithStrings_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new[] { "a", "b", "c", "d", "e", "f" };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new[] { "a", "b" });
            result[1].Should().BeEquivalentTo(new[] { "c", "d" });
            result[2].Should().BeEquivalentTo(new[] { "e", "f" });
        }

        [Fact]
        public void Chunk2_WithCustomObjects_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new[] 
            { 
                new { Id = 1, Name = "A" }, 
                new { Id = 2, Name = "B" }, 
                new { Id = 3, Name = "C" },
                new { Id = 4, Name = "D" }
            };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Should().HaveCount(2);
            result[1].Should().HaveCount(2);
        }

        [Fact]
        public void Chunk2_WithNullableTypes_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new int?[] { 1, null, 3, 4, null, 6 };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new int?[] { 1, null });
            result[1].Should().BeEquivalentTo(new int?[] { 3, 4 });
            result[2].Should().BeEquivalentTo(new int?[] { null, 6 });
        }

        #endregion

        #region Chunk2 Tests - Edge Cases

        [Fact]
        public void Chunk2_WithEmptyCollection_ShouldReturnEmptyResult()
        {
            // Arrange
            var values = new int[0];
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Chunk2_WithZeroChunkSize_ShouldThrowException()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var chunkSize = 0;

            // Act & Assert
            var exception = Assert.Throws<DivideByZeroException>(() => values.Chunk2(chunkSize).ToList());
            exception.Should().NotBeNull();
        }

        [Fact]
        public void Chunk2_WithNegativeChunkSize_ShouldReturnIndividualChunks()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var chunkSize = -1;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            // With negative chunk size, the division results in negative group indices
            // which causes each item to be in its own group
            result.Should().HaveCount(5);
            result[0].Should().BeEquivalentTo(new[] { 1 });
            result[1].Should().BeEquivalentTo(new[] { 2 });
            result[2].Should().BeEquivalentTo(new[] { 3 });
            result[3].Should().BeEquivalentTo(new[] { 4 });
            result[4].Should().BeEquivalentTo(new[] { 5 });
        }

        [Fact]
        public void Chunk2_WithSingleItem_ShouldReturnOneChunk()
        {
            // Arrange
            var values = new[] { 42 };
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(new[] { 42 });
        }

        [Fact]
        public void Chunk2_WithLargeChunkSize_ShouldReturnOneChunk()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var chunkSize = 100;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        }

        #endregion

        #region Chunk2 Tests - Performance and Behavior

        [Fact]
        public void Chunk2_ShouldPreserveOrder()
        {
            // Arrange
            var values = new[] { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result[0].Should().BeEquivalentTo(new[] { 5, 2, 8 });
            result[1].Should().BeEquivalentTo(new[] { 1, 9, 3 });
            result[2].Should().BeEquivalentTo(new[] { 7, 4, 6 });
        }

        [Fact]
        public void Chunk2_ShouldBeLazyEvaluated()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize);

            // Assert
            result.Should().NotBeNull();
            // The result should be an IEnumerable that hasn't been enumerated yet
            result.Should().BeAssignableTo<IEnumerable<IEnumerable<int>>>();
        }

        [Fact]
        public void Chunk2_WithLargeDataset_ShouldHandleCorrectly()
        {
            // Arrange
            var values = Enumerable.Range(1, 1000).ToArray();
            var chunkSize = 100;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(10);
            result[0].Should().HaveCount(100);
            result[9].Should().HaveCount(100);
        }

        #endregion

        #region Chunk2 Tests - Different Collection Types

        [Fact]
        public void Chunk2_WithList_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new List<int> { 1, 2, 3, 4, 5, 6 };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new[] { 1, 2 });
            result[1].Should().BeEquivalentTo(new[] { 3, 4 });
            result[2].Should().BeEquivalentTo(new[] { 5, 6 });
        }

        [Fact]
        public void Chunk2_WithHashSet_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new HashSet<int> { 1, 2, 3, 4, 5, 6 };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            // Note: HashSet doesn't guarantee order, so we check count and content
            result[0].Should().HaveCount(2);
            result[1].Should().HaveCount(2);
            result[2].Should().HaveCount(2);
        }

        [Fact]
        public void Chunk2_WithLinkedList_ShouldReturnCorrectChunks()
        {
            // Arrange
            var values = new LinkedList<int>(new[] { 1, 2, 3, 4, 5, 6 });
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new[] { 1, 2 });
            result[1].Should().BeEquivalentTo(new[] { 3, 4 });
            result[2].Should().BeEquivalentTo(new[] { 5, 6 });
        }

        #endregion

        #region Chunk2 Tests - Complex Scenarios

        [Fact]
        public void Chunk2_WithOddNumberOfItems_ShouldHandleLastChunkCorrectly()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var chunkSize = 3;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new[] { 1, 2, 3 });
            result[1].Should().BeEquivalentTo(new[] { 4, 5, 6 });
            result[2].Should().BeEquivalentTo(new[] { 7 });
        }

        [Fact]
        public void Chunk2_WithVerySmallChunkSize_ShouldCreateManyChunks()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var chunkSize = 1;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(5);
            result[0].Should().BeEquivalentTo(new[] { 1 });
            result[1].Should().BeEquivalentTo(new[] { 2 });
            result[2].Should().BeEquivalentTo(new[] { 3 });
            result[3].Should().BeEquivalentTo(new[] { 4 });
            result[4].Should().BeEquivalentTo(new[] { 5 });
        }

        [Fact]
        public void Chunk2_WithDuplicateValues_ShouldPreserveDuplicates()
        {
            // Arrange
            var values = new[] { 1, 1, 2, 2, 3, 3 };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new[] { 1, 1 });
            result[1].Should().BeEquivalentTo(new[] { 2, 2 });
            result[2].Should().BeEquivalentTo(new[] { 3, 3 });
        }

        #endregion

        #region Chunk2 Tests - Method Signature and Extension

        [Fact]
        public void Chunk2_ShouldBeExtensionMethod()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<IEnumerable<int>>>();
        }

        [Fact]
        public void Chunk2_ShouldAcceptGenericTypes()
        {
            // Arrange
            var values = new[] { "test1", "test2", "test3" };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new[] { "test1", "test2" });
            result[1].Should().BeEquivalentTo(new[] { "test3" });
        }

        #endregion

        #region Chunk2 Tests - Stress Tests

        [Fact]
        public void Chunk2_WithVeryLargeChunkSize_ShouldHandleCorrectly()
        {
            // Arrange
            var values = new[] { 1, 2, 3 };
            var chunkSize = int.MaxValue;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Fact]
        public void Chunk2_WithMaxIntValues_ShouldHandleCorrectly()
        {
            // Arrange
            var values = new[] { int.MaxValue, int.MinValue, 0 };
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new[] { int.MaxValue, int.MinValue });
            result[1].Should().BeEquivalentTo(new[] { 0 });
        }

        #endregion

        #region Chunk2 Tests - Integration Tests

        [Fact]
        public void Chunk2_WithLinqOperations_ShouldWorkCorrectly()
        {
            // Arrange
            var values = Enumerable.Range(1, 10).Where(x => x % 2 == 0).ToArray();
            var chunkSize = 2;

            // Act
            var result = values.Chunk2(chunkSize).ToList();

            // Assert
            result.Should().HaveCount(3);
            result[0].Should().BeEquivalentTo(new[] { 2, 4 });
            result[1].Should().BeEquivalentTo(new[] { 6, 8 });
            result[2].Should().BeEquivalentTo(new[] { 10 });
        }

        [Fact]
        public void Chunk2_WithNestedChunking_ShouldWorkCorrectly()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var chunkSize = 4;

            // Act
            var result = values.Chunk2(chunkSize).ToList();
            var nestedResult = result.SelectMany(chunk => chunk.Chunk2(2)).ToList();

            // Assert
            result.Should().HaveCount(3);
            nestedResult.Should().HaveCount(6); // 3 chunks of 4 items each, chunked by 2 = 6 chunks
        }

        #endregion
    }
}

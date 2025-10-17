using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Features.FraudCase.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.FraudCase
{
    public class FraudCaseTests
    {
        private readonly IFraudCaseDataAccess fraudCaseDataAccess;
        private readonly IFraudCaseServices fraudCaseServices;

        public FraudCaseTests()
        {
            fraudCaseDataAccess = Substitute.For<IFraudCaseDataAccess>();
            fraudCaseServices = new FraudCaseServices(fraudCaseDataAccess);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new FraudCaseServices(fraudCaseDataAccess);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new FraudCaseServices(null);

            // Assert
            service.Should().NotBeNull();
        }

        #endregion

        #region Active Fraud Case Tests

        [Fact]
        public void CheckFraudCaseIsActive_WithActiveFraudCase_ShouldReturnTrue()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Test Fraud Case",
                IsActive = true,
                FraudCaseDesc = "Test Description",
                FraudCaseInterval = 3600,
                IsTest = false
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithActiveTestFraudCase_ShouldReturnTrue()
        {
            // Arrange
            var fraudCaseId = 2;
            var environment = 2;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Test Fraud Case",
                IsActive = true,
                FraudCaseDesc = "Test Description",
                FraudCaseInterval = 1800,
                IsTest = true
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region Inactive Fraud Case Tests

        [Fact]
        public void CheckFraudCaseIsActive_WithInactiveFraudCase_ShouldReturnFalse()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Inactive Fraud Case",
                IsActive = false,
                FraudCaseDesc = "Inactive Description",
                FraudCaseInterval = 3600,
                IsTest = false
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithInactiveTestFraudCase_ShouldReturnFalse()
        {
            // Arrange
            var fraudCaseId = 2;
            var environment = 2;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Inactive Test Fraud Case",
                IsActive = false,
                FraudCaseDesc = "Inactive Test Description",
                FraudCaseInterval = 1800,
                IsTest = true
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Null Fraud Case Tests

        [Fact]
        public void CheckFraudCaseIsActive_WithNullFraudCase_ShouldReturnFalse()
        {
            // Arrange
            var fraudCaseId = 999;
            var environment = 1;

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns((FraudCaseEntity)null);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithNonExistentFraudCase_ShouldReturnFalse()
        {
            // Arrange
            var fraudCaseId = 0;
            var environment = 1;

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns((FraudCaseEntity)null);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Data Access Verification Tests

        [Fact]
        public void CheckFraudCaseIsActive_ShouldCallDataAccessWithCorrectParameters()
        {
            // Arrange
            var fraudCaseId = 123;
            var environment = 456;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                IsActive = true
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            fraudCaseDataAccess.Received(1).GetFraudCaseById(fraudCaseId, environment);
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithMultipleCalls_ShouldCallDataAccessEachTime()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                IsActive = true
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);
            fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);
            fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            fraudCaseDataAccess.Received(3).GetFraudCaseById(fraudCaseId, environment);
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public void CheckFraudCaseIsActive_WithZeroFraudCaseId_ShouldCallDataAccess()
        {
            // Arrange
            var fraudCaseId = 0;
            var environment = 1;

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns((FraudCaseEntity)null);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
            fraudCaseDataAccess.Received(1).GetFraudCaseById(fraudCaseId, environment);
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithNegativeFraudCaseId_ShouldCallDataAccess()
        {
            // Arrange
            var fraudCaseId = -1;
            var environment = 1;

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns((FraudCaseEntity)null);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
            fraudCaseDataAccess.Received(1).GetFraudCaseById(fraudCaseId, environment);
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithZeroEnvironment_ShouldCallDataAccess()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 0;

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns((FraudCaseEntity)null);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
            fraudCaseDataAccess.Received(1).GetFraudCaseById(fraudCaseId, environment);
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithNegativeEnvironment_ShouldCallDataAccess()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = -1;

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns((FraudCaseEntity)null);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
            fraudCaseDataAccess.Received(1).GetFraudCaseById(fraudCaseId, environment);
        }

        #endregion

        #region Different Environment Tests

        [Fact]
        public void CheckFraudCaseIsActive_WithDifferentEnvironments_ShouldCallDataAccessWithCorrectEnvironment()
        {
            // Arrange
            var fraudCaseId = 1;
            var environments = new[] { 1, 2, 3, 4, 5 };

            foreach (var environment in environments)
            {
                var fraudCaseEntity = new FraudCaseEntity
                {
                    FraudCaseId = fraudCaseId,
                    IsActive = true
                };

                fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

                // Act
                var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

                // Assert
                result.Should().BeTrue();
                fraudCaseDataAccess.Received(1).GetFraudCaseById(fraudCaseId, environment);
            }
        }

        #endregion

        #region Fraud Case Entity Properties Tests

        [Fact]
        public void CheckFraudCaseIsActive_WithCompleteFraudCaseEntity_ShouldReturnCorrectResult()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Complete Fraud Case",
                FraudCaseDesc = "Complete Description",
                FraudCaseInterval = 7200,
                IsActive = true,
                IsTest = false
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CheckFraudCaseIsActive_WithEmptyFraudCaseEntity_ShouldReturnFalse()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity();

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            // Act
            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Exception Handling Tests

        [Fact]
        public void CheckFraudCaseIsActive_WhenDataAccessThrowsException_ShouldThrowException()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;
            var exception = new Exception("Data access error");

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => 
                fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment));
            
            thrownException.Should().Be(exception);
        }

        [Fact]
        public void CheckFraudCaseIsActive_WhenDataAccessThrowsArgumentException_ShouldThrowException()
        {
            // Arrange
            var fraudCaseId = 1;
            var environment = 1;
            var exception = new ArgumentException("Invalid argument");

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Throws(exception);

            // Act & Assert
            var thrownException = Assert.Throws<ArgumentException>(() => 
                fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment));
            
            thrownException.Should().Be(exception);
        }

        #endregion
    }
}

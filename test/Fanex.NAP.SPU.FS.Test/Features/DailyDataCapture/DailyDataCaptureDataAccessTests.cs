using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
using Fanex.NAP.SPU.FS.Features.DailyDataCapture.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.DailyDataCapture
{
    public class DailyDataCaptureDataAccessTests
    {
        private readonly DailyDataCaptureDataAccess dataAccess;

        public DailyDataCaptureDataAccessTests()
        {
            dataAccess = new DailyDataCaptureDataAccess();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var instance = new DailyDataCaptureDataAccess();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository()
        {
            // Arrange & Act
            var instance = new DailyDataCaptureDataAccess();

            // Assert
            instance.Should().NotBeNull();
            // Note: Repository is private, so we can't directly test it
            // but we can test that the instance is created successfully
        }

        #endregion
      
        #region Interface Implementation Tests

        [Fact]
        public void DailyDataCaptureDataAccess_ShouldImplementInterface()
        {
            // Arrange
            var dataAccess = new DailyDataCaptureDataAccess();

            // Assert
            dataAccess.Should().BeAssignableTo<IDailyDataCaptureDataAccess>();
        }
       
        #endregion

        #region Criteria Integration Tests

        [Fact]
        public void DailyDataCaptureCriteria_ShouldHaveCorrectSettingKey()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Be("FS_FraudCase_DailyDataMonitoring_Rule");
        }

        [Fact]
        public void DailyDataCaptureCriteria_ShouldBeValid()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion                       

        #region Domain-Specific Tests

        [Fact]
        public void Rule_ShouldBeRelatedToDailyDataMonitoring()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("DailyDataMonitoring");
            settingKey.Should().Contain("FS_FraudCase");
        }

        [Fact]
        public void Rule_ShouldUseCorrectFraudCaseContext()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("Rule");
        }

        #endregion

        #region Criteria Validation Tests

        [Fact]
        public void DailyDataCaptureCriteria_ShouldInheritFromNonQueryCommand()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Assert
            criteria.Should().BeAssignableTo<NonQueryCommand>();
        }

        [Fact]
        public void DailyDataCaptureCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_DailyDataMonitoring_Rule");
        }

        [Fact]
        public void DailyDataCaptureCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion              

        #region Additional DailyDataCaptureCriteria Validation Tests

        [Fact]
        public void DailyDataCaptureCriteria_ShouldContainCorrectDomainElements()
        {
            // Arrange
            var criteria = new DailyDataCaptureCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("DailyDataMonitoring");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("Rule");
        }

        #endregion
    }
}

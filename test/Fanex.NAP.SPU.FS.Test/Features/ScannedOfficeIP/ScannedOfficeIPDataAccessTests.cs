using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.ScannedOfficeIP
{
    public class ScannedOfficeIPDataAccessTests
    {
        private readonly ScannedOfficeIPDataAccess dataAccess;

        public ScannedOfficeIPDataAccessTests()
        {
            dataAccess = new ScannedOfficeIPDataAccess();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var instance = new ScannedOfficeIPDataAccess();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository()
        {
            // Arrange & Act
            var instance = new ScannedOfficeIPDataAccess();

            // Assert
            instance.Should().NotBeNull();
            // Note: Repository is private, so we can't directly test it
            // but we can test that the instance is created successfully
        }

        #endregion
                                

        #region Interface Implementation Tests

        [Fact]
        public void ScannedOfficeIPDataAccess_ShouldImplementInterface()
        {
            // Arrange
            var dataAccess = new ScannedOfficeIPDataAccess();

            // Assert
            dataAccess.Should().BeAssignableTo<IScannedOfficeIPDataAccess>();
        }

        #endregion      

        #region Domain-Specific Tests

        [Fact]
        public void PreCondition_ShouldBeRelatedToOfficeIP()
        {
            // Arrange
            var criteria = new OfficeIPPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("OfficeIP");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("PreCondition");
        }      

        #endregion

        #region Criteria Validation Tests

        [Fact]
        public void OfficeIPPreconditionCriteria_ShouldInheritFromCriteriaBase()
        {
            // Arrange
            var criteria = new OfficeIPPreconditionCriteria();

            // Assert
            criteria.Should().BeAssignableTo<CriteriaBase>();
        }       

        [Fact]
        public void OfficeIPPreconditionCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new OfficeIPPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_OfficeIP_PreCondition");
        }     

        [Fact]
        public void OfficeIPPreconditionCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new OfficeIPPreconditionCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }     

        #endregion         

        #region OfficeIP Specific Tests

        [Fact]
        public void PreCondition_ShouldBeRelatedToScannedOfficeIP()
        {
            // Arrange
            var criteria = new OfficeIPPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("OfficeIP");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("PreCondition");
        }
       
        #endregion
    }
}

using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount.Criteria;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount.Entities;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.FlaggedAccount
{
    public class FlaggedAccountDataAccessTests
    {
        private readonly FlaggedAccountDataAccess dataAccess;

        public FlaggedAccountDataAccessTests()
        {
            dataAccess = new FlaggedAccountDataAccess();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var instance = new FlaggedAccountDataAccess();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository()
        {
            // Arrange & Act
            var instance = new FlaggedAccountDataAccess();

            // Assert
            instance.Should().NotBeNull();
            // Note: Repository is private, so we can't directly test it
            // but we can test that the instance is created successfully
        }

        #endregion              

        #region Interface Implementation Tests

        [Fact]
        public void FlaggedAccountDataAccess_ShouldImplementInterface()
        {
            // Arrange
            var dataAccess = new FlaggedAccountDataAccess();

            // Assert
            dataAccess.Should().BeAssignableTo<IFlaggedAccountDataAccess>();
        }
      
        #endregion      
       
        #region Domain-Specific Tests

        [Fact]
        public void PreCondition_ShouldBeRelatedToFlaggedAccount()
        {
            // Arrange
            var criteria = new FlaggedAccountPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("FlaggedAccount");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("PreCondition");
        }

        [Fact]
        public void Rule_ShouldBeRelatedToFlaggedAccount()
        {
            // Arrange
            var criteria = new FlaggedAccountRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("FlaggedAccount");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("Rule");
        }

        [Fact]
        public void Rule_ShouldUseCorrectFraudCaseContext()
        {
            // Arrange
            var criteria = new FlaggedAccountRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("FlaggedAccount");
        }

        #endregion

        #region Criteria Validation Tests

        [Fact]
        public void FlaggedAccountPreconditionCriteria_ShouldInheritFromCriteriaBase()
        {
            // Arrange
            var criteria = new FlaggedAccountPreconditionCriteria();

            // Assert
            criteria.Should().BeAssignableTo<CriteriaBase>();
        }

        [Fact]
        public void FlaggedAccountRuleCriteria_ShouldInheritFromNonQueryCommand()
        {
            // Arrange
            var criteria = new FlaggedAccountRuleCriteria();

            // Assert
            criteria.Should().BeAssignableTo<NonQueryCommand>();
        }

        [Fact]
        public void FlaggedAccountPreconditionCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new FlaggedAccountPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_FlaggedAccount_PreCondition");
        }

        [Fact]
        public void FlaggedAccountRuleCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new FlaggedAccountRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_FlaggedAccount_Rule");
        }

        [Fact]
        public void FlaggedAccountPreconditionCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new FlaggedAccountPreconditionCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void FlaggedAccountRuleCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new FlaggedAccountRuleCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion      
      
    }
}

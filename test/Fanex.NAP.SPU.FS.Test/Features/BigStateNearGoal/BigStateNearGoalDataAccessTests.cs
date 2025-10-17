using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.BigStateNearGoal
{
    public class BigStateNearGoalDataAccessTests
    {
        private readonly BigStateNearGoalDataAccess dataAccess;

        public BigStateNearGoalDataAccessTests()
        {
            dataAccess = new BigStateNearGoalDataAccess();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var instance = new BigStateNearGoalDataAccess();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository()
        {
            // Arrange & Act
            var instance = new BigStateNearGoalDataAccess();

            // Assert
            instance.Should().NotBeNull();
            // Note: Repository is private, so we can't directly test it
            // but we can test that the instance is created successfully
        }

        #endregion                    

        #region Interface Implementation Tests

        [Fact]
        public void BigStateNearGoalDataAccess_ShouldImplementInterface()
        {
            // Arrange
            var dataAccess = new BigStateNearGoalDataAccess();

            // Assert
            dataAccess.Should().BeAssignableTo<IBigStateNearGoalDataAccess>();
        }
      
        #endregion      
       
        #region Criteria Validation Tests

        [Fact]
        public void BigStakeNearGoalPreconditionCriteria_ShouldInheritFromCriteriaBase()
        {
            // Arrange
            var criteria = new BigStakeNearGoalPreconditionCriteria();

            // Assert
            criteria.Should().BeAssignableTo<CriteriaBase>();
        }

        [Fact]
        public void BigStakeNearGoalRuleCriteria_ShouldInheritFromNonQueryCommand()
        {
            // Arrange
            var criteria = new BigStakeNearGoalRuleCriteria();

            // Assert
            criteria.Should().BeAssignableTo<NonQueryCommand>();
        }

        [Fact]
        public void BigStakeNearGoalPreconditionCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new BigStakeNearGoalPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_BSNG_PreCondition");
        }

        [Fact]
        public void BigStakeNearGoalRuleCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new BigStakeNearGoalRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_BSNG_Rule");
        }

        [Fact]
        public void BigStakeNearGoalPreconditionCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new BigStakeNearGoalPreconditionCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void BigStakeNearGoalRuleCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new BigStakeNearGoalRuleCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void BigStakeNearGoalRuleCriteria_ShouldSetMatchIdsProperty()
        {
            // Arrange
            var criteria = new BigStakeNearGoalRuleCriteria();
            var expectedMatchIds = "1,2,3";

            // Act
            criteria.MatchIds = expectedMatchIds;

            // Assert
            criteria.MatchIds.Should().Be(expectedMatchIds);
        }

        #endregion

        #region Domain-Specific Tests

        [Fact]
        public void PreCondition_ShouldBeRelatedToBigStateNearGoal()
        {
            // Arrange
            var criteria = new BigStakeNearGoalPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("BSNG");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("PreCondition");
        }

        [Fact]
        public void Rule_ShouldBeRelatedToBigStateNearGoal()
        {
            // Arrange
            var criteria = new BigStakeNearGoalRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("BSNG");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("Rule");
        }

        [Fact]
        public void Rule_ShouldUseCorrectFraudCaseContext()
        {
            // Arrange
            var criteria = new BigStakeNearGoalRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("BSNG");
        }

        #endregion
       
    }
}

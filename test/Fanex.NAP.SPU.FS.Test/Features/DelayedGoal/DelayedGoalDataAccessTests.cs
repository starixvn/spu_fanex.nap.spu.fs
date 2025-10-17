using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.DelayedGoal;
using Fanex.NAP.SPU.FS.Features.DelayedGoal.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.DelayedGoal
{
    public class DelayedGoalDataAccessTests
    {
        private readonly DelayedGoalDataAccess dataAccess;

        public DelayedGoalDataAccessTests()
        {
            dataAccess = new DelayedGoalDataAccess();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var instance = new DelayedGoalDataAccess();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository()
        {
            // Arrange & Act
            var instance = new DelayedGoalDataAccess();

            // Assert
            instance.Should().NotBeNull();
            // Note: Repository is private, so we can't directly test it
            // but we can test that the instance is created successfully
        }

        #endregion
           
        #region Interface Implementation Tests

        [Fact]
        public void DelayedGoalDataAccess_ShouldImplementInterface()
        {
            // Arrange
            var dataAccess = new DelayedGoalDataAccess();

            // Assert
            dataAccess.Should().BeAssignableTo<IDelayedGoalDataAccess>();
        }  

        #endregion       
       
        #region Domain-Specific Tests

        [Fact]
        public void PreCondition_ShouldBeRelatedToDelayGoal()
        {
            // Arrange
            var criteria = new DelayGoalPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("DelayGoal");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("PreCondition");
        }

        [Fact]
        public void Rule_ShouldBeRelatedToDelayGoal()
        {
            // Arrange
            var criteria = new DelayGoalRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("DelayGoal");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("Rule");
        }

        [Fact]
        public void Rule_ShouldUseCorrectFraudCaseContext()
        {
            // Arrange
            var criteria = new DelayGoalRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("DelayGoal");
        }

        #endregion

        #region Criteria Validation Tests

        [Fact]
        public void DelayGoalPreconditionCriteria_ShouldInheritFromCriteriaBase()
        {
            // Arrange
            var criteria = new DelayGoalPreconditionCriteria();

            // Assert
            criteria.Should().BeAssignableTo<CriteriaBase>();
        }

        [Fact]
        public void DelayGoalRuleCriteria_ShouldInheritFromNonQueryCommand()
        {
            // Arrange
            var criteria = new DelayGoalRuleCriteria();

            // Assert
            criteria.Should().BeAssignableTo<NonQueryCommand>();
        }

        [Fact]
        public void DelayGoalPreconditionCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new DelayGoalPreconditionCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_DelayGoal_PreCondition");
        }

        [Fact]
        public void DelayGoalRuleCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new DelayGoalRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_DelayGoal_Rule");
        }

        [Fact]
        public void DelayGoalPreconditionCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new DelayGoalPreconditionCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void DelayGoalRuleCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new DelayGoalRuleCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion              
       
    }
}

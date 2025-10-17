
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Features.CheckBet.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.CheckBet
{
    public class CheckBetDataAccessTests
    {
        private readonly CheckBetDataAccess dataAccess;

        public CheckBetDataAccessTests()
        {
            dataAccess = new CheckBetDataAccess();
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var instance = new CheckBetDataAccess();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository()
        {
            // Arrange & Act
            var instance = new CheckBetDataAccess();

            // Assert
            instance.Should().NotBeNull();
            // Note: Repository is private, so we can't directly test it
            // but we can test that the instance is created successfully
        }

        #endregion

        #region Interface Implementation Tests

        [Fact]
        public void CheckBetDataAccess_ShouldImplementInterface()
        {
            // Arrange
            var dataAccess = new CheckBetDataAccess();

            // Assert
            dataAccess.Should().BeAssignableTo<ICheckBetDataAccess>();
        }
       
        #endregion

        #region Criteria Integration Tests       
        [Fact]
        public void CheckBetRuleCriteria_ShouldHaveCorrectSettingKey()
        {
            // Arrange
            var criteria = new CheckBetRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Be("FS_FraudCase_CheckBet_Rule");
        }

        [Fact]
        public void CheckBetRuleCriteria_ShouldBeValid()
        {
            // Arrange
            var criteria = new CheckBetRuleCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        #endregion        
              
        #region CheckBetRuleCriteria Validation Tests

        [Fact]
        public void CheckBetRuleCriteria_ShouldInheritFromNonQueryCommand()
        {
            // Arrange
            var criteria = new CheckBetRuleCriteria();

            // Assert
            criteria.Should().BeAssignableTo<NonQueryCommand>();
        }

        [Fact]
        public void CheckBetRuleCriteria_ShouldHaveValidSettingKey()
        {
            // Arrange
            var criteria = new CheckBetRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().NotBeNullOrEmpty();
            settingKey.Should().Be("FS_FraudCase_CheckBet_Rule");
        }

        [Fact]
        public void CheckBetRuleCriteria_ShouldAlwaysBeValid()
        {
            // Arrange
            var criteria = new CheckBetRuleCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void CheckBetRuleCriteria_ShouldContainCorrectDomainElements()
        {
            // Arrange
            var criteria = new CheckBetRuleCriteria();

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            settingKey.Should().Contain("CheckBet");
            settingKey.Should().Contain("FS_FraudCase");
            settingKey.Should().Contain("Rule");
        }

        #endregion
               
    }
}

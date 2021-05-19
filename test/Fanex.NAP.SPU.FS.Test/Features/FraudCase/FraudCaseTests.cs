using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Features.FraudCase.Entities;
using FluentAssertions;
using NSubstitute;
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

        [Fact]
        public void CheckExistingFraudCase_Successful_ReturnTrue()
        {
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Test",
                IsActive = true,
                FraudCaseDesc = string.Empty,
                FraudCaseInterval = 1,
                IsTest = false                
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            result.Should().BeTrue();
        }

        [Fact]
        public void CheckExistingFraudCase_Successful_ReturnFalse()
        {
            var fraudCaseId = 1;
            var environment = 1;

            var fraudCaseEntity = new FraudCaseEntity
            {
                FraudCaseId = fraudCaseId,
                FraudCaseName = "Test",
                IsActive = false,
                FraudCaseDesc = string.Empty,
                FraudCaseInterval = 1,
                IsTest = false
            };

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(fraudCaseEntity);

            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            result.Should().BeFalse();
        }

        [Fact]
        public void CheckExistingFraudCase_Failed_ReturnFalse()
        {
            var fraudCaseId = 1;
            var environment = 1;            

            fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment).Returns(new FraudCaseEntity());

            var result = fraudCaseServices.CheckFraudCaseIsActive(fraudCaseId, environment);

            result.Should().BeFalse();
        }

    }
}

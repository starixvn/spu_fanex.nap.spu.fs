using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.CheckBet
{
    public class CheckBetTests
    {
        private readonly CheckBetServices checkBetServices;
        private readonly ICheckBetDataAccess checkBetDataAccess;

        public CheckBetTests()
        {
            checkBetDataAccess = Substitute.For<ICheckBetDataAccess>();

            checkBetServices = new CheckBetServices(checkBetDataAccess);
        }

        [Fact]
        public void ExecuteRule_Successful_ReturnDataResult()
        {
            var dataResult = new DataResult
            {
                ErrorCode = 0
            };

            checkBetDataAccess.Rule().Returns(dataResult);

            var result = checkBetServices.Rule();

            result.Should().Be(dataResult);
        }
    }
}

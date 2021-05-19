using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
using Fanex.NAP.SPU.FS.Shares;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Fanex.NAP.SPU.FS.Tests.Features.DailyDataCapture
{
    public class DailyDataCaptureTests
    {
        private readonly DailyDataCaptureServices dailyDataCaptureServices;
        private readonly IDailyDataCaptureDataAccess dailyDataCaptureDataAccess;

        public DailyDataCaptureTests()
        {
            dailyDataCaptureDataAccess = Substitute.For<IDailyDataCaptureDataAccess>();

            dailyDataCaptureServices = new DailyDataCaptureServices(dailyDataCaptureDataAccess);
        }

        [Fact]
        public void ExecuteRule_Successful_ReturnDataResult()
        {
            var dataResult = new DataResult
            {
                ErrorCode = 0
            };

            dailyDataCaptureDataAccess.Rule().Returns(dataResult);

            var result = dailyDataCaptureServices.Rule();

            result.Should().Be(dataResult);
        }
    }
}

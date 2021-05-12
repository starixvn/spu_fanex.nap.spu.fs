using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.FS.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.DailyDataCapture
{
   
   public class DailyDataCaptureServices: BaseService
    {
        private readonly IDailyDataCaptureDataAccess dailyDataCaptureDataAccess;

        public DailyDataCaptureServices(IDailyDataCaptureDataAccess dailyDataCaptureDataAccess)
        {
            this.dailyDataCaptureDataAccess = dailyDataCaptureDataAccess;
        }

        public override DataResult Rule()
        {
            try
            {
                return this.dailyDataCaptureDataAccess.Rule();

            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                return new DataResult
                {
                    ErrorCode = -100,
                    ErrorMessage = logMessage
                };
            }
        }
    }
}

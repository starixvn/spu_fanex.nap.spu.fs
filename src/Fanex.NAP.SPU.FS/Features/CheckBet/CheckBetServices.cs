using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.FS.Shares;
using System;

namespace Fanex.NAP.SPU.FS.Features.CheckBet
{
    public class CheckBetServices : BaseService
    {
        private readonly ICheckBetDataAccess checkbetDataAccess;

        public CheckBetServices(ICheckBetDataAccess checkbetDataAccess)
        {
            this.checkbetDataAccess = checkbetDataAccess;
        }

        public override DataResult Rule()
        {
            try
            {
                return this.checkbetDataAccess.Rule();

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

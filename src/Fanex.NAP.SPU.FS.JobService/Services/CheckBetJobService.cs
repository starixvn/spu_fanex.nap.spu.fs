namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.FS.Features.CheckBet;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using System.Threading.Tasks;

    public interface ICheckBetJobService
    {
        Task<bool> Execute();
    }

    public class CheckBetJobService: ICheckBetJobService
    {
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;

        private readonly CheckBetServices checkbetService;
        private readonly int fraudCaseId = 3;

        public CheckBetJobService(
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            CheckBetServices checkbetService)
        {
            this.checkbetService = checkbetService;
            this.fraudCaseService = fraudCaseService;
            this.appSettings = appSettings;
        }

        public Task<bool> Execute()
        {
            try
            {                
                var isActiveFraudCase = fraudCaseService.CheckFraudCaseIsActive(fraudCaseId, appSettings.Environment);

                if (isActiveFraudCase)
                {
                    JobRun();
                }
            }
            catch (System.Exception ex)
            {
                LogService.Error($"Error in {nameof(CheckBetJobService)}: {ex.Message}", ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun()
        {            
            var result = this.checkbetService.Rule();                        

            if (result.ErrorCode == -100)
            {
                LogService.Error(result.ErrorMessage);
            }
        }
    }
}

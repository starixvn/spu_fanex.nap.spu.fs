namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IFlaggedAccountJobService
    {
        Task<bool> Execute(int chunkSize);
    }

    public class FlaggedAccountJobService : IFlaggedAccountJobService
    {
        private readonly FlaggedAccountServices flaggedAccountServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 6;

        public FlaggedAccountJobService(
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            FlaggedAccountServices flaggedAccountServices)
        {
            this.flaggedAccountServices = flaggedAccountServices;
            this.fraudCaseService = fraudCaseService;
            this.appSettings = appSettings;
        }

        public Task<bool> Execute(int chunkSize)
        {
            try
            {
                var isActiveFraudCase = fraudCaseService.CheckFraudCaseIsActive(fraudCaseId, appSettings.Environment);

                if (isActiveFraudCase)
                {
                    JobRun(chunkSize);
                }
            }
            catch (System.Exception ex)
            {
                LogService.Error($"Error in {nameof(FlaggedAccountJobService)}: {ex.Message}", ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun(int chunkSize)
        {
            var results = flaggedAccountServices
                 .CleanResult()
                 .Precondition()
                 .GetFlaggedCustomer(appSettings.CustomerEvidenceAPI)
                 .Rules(chunkSize);

            if (results != null && results.Any())
            {
                foreach (var result in results.Distinct())
                {
                    if (result.ErrorCode == -100)
                    {
                        LogService.Error(result.ErrorMessage);
                    }
                }
            }
        }
    }
}

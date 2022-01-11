namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using Sentry;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IFlaggedAccountJobService
    {
        Task<bool> Execute(int chunkSize);
    }

    public class FlaggedAccountJobService : IFlaggedAccountJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly FlaggedAccountServices flaggedAccountServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 6;

        public FlaggedAccountJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            FlaggedAccountServices flaggedAccountServices)
        {
            this.flaggedAccountServices = flaggedAccountServices;
            this.sentryClient = sentryClient;
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
                sentryClient.CaptureException(ex);

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
                        sentryClient.CaptureMessage(result.ErrorMessage, Sentry.Protocol.SentryLevel.Error);
                    }
                }
            }
        }
    }
}

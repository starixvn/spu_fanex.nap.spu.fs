namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using Sentry;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBigStateNearGoalJobService
    {
        Task<bool> Execute();

    }

    public class BigStateNearGoalJobService : IBigStateNearGoalJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly BigStateNearGoalServices bigStateNearGoalService;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 1;

        public BigStateNearGoalJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            BigStateNearGoalServices bigStateNearGoalService)
        {
            this.bigStateNearGoalService = bigStateNearGoalService;
            this.sentryClient = sentryClient;
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
                sentryClient.CaptureException(ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun()
        {
            var results = bigStateNearGoalService
                 .CleanResult()
                 .Precondition()
                 .Rules();

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

using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Shares;
using Sentry;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.JobService.Services
{
    public interface IBigStateNearGoalJobService
    {
        Task<bool> Execute(int fraudCaseId);

    }
    public class BigStateNearGoalJobService : IBigStateNearGoalJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IBaseService bigStateNearGoalService;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;

        public BigStateNearGoalJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IBaseService bigStateNearGoalService,
            IFraudCaseServices fraudCaseService)
        {
            this.sentryClient = sentryClient;
            this.bigStateNearGoalService = bigStateNearGoalService;
            this.fraudCaseService = fraudCaseService;
            this.appSettings = appSettings;
        }

        public Task<bool> Execute(int fraudCaseId)
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
            var result = bigStateNearGoalService
                 .CleanResult()
                 .Precondition()
                 .Rule();

            if(result.ErrorCode != 0)
            {
                sentryClient.CaptureMessage(result.ErrorMessage, Sentry.Protocol.SentryLevel.Error);
            }
        }
    }
}

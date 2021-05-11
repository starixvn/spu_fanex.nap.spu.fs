using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Sentry;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.JobService.Services
{
    public interface ICheckBetJobService
    {
        Task<bool> Execute(int fraudCaseId);
    }

    public class CheckBetJobService: ICheckBetJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;

        private readonly CheckBetServices checkbetService;

        public CheckBetJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            CheckBetServices checkbetService)
        {
            this.sentryClient = sentryClient;
            this.checkbetService = checkbetService;
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
            var result = this.checkbetService.Rule();

            if(result.ErrorCode != 0)
            {
                this.sentryClient.CaptureMessage(result.ErrorMessage, Sentry.Protocol.SentryLevel.Error);
            }
        }
    }
}

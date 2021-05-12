using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Sentry;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.JobService.Services
{
    public interface IDailyCaptureDataJobService
    {
        Task<bool> Execute(int fraudCaseId);
    }

    public class DailyCaptureDataJobService: IDailyCaptureDataJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly DailyDataCaptureServices dailyDataCaptureServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;

        public DailyCaptureDataJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            DailyDataCaptureServices dailyDataCaptureServices)
        {
            this.dailyDataCaptureServices = dailyDataCaptureServices;
            this.sentryClient = sentryClient;
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
            var result = this.dailyDataCaptureServices.Rule();

            if (result.ErrorCode != 0)
            {
                this.sentryClient.CaptureMessage(result.ErrorMessage, Sentry.Protocol.SentryLevel.Error);
            }
        }
    }
}

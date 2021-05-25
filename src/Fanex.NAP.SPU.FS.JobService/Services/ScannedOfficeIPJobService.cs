using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
using Sentry;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.JobService.Services
{
    public interface IScannedOfficeIPJobService
    {
        Task<bool> Execute(int chunkSize);
    }

    public class ScannedOfficeIPJobService: IScannedOfficeIPJobService
    {
        private readonly ISentryClient sentryClient;
        private readonly ScannedOfficeIPServices scannedOfficeIPServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 2;

        public ScannedOfficeIPJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            ScannedOfficeIPServices scannedOfficeIPServices)
        {
            this.scannedOfficeIPServices = scannedOfficeIPServices;
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
            var results = scannedOfficeIPServices
                 .CleanResult()
                 .Precondition()                 
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

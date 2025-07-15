namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IScannedOfficeIPJobService
    {
        Task<bool> Execute(int chunkSize);
    }

    public class ScannedOfficeIPJobService: IScannedOfficeIPJobService
    {
        private readonly ScannedOfficeIPServices scannedOfficeIPServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 2;

        public ScannedOfficeIPJobService(
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            ScannedOfficeIPServices scannedOfficeIPServices)
        {
            this.scannedOfficeIPServices = scannedOfficeIPServices;
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
                LogService.Error($"Error in {nameof(ScannedOfficeIPJobService)}: {ex.Message}", ex);

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
                        LogService.Error(result.ErrorMessage);
                    }
                }
            }
        }
    }
}

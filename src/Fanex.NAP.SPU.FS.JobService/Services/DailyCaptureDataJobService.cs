namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using System.Threading.Tasks;

    public interface IDailyCaptureDataJobService
    {
        Task<bool> Execute();
    }

    public class DailyCaptureDataJobService: IDailyCaptureDataJobService
    {
        private readonly DailyDataCaptureServices dailyDataCaptureServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 4;

        public DailyCaptureDataJobService(
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            DailyDataCaptureServices dailyDataCaptureServices)
        {
            this.dailyDataCaptureServices = dailyDataCaptureServices;
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
                LogService.Error($"Error in {nameof(DailyCaptureDataJobService)}: {ex.Message}", ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }


        private void JobRun()
        {
            var result = this.dailyDataCaptureServices.Rule();

            if (result.ErrorCode == -100)
            {
                LogService.Error(result.ErrorMessage);
            }
        }
    }
}

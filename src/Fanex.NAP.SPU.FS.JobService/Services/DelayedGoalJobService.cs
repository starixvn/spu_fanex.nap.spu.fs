namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.FS.Features.DelayedGoal;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDelayedGoalJobService
    {
        Task<bool> Execute(int chunkSize);
    }

    public class DelayedGoalJobService : IDelayedGoalJobService
    {

        private readonly DelayedGoalServices delayedGoalServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;
        private readonly int fraudCaseId = 8;

        public DelayedGoalJobService(
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            DelayedGoalServices delayedGoalServices)
        {
            this.delayedGoalServices = delayedGoalServices;
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
                LogService.Error($"Error in {nameof(DelayedGoalJobService)}: {ex.Message}", ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun(int chunkSize)
        {
            var results = delayedGoalServices
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

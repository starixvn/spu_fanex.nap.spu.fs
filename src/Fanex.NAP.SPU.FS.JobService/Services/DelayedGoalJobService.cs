using Fanex.NAP.SPU.FS.Features.DelayedGoal;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.JobService.Services
{
    public interface IDelayedGoalJobService
    {
        Task<bool> Execute(int fraudCaseId, int chunkSize);
    }

    public class DelayedGoalJobService : IDelayedGoalJobService
    {

        private readonly ISentryClient sentryClient;
        private readonly DelayedGoalServices delayedGoalServices;
        private readonly IFraudCaseServices fraudCaseService;
        private readonly IAppSettings appSettings;

        public DelayedGoalJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IFraudCaseServices fraudCaseService,
            DelayedGoalServices delayedGoalServices)
        {
            this.delayedGoalServices = delayedGoalServices;
            this.sentryClient = sentryClient;
            this.fraudCaseService = fraudCaseService;
            this.appSettings = appSettings;
        }

        public Task<bool> Execute(int fraudCaseId, int chunkSize)
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
            var results = delayedGoalServices
                 .CleanResult()
                 .Precondition()
                 .Rules(chunkSize);

            if (results != null && results.Any())
            {
                foreach (var result in results.Distinct())
                {
                    if (result.ErrorCode != 0)
                    {
                        sentryClient.CaptureMessage(result.ErrorMessage, Sentry.Protocol.SentryLevel.Error);
                    }
                }
            }
        }
    }
}

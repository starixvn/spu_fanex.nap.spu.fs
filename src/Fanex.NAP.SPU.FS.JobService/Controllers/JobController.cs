using Fanex.NAP.SPU.FS.JobService.Services;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.JobService.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IBigStateNearGoalJobService bigStateNearGoalJobService;
        private readonly ICheckBetJobService checkBetJobService;
        private readonly IDelayedGoalJobService delayedGoalJobService;
        private readonly IFlaggedAccountJobService flaggedAccountJobService;
        private readonly IDailyCaptureDataJobService dailyCaptureDataJobService;
        private readonly IScannedOfficeIPJobService scannedOfficeIpJobService;

        public JobController(ISentryClient sentryClient,
            IBigStateNearGoalJobService bigStateNearGoalJobService,
            ICheckBetJobService checkBetJobService,
            IDelayedGoalJobService delayedGoalJobService,
            IFlaggedAccountJobService flaggedAccountJobService,
            IDailyCaptureDataJobService dailyCaptureDataJobService,
            IScannedOfficeIPJobService scannedOfficeIpJobService)
        {
            this.sentryClient = sentryClient;
            this.bigStateNearGoalJobService = bigStateNearGoalJobService;
            this.checkBetJobService = checkBetJobService;
            this.delayedGoalJobService = delayedGoalJobService;
            this.flaggedAccountJobService = flaggedAccountJobService;
            this.dailyCaptureDataJobService = dailyCaptureDataJobService;
            this.scannedOfficeIpJobService = scannedOfficeIpJobService;
        }

        [HttpPost("bigstateneargoal")]
        public async Task<bool> BigStateNearGoal(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.bigStateNearGoalJobService.Execute(fraudCaseId);

        }

        [HttpPost("checkbet")]
        public async Task<bool> CheckBet(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.checkBetJobService.Execute(fraudCaseId);

        }

        [HttpPost("delayedgoal")]
        public async Task<bool> DelayedGoal(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.delayedGoalJobService.Execute(fraudCaseId, 100);

        }

        [HttpPost("flaggedaccount")]
        public async Task<bool> FlaggedAccount(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.flaggedAccountJobService.Execute(fraudCaseId, 100);

        }

        [HttpPost("dailycapturedata")]
        public async Task<bool> DailyCaptureData(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.dailyCaptureDataJobService.Execute(fraudCaseId);

        }

        [HttpPost("scannedofficeip")]
        public async Task<bool> ScannedOfficeIp(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.scannedOfficeIpJobService.Execute(fraudCaseId, 100);

        }
    }
}

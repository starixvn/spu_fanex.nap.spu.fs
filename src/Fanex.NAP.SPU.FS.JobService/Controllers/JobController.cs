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

        public JobController(ISentryClient sentryClient,
            IBigStateNearGoalJobService bigStateNearGoalJobService,
            ICheckBetJobService checkBetJobService)
        {
            this.sentryClient = sentryClient;
            this.bigStateNearGoalJobService = bigStateNearGoalJobService;
            this.checkBetJobService = checkBetJobService;


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
    }
}

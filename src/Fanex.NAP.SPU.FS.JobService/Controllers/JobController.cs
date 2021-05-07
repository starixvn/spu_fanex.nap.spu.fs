using Fanex.NAP.SPU.DCS.JobService.Services;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.DCS.JobService.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly ISentryClient sentryClient;
        private readonly IBigStateNearGoalJobService bigStateNearGoalJobService;       

        public JobController(ISentryClient sentryClient,
            IBigStateNearGoalJobService bigStateNearGoalJobService)
        {
            this.sentryClient = sentryClient;
            this.bigStateNearGoalJobService = bigStateNearGoalJobService;
            
        }

        [HttpPost("bigstateneargoal")]
        public async Task<bool> BigStateNearGoal(string jobName, int fraudCaseId)
        {
            sentryClient.CaptureMessage($"{jobName} is running at {DateTime.Now}");

            return await this.bigStateNearGoalJobService.Execute(fraudCaseId);

        }        
    }
}

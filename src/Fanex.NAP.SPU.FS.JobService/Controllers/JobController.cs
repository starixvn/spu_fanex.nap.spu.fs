namespace Fanex.NAP.SPU.FS.JobService.Controllers
{
    using Fanex.NAP.SPU.FS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly IBigStateNearGoalJobService bigStateNearGoalJobService;
        private readonly ICheckBetJobService checkBetJobService;
        private readonly IDelayedGoalJobService delayedGoalJobService;
        private readonly IFlaggedAccountJobService flaggedAccountJobService;
        private readonly IDailyCaptureDataJobService dailyCaptureDataJobService;
        private readonly IScannedOfficeIPJobService scannedOfficeIpJobService;
        private readonly int chunkSize = 100;

        public JobController(
            IBigStateNearGoalJobService bigStateNearGoalJobService,
            ICheckBetJobService checkBetJobService,
            IDelayedGoalJobService delayedGoalJobService,
            IFlaggedAccountJobService flaggedAccountJobService,
            IDailyCaptureDataJobService dailyCaptureDataJobService,
            IScannedOfficeIPJobService scannedOfficeIpJobService)
        {
            this.bigStateNearGoalJobService = bigStateNearGoalJobService;
            this.checkBetJobService = checkBetJobService;
            this.delayedGoalJobService = delayedGoalJobService;
            this.flaggedAccountJobService = flaggedAccountJobService;
            this.dailyCaptureDataJobService = dailyCaptureDataJobService;
            this.scannedOfficeIpJobService = scannedOfficeIpJobService;
        }

        [HttpPost("bigstateneargoal")]
        public async Task<bool> BigStateNearGoal(string jobName)
        {           
            return await this.bigStateNearGoalJobService.Execute();
        }

        [HttpPost("checkbet")]
        public async Task<bool> CheckBet(string jobName)
        {           
            return await this.checkBetJobService.Execute();
        }

        [HttpPost("delayedgoal")]
        public async Task<bool> DelayedGoal(string jobName)
        {           
            return await this.delayedGoalJobService.Execute(chunkSize);
        }

        [HttpPost("flaggedaccount")]
        public async Task<bool> FlaggedAccount(string jobName)
        {           
            return await this.flaggedAccountJobService.Execute(chunkSize);
        }

        [HttpPost("dailycapturedata")]
        public async Task<bool> DailyCaptureData(string jobName)
        {           
            return await this.dailyCaptureDataJobService.Execute();
        }

        [HttpPost("scannedofficeip")]
        public async Task<bool> ScannedOfficeIp(string jobName)
        {           
            return await this.scannedOfficeIpJobService.Execute(chunkSize);
        }
    }
}

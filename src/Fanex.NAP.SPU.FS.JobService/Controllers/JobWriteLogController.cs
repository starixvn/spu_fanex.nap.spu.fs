namespace Fanex.NAP.SPU.FS.JobService.Controllers
{
    using Fanex.NAP.SPU.FS.Features.WriteLog;
    using Fanex.NAP.SPU.FS.JobService.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/jobwritelog")]
    public class JobWriteLogController : ControllerBase
    {
        private IWriteLogJobService _writeLogJobService;

        public JobWriteLogController(IWriteLogJobService writeLogJobService)
        {
            _writeLogJobService = writeLogJobService;
        }

        [HttpPost("writelog")]
        public async Task<bool> WriteLog([FromBody] LogInsertModel logInsert)
        {            
            return await _writeLogJobService.Execute(logInsert);
        }
    }
}

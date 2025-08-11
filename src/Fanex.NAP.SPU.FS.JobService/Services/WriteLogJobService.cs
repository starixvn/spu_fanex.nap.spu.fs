namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.FS.Features.WriteLog;
    using System;
    using System.Threading.Tasks;

    public interface IWriteLogJobService
    {
        Task<bool> Execute(LogInsertModel logInsert);
    }

    public class WriteLogJobService : IWriteLogJobService
    {
        private readonly IWriteLogService _service;

        public WriteLogJobService(IWriteLogService service)
        {
            _service = service;
        }

        public Task<bool> Execute(LogInsertModel logInsert)
        {
            try
            {
                JobRun(logInsert);
            }
            catch (Exception ex)
            {
                LogService.Error($"FS Error in {nameof(WriteLogJobService)}: {ex.Message}", ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun(LogInsertModel logInsert)
        {
            _service.InsertLog(logInsert);
        }
    }
}

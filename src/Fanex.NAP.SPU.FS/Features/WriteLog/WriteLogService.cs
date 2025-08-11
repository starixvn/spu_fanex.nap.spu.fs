namespace Fanex.NAP.SPU.FS.Features.WriteLog
{
    using Fanex.NAP.SPU.FS.Shares;   

    public interface IWriteLogService
    {
        DataResult InsertLog(LogInsertModel logInsert);
    }

    public class WriteLogService : IWriteLogService
    {
        private readonly IWriteLogDataAccess _dataAccess;

        public WriteLogService()
        {
            _dataAccess = new WriteLogDataAccess();
        }

        public DataResult InsertLog(LogInsertModel logInsert)
        {
           return _dataAccess.InsertLog(logInsert);
        }
    }
}

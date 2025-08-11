namespace Fanex.NAP.SPU.FS.Features.WriteLog
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.FS.Shares;

    public interface IWriteLogDataAccess
    {
        DataResult InsertLog(LogInsertModel logInsert);
    }

    public class WriteLogDataAccess : IWriteLogDataAccess
    {
        private readonly IDynamicRepository _repository;

        public WriteLogDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public DataResult InsertLog(LogInsertModel logInsert)
        {
            var criteria = new WriteLogCriteria
            {
                Funtion = logInsert.Function,
                Duration = logInsert.Duration
            };

            _repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = 0
            };
        }
    }
}

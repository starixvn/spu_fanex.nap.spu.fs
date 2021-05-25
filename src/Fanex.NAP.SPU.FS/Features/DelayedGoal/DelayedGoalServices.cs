using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.DelayedGoal
{    
    public class DelayedGoalServices: BaseService
    {
        private readonly IDelayedGoalDataAccess delayedGoalDataAccess;

        private IEnumerable<MatchEntity> _matchList;

        public IEnumerable<MatchEntity> MatchList => _matchList;

        public DelayedGoalServices(IDelayedGoalDataAccess delayedGoalDataAccess)
        {
            this.delayedGoalDataAccess = delayedGoalDataAccess;
        }
        public override BaseService CleanResult()
        {
            if (_matchList == null || !_matchList.Any())
            {
                return this;
            }

            _matchList = Enumerable.Empty<MatchEntity>();

            return this;
        }

        public override BaseService Precondition()
        {
            _matchList = this.delayedGoalDataAccess.PreCondition<MatchEntity>();

            return base.Precondition();
        }

        public override IEnumerable<DataResult> Rules(int chunkSize)
        {
            var dataResults = new List<DataResult>();

            var matchChunks = _matchList.Chunk(chunkSize);

            foreach (var matches in matchChunks)
            {
                var dataResult = Execute(matches);

                if (dataResult.ErrorCode == -100)
                {
                    dataResults.Add(dataResult);
                }
            }

            return dataResults;
        }

        private DataResult Execute(IEnumerable<MatchEntity> matchList)
        {
            try
            {
                var matchIds = string.Join(",", matchList.Select(t => t.MatchId.ToString()));

                return delayedGoalDataAccess.Rule(matchIds);

            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                return new DataResult
                {
                    ErrorCode = -100,
                    ErrorMessage = logMessage
                };
            }
        }

    }
}

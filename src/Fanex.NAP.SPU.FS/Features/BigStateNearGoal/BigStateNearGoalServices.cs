using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fanex.NAP.SPU.FS.Features.BigStateNearGoal
{
    public class BigStateNearGoalServices : BaseService
    {
        private readonly IBigStateNearGoalDataAccess bigStateNearGoalDataAccess;

        private IEnumerable<MatchEntity> _matchList;

        private readonly int waitingTimeInSecond = 2;

        public IEnumerable<MatchEntity> MatchList => _matchList;

        public BigStateNearGoalServices(IBigStateNearGoalDataAccess bigStateNearGoalDataAccess)
        {
            this.bigStateNearGoalDataAccess = bigStateNearGoalDataAccess;
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
            _matchList = bigStateNearGoalDataAccess.PreCondition<MatchEntity>();

            return this;
        }

        public override IEnumerable<DataResult> Rules()
        {
            var dataResults = new List<DataResult>();

            if (_matchList != null && _matchList.Any())
            {
                foreach (var match in _matchList)
                {
                    var dataResult = Execute(match);

                    if(dataResult.ErrorCode == -100)
                    {
                        dataResults.Add(dataResult);
                    }
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(waitingTimeInSecond));

            return dataResults;
        }
       
        private DataResult Execute(MatchEntity match)
        {
            try
            {
                return bigStateNearGoalDataAccess.Rule(match.MatchId.ToString());

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

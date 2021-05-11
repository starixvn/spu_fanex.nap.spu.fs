using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Entities;
using Fanex.NAP.SPU.FS.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fanex.NAP.SPU.FS.Features.BigStateNearGoal
{
    public class BigStateNearGoalServices : BaseService
    {
        private readonly IBigStateNearGoalDataAccess bigStateNearGoalDataAccess;

        private IEnumerable<MatchEntity> _matches;

        private readonly int waitingTimeInSecond = 2;

        public IEnumerable<MatchEntity> Matches => _matches;

        public BigStateNearGoalServices(IBigStateNearGoalDataAccess bigStateNearGoalDataAccess)
        {
            this.bigStateNearGoalDataAccess = bigStateNearGoalDataAccess;
        }


        public override BaseService CleanResult()
        {
            if (_matches == null || !_matches.Any())
            {
                return this;
            }

            _matches = Enumerable.Empty<MatchEntity>();

            return this;
        }

        public override BaseService Precondition()
        {
            _matches = bigStateNearGoalDataAccess.PreCondition<MatchEntity>();

            return this;
        }

        public override DataResult Rule()
        {
            var dataResult = new DataResult
            {   
                ErrorCode = -1,
                ErrorMessage = "Matches is empty."
            };

            if (_matches != null && _matches.Any())
            {
                foreach (var match in _matches)
                {
                    dataResult = Execute(match);
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(waitingTimeInSecond));

            return dataResult;
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

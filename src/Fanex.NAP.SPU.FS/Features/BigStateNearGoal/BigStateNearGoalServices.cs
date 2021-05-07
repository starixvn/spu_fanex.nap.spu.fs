using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.CTS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Entities;
using Fanex.NAP.SPU.FS.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fanex.NAP.SPU.CTS.Features.Accounts
{
    public class BigStateNearGoalServices : IBaseService
    {
        private readonly IBigStateNearGoalDataAccess bigStateNearGoalDataAccess;
        
        private IEnumerable<MatchEntity> _matches;

        private readonly int waitingTimeInSecond = 2;

        public BigStateNearGoalServices(IBigStateNearGoalDataAccess bigStateNearGoalDataAccess)
        {
            this.bigStateNearGoalDataAccess = bigStateNearGoalDataAccess;
        }

        public IBaseService CleanResult()
        {
            if (_matches == null || !_matches.Any())
            {
                return this;
            }

            _matches = Enumerable.Empty<MatchEntity>();

            return this;
        }

        public IBaseService Precondition()
        {
            _matches = bigStateNearGoalDataAccess.PreCondition<MatchEntity>();

            return this;
        }

        public IBaseService Rule()
        {
            ExecuteBySeparatedThreads();

            return this;
        }

        private void ExecuteBySeparatedThreads()
        {
            if (_matches == null)
            {
                return;
            }
            foreach (var match in _matches)
            {
                Execute(match);
            }

            Thread.Sleep(TimeSpan.FromSeconds(waitingTimeInSecond));
        }

        private void Execute(MatchEntity match)
        {
            try
            {
                if(match != null && match.MatchId != 0)
                {
                    bigStateNearGoalDataAccess.Rule(match.MatchId.ToString());
                }
                
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }
        }
    }
}

using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.FS.Features.BigStateNearGoal
{
    public interface IBigStateNearGoalDataAccess
    {
        IEnumerable<Match> PreCondition<Match>();

        DataResult Rule(string matchIds);

    }

    public class BigStateNearGoalDataAccess : IBigStateNearGoalDataAccess
    {
        readonly IDynamicRepository repository;

        public BigStateNearGoalDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<Match> PreCondition<Match>()
        {
            var criteria = new BigStakeNearGoalPreconditionCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<Match>(criteria);
            }
        }

        public DataResult Rule(string matchIds)
        {            
            var criteria = new BigStakeNearGoalRuleCriteria
            {
                MatchIds = matchIds
            };

           var errorCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errorCode
            };
        }
    }
}

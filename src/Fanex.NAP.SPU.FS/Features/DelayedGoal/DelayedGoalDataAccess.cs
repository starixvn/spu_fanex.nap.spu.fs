using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.DelayedGoal.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.DelayedGoal
{
   public interface IDelayedGoalDataAccess
    {
        IEnumerable<T> PreCondition<T>();

        DataResult Rule(string matchIds);
    }

    public class DelayedGoalDataAccess : IDelayedGoalDataAccess
    {
        readonly IDynamicRepository repository;

        public DelayedGoalDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<T> PreCondition<T>()
        {
            var criteria = new DelayGoalPreconditionCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<T>(criteria);
            }
        }

        public DataResult Rule(string matchIds)
        {
            var criteria = new DelayGoalRuleCriteria
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

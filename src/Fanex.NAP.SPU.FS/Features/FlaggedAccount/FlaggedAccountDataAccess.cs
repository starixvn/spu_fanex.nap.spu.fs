using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.FS.Features.FlaggedAccount
{
    public interface IFlaggedAccountDataAccess
    {
        IEnumerable<T> PreCondition<T>();

        DataResult Rule(string matchIds, string flaggedAccounts);
    }

    public class FlaggedAccountDataAccess : IFlaggedAccountDataAccess
    {
        readonly IDynamicRepository repository;

        public FlaggedAccountDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<T> PreCondition<T>()
        {
            var criteria = new FlaggedAccountPreconditionCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<T>(criteria);
            }
        }

        public DataResult Rule(string matchIds, string flaggedAccounts)
        {            

            var criteria = new FlaggedAccountRuleCriteria
            {
                MatchIds = matchIds,
                FlaggedAccountsXML = flaggedAccounts
            };

            var result = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = result
            };
        }
    }
}

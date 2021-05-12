using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.ScannedOfficeIP
{
    public interface IScannedOfficeIPDataAccess
    {
        IEnumerable<T> PreCondition<T>();

        DataResult Rule(string matchIds);
    }

    public class ScannedOfficeIPDataAccess : IScannedOfficeIPDataAccess
    {
        readonly IDynamicRepository repository;

        public ScannedOfficeIPDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<T> PreCondition<T>()
        {
            var criteria = new OfficeIPPreconditionCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<T>(criteria);
            }
        }

        public DataResult Rule(string matchIds)
        {
            var criteria = new OfficeIPRuleCriteria
            {
                MatchIds = matchIds
            };

            var result = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = result
            };
        }
    }
}

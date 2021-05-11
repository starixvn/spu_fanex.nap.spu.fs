using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.CheckBet.Criteria;
using Fanex.NAP.SPU.FS.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.CheckBet
{
    public interface ICheckBetDataAccess
    {
        DataResult Rule();
    }
    public class CheckBetDataAccess : ICheckBetDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public CheckBetDataAccess()
        {
            dynamicRepository = new DynamicRepository();
        }

        public DataResult Rule()
        {
            var result = dynamicRepository.Execute(new CheckBetRuleCriteria());

            return new DataResult
            {
                ErrorCode = result
            };
        }
    }
}

using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.FlaggedAccount.Criteria
{
    public class FlaggedAccountPreconditionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "FS_FraudCase_FlaggedAccount_PreCondition";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.DelayedGoal.Criteria
{
    public class DelayGoalPreconditionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "FS_FraudCase_DelayGoal_PreCondition";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

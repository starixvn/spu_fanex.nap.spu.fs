using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Criteria
{
    public class BigStakeNearGoalPreconditionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "FS_FraudCase_BSNG_PreCondition";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

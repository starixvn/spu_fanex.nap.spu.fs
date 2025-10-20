namespace Fanex.NAP.SPU.FS.Features.DelayedGoal.Criteria
{
    using Fanex.Data.Repository;

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

using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.DelayedGoal.Criteria
{
    public class DelayGoalRuleCriteria : NonQueryCommand
    {
        public string MatchIds { get; set; }

        public override string GetSettingKey()
        {
            return "FS_FraudCase_DelayGoal_Rule";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

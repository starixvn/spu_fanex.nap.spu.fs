namespace Fanex.NAP.SPU.FS.Features.CheckBet.Criteria
{
    using Fanex.Data.Repository;

    public class CheckBetRuleCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "FS_FraudCase_CheckBet_Rule";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

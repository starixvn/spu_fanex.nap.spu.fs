namespace Fanex.NAP.SPU.FS.Features.FlaggedAccount.Criteria
{
    using Fanex.Data.Repository;

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

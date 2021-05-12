using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.FlaggedAccount.Criteria
{
    public class FlaggedAccountRuleCriteria: NonQueryCommand
    {
        public string MatchIds { get; set; }

        public string FlaggedAccountsXML { get; set; }

        public override string GetSettingKey()
        {
            return "FS_FraudCase_FlaggedAccount_Rule";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

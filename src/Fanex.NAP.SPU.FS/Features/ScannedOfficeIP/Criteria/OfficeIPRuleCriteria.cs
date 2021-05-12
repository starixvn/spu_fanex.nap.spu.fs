using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.ScannedOfficeIP.Criteria
{
    class OfficeIPRuleCriteria : NonQueryCommand
    {
        public string MatchIds { get; set; }

        public override string GetSettingKey() => "FS_FraudCase_OfficeIP_Rule";

        public override bool IsValid() => true;
    }
}

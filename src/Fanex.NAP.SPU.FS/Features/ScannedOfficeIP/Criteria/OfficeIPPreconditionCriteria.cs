using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.ScannedOfficeIP.Criteria
{
    public class OfficeIPPreconditionCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "FS_FraudCase_OfficeIP_PreCondition";

        public override bool IsValid() => true;
    }
}

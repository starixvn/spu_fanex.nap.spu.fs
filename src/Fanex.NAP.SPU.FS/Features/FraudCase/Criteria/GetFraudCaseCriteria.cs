using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.FraudCase.Criteria
{
    public class GetFraudCaseCriteria : CriteriaBase
    {
        public int FraudCaseId { get; set; }

        public int Environment { get; set; }

        public override string GetSettingKey()
        {
            return "FS_FraudCase_GetById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

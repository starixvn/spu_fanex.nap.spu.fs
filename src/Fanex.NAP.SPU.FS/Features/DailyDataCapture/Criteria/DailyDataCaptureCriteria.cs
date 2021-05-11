using Fanex.Data.Repository;

namespace Fanex.NAP.SPU.FS.Features.DailyDataCapture.Criteria
{
    public class DailyDataCaptureCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "FS_FraudCase_DailyDataMonitoring_Rule";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

namespace Fanex.NAP.SPU.FS.Features.WriteLog
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class WriteLogCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_Function")]
        public string Funtion { get; set; }

        [SpParam(SqlParamName = "ip_Duration")]
        public int Duration { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_WriteLog_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}

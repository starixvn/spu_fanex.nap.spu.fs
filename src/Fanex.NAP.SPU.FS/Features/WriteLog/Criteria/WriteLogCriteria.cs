namespace Fanex.NAP.SPU.FS.Features.WriteLog
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class WriteLogCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_UID")]
        public string UID { get; set; }

        [SpParam(SqlParamName = "ip_FunctionName")]
        public string FunctionName { get; set; }

        [SpParam(SqlParamName = "ip_ServiceName")]
        public string ServiceName { get; set; }

        [SpParam(SqlParamName = "ip_DurationFanexApi")]
        public long DurationFanexApi { get; set; }

        [SpParam(SqlParamName = "ip_IsSuccess")]
        public bool IsSuccess { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_LogFanexApiDuration";
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}

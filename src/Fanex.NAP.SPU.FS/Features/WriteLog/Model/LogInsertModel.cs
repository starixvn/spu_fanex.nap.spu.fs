namespace Fanex.NAP.SPU.FS.Features.WriteLog
{
    public class LogInsertModel
    {
        public string UID { get; set; }

        public string FunctionName { get; set; }

        public string ServiceName { get; set; }

        public long DurationFanexApi { get; set; }

        public bool IsSuccess { get; set; }
    }
}

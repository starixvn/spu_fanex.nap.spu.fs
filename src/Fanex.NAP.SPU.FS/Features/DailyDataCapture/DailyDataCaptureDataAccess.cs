using Fanex.Data.Repository;
using Fanex.NAP.SPU.FS.Features.DailyDataCapture.Criteria;
using Fanex.NAP.SPU.FS.Shares;

namespace Fanex.NAP.SPU.FS.Features.DailyDataCapture
{
    public interface IDailyDataCaptureDataAccess
    {
        DataResult Rule();
    }
    public class DailyDataCaptureDataAccess : IDailyDataCaptureDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public DailyDataCaptureDataAccess()
        {
            dynamicRepository = new DynamicRepository();
        }

        public DataResult Rule()
        {
            var result = dynamicRepository.Execute(new DailyDataCaptureCriteria());

            return new DataResult
            {
                ErrorCode = result
            };
        }
    }
}

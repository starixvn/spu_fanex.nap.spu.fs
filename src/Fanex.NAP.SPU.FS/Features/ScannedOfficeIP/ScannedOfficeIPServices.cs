using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.ScannedOfficeIP
{
    public class ScannedOfficeIPServices : BaseService
    {
        private readonly IScannedOfficeIPDataAccess scannedOfficeIPDataAccess;

        private IEnumerable<MatchEntity> _matchList;

        private readonly int waitingTimeInSecond = 2;

        public IEnumerable<MatchEntity> MatchList => _matchList;

        public ScannedOfficeIPServices(IScannedOfficeIPDataAccess scannedOfficeIPDataAccess)
        {
            this.scannedOfficeIPDataAccess = scannedOfficeIPDataAccess;
        }

        public override BaseService CleanResult()
        {
            if (_matchList == null || !_matchList.Any())
            {
                return this;
            }

            _matchList = Enumerable.Empty<MatchEntity>();

            return this;
        }

        public override BaseService Precondition()
        {
            _matchList = scannedOfficeIPDataAccess.PreCondition<MatchEntity>();

            return this;
        }

        public override IEnumerable<DataResult> Rules(int chunkSize)
        {
            var dataResults = new List<DataResult>();

            if (_matchList != null && _matchList.Any())
            {
                var matchChunks = _matchList.Chunk(chunkSize);

                foreach (var match in matchChunks)
                {                    
                    try
                    {
                        var matchIds = string.Join(",", match.Select(x => x.MatchId));

                        scannedOfficeIPDataAccess.Rule(matchIds);
                    }
                    catch (Exception ex)
                    {
                        dataResults.Add(new DataResult
                        {
                            ErrorCode = -100,
                            ErrorMessage = ex.Message
                        });
                    }
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(waitingTimeInSecond));

            return dataResults;
        }

    }
}

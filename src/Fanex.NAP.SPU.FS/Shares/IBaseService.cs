using Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Entities;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.FS.Shares
{
    public interface IBaseService
    {
        IBaseService Precondition();

        DataResult Rule();

        IBaseService CleanResult();

        IEnumerable<MatchEntity> Matches { get; }
    }
}

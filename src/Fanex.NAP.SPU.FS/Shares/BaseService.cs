using Fanex.NAP.SPU.FS.Features.BigStateNearGoal.Entities;
using System.Collections.Generic;

namespace Fanex.NAP.SPU.FS.Shares
{
    public class BaseService
    {
        public virtual BaseService Precondition()
        {
            return this;
        }

        public virtual DataResult Rule()
        {
            return new DataResult();
        }

        public virtual BaseService CleanResult()
        {
            return this;
        }        
    }
}

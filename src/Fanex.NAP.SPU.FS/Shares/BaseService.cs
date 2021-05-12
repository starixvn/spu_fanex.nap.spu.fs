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

        public virtual DataResult Rule(int chunkSize)
        {
            return new DataResult();
        }

        public virtual IEnumerable<DataResult> Rules()
        {
            return new List<DataResult>();
        }

        public virtual IEnumerable<DataResult> Rules(int chunkSize)
        {
            return new List<DataResult>();
        }

        public virtual BaseService CleanResult()
        {
            return this;
        }

        public virtual BaseService GetFlaggedCustomer(string apiUrl)
        {
            return this;
        }
    }
}

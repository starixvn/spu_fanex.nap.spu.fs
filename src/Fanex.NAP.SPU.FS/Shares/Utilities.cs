namespace Fanex.NAP.SPU.FS.Shares
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Utilities
    {
        public static IEnumerable<IEnumerable<TValue>> Chunk2<TValue>(
            this IEnumerable<TValue> values,
            Int32 chunkSize)
        {
            return values
                .Select((v, i) => new { v, groupIndex = i / chunkSize })
                .GroupBy(x => x.groupIndex)
                .Select(g => g.Select(x => x.v));
        }     
    }
}

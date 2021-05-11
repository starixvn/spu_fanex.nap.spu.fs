using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.FS.Features.CheckBet.Criteria
{
    public class CheckBetRuleCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "FS_FraudCase_CheckBet_Rule";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.FS.Features.FlaggedAccount.Entities;
using Fanex.NAP.SPU.FS.Shares;
using Fanex.NAP.SPU.FS.Shares.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.SPU.FS.Features.FlaggedAccount
{
    public class FlaggedAccountServices: BaseService
    {
        private readonly IFlaggedAccountDataAccess flaggedAccountDataAccess;
        private IEnumerable<MatchEntity> _matchList;
        private IEnumerable<FlaggedCustomerEntity> _flaggedCustomerList;

        public IEnumerable<MatchEntity> MatchList => _matchList;

        public FlaggedAccountServices(IFlaggedAccountDataAccess flaggedAccountDataAccess)
        {
            this.flaggedAccountDataAccess = flaggedAccountDataAccess;
        }

        public override BaseService GetFlaggedCustomer(string apiUrl)
        {
            
            var client = new RestClient(new Uri(apiUrl));
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{CustIdList: null,EvidenceCodeList: null}", ParameterType.RequestBody);

            var response = client.Execute(request);

            if (!string.IsNullOrWhiteSpace(response.Content))
            {
                _flaggedCustomerList = JsonConvert.DeserializeObject<List<FlaggedCustomerEntity>>(response.Content);
            }

            return this;
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
            _matchList = this.flaggedAccountDataAccess.PreCondition<MatchEntity>();

            return base.Precondition();
        }

        public override IEnumerable<DataResult> Rules(int chunkSize)
        {
            var dataResults = new List<DataResult>();

            var matchChunks = _matchList.Chunk(chunkSize);

            foreach (var matches in matchChunks)
            {
                var dataResult = Execute(matches);

                if (dataResult.ErrorCode != 0)
                {
                    dataResults.Add(dataResult);
                }
            }

            return dataResults;
        }


        private DataResult Execute(IEnumerable<MatchEntity> matchList)
        {
            try
            {
                if(_flaggedCustomerList != null && _flaggedCustomerList.Any())
                {
                    var matchIds = string.Join(",", matchList.Select(t => t.MatchId.ToString()));
                    var flaggedAccounts = XmlHelper.ToXmlString(_flaggedCustomerList);

                    return flaggedAccountDataAccess.Rule(matchIds, flaggedAccounts);
                }
                else
                {
                    return new DataResult
                    {
                        ErrorCode = -2,
                        ErrorMessage = "Flagged Customer is empty."
                    };
                }

            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                return new DataResult
                {
                    ErrorCode = -100,
                    ErrorMessage = logMessage
                };
            }
        }
    }
}

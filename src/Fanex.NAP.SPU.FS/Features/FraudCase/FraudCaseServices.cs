using Sentry;

namespace Fanex.NAP.SPU.FS.Features.FraudCase
{
    public interface IFraudCaseServices
    {
        bool CheckFraudCaseIsActive(int fraudCaseId, int environment);
    }

    public class FraudCaseServices : IFraudCaseServices
    {
        private readonly IFraudCaseDataAccess fraudCaseDataAccess;
        private readonly ISentryClient sentryClient;

        public FraudCaseServices(IFraudCaseDataAccess fraudCaseDataAccess, ISentryClient sentryClient)
        {
            this.fraudCaseDataAccess = fraudCaseDataAccess;
            this.sentryClient = sentryClient;
        }

        public bool CheckFraudCaseIsActive(int fraudCaseId, int environment)
        {
            var fraudCase = fraudCaseDataAccess.GetFraudCaseById(fraudCaseId, environment);

            if(fraudCase != null)
            {
                return fraudCase.IsActive;
            }

            return false;
        }
    }
}

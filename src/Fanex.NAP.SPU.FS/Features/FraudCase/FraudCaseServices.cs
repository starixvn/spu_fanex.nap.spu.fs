namespace Fanex.NAP.SPU.FS.Features.FraudCase
{
    public interface IFraudCaseServices
    {
        bool CheckFraudCaseIsActive(int fraudCaseId, int environment);
    }

    public class FraudCaseServices : IFraudCaseServices
    {
        private readonly IFraudCaseDataAccess fraudCaseDataAccess;
        
        public FraudCaseServices(IFraudCaseDataAccess fraudCaseDataAccess)
        {
            this.fraudCaseDataAccess = fraudCaseDataAccess;            
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

using Fanex.NAP.SPU.CTS.Features.Accounts;
using Fanex.NAP.SPU.CTS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Shares;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.NAP.SPU.DCS.JobService.Services
{
    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterBaseServices(services);

            RegisterBigStateNearGoalService(services);            
        }

        private static void RegisterBigStateNearGoalService(this IServiceCollection services)
        {
            services.AddSingleton<IBigStateNearGoalDataAccess, BigStateNearGoalDataAccess>();
            services.AddSingleton<IBaseService, BigStateNearGoalServices>();
            services.AddSingleton<IBigStateNearGoalJobService, BigStateNearGoalJobService>();
        }

        private static void RegisterBaseServices(this IServiceCollection services)
        {
            services.AddSingleton<IFraudCaseDataAccess, FraudCaseDataAccess>();
            services.AddSingleton<IFraudCaseServices, FraudCaseServices>();
        }
    }
}

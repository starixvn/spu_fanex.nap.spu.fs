using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
using Fanex.NAP.SPU.FS.Features.CheckBet;
using Fanex.NAP.SPU.FS.Features.FraudCase;
using Fanex.NAP.SPU.FS.Shares;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.NAP.SPU.FS.JobService.Services
{
    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterBaseServices(services);

            RegisterBigStateNearGoalService(services);

            RegisterCheckBetService(services);
        }

        private static void RegisterBigStateNearGoalService(this IServiceCollection services)
        {
            services.AddSingleton<IBigStateNearGoalDataAccess, BigStateNearGoalDataAccess>();
            services.AddSingleton<BigStateNearGoalServices>();
            services.AddSingleton<IBigStateNearGoalJobService, BigStateNearGoalJobService>();
        }

        private static void RegisterCheckBetService(this IServiceCollection services)
        {
            services.AddSingleton<ICheckBetDataAccess, CheckBetDataAccess>();
            services.AddSingleton<CheckBetServices>();
            services.AddSingleton<ICheckBetJobService, CheckBetJobService>();
        }

        private static void RegisterBaseServices(this IServiceCollection services)
        {
            services.AddSingleton<IFraudCaseDataAccess, FraudCaseDataAccess>();
            services.AddSingleton<IFraudCaseServices, FraudCaseServices>();
        }
    }
}

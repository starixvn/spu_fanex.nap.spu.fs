namespace Fanex.NAP.SPU.FS.JobService.Services
{
    using Fanex.NAP.SPU.FS.Features.BigStateNearGoal;
    using Fanex.NAP.SPU.FS.Features.CheckBet;
    using Fanex.NAP.SPU.FS.Features.DailyDataCapture;
    using Fanex.NAP.SPU.FS.Features.DelayedGoal;
    using Fanex.NAP.SPU.FS.Features.FlaggedAccount;
    using Fanex.NAP.SPU.FS.Features.FraudCase;
    using Fanex.NAP.SPU.FS.Features.ScannedOfficeIP;
    using Fanex.NAP.SPU.FS.Shares;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterBaseServices(services);
            RegisterBigStateNearGoalService(services);
            RegisterCheckBetService(services);
            RegisterDelayedGoalService(services);
            RegisterFlaggedAccountService(services);
            RegisterDailyCaptureService(services);
            RegisterScannedOfficeIpService(services);
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

        private static void RegisterDelayedGoalService(this IServiceCollection services)
        {
            services.AddSingleton<IDelayedGoalDataAccess, DelayedGoalDataAccess>();
            services.AddSingleton<DelayedGoalServices>();
            services.AddSingleton<IDelayedGoalJobService, DelayedGoalJobService>();
        }

        private static void RegisterFlaggedAccountService(this IServiceCollection services)
        {
            services.AddSingleton<IFlaggedAccountDataAccess, FlaggedAccountDataAccess>();
            services.AddSingleton<FlaggedAccountServices>();
            services.AddSingleton<IFlaggedAccountJobService, FlaggedAccountJobService>();
        }

        private static void RegisterDailyCaptureService(this IServiceCollection services)
        {
            services.AddSingleton<IDailyDataCaptureDataAccess, DailyDataCaptureDataAccess>();
            services.AddSingleton<DailyDataCaptureServices>();
            services.AddSingleton<IDailyCaptureDataJobService, DailyCaptureDataJobService>();
        }

        private static void RegisterScannedOfficeIpService(this IServiceCollection services)
        {
            services.AddSingleton<IScannedOfficeIPDataAccess, ScannedOfficeIPDataAccess>();
            services.AddSingleton<ScannedOfficeIPServices>();
            services.AddSingleton<IScannedOfficeIPJobService, ScannedOfficeIPJobService>();
        }

        private static void RegisterBaseServices(this IServiceCollection services)
        {
            services.AddSingleton<IFraudCaseDataAccess, FraudCaseDataAccess>();
            services.AddSingleton<IFraudCaseServices, FraudCaseServices>();
        }
    }
}

using System.Reflection;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameRecord.FaChai.Core;
using Tgs.GameRounds.Schedule.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRecordCore(
        this IServiceCollection services,
        Action<RedisOptions, IServiceProvider> redisDBconfigure,
        Action<GameRecordInfoOptions, IServiceProvider> gameRecordInfoOptions,
        Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
        Action<GameRoundsScheduleClientOptions, IServiceProvider> gameRoundsScheduleClientOptions,
        Action<GameInterfaceFaChaiClientOptions, IServiceProvider> gameInterfaceFaChaiClientOptions)
        => services
            .AddOptions<RedisOptions>()
            .Configure(redisDBconfigure)
            .Services
            .AddOptions<GameRecordInfoOptions>()
            .Configure(gameRecordInfoOptions)
            .Services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient)
            .AddMemoryCache()
            .AddSingleton<RedisConnectionManager>()
            .AddGameCommandClient(gameCommandClientOptions)
            .AddGameRoundsScheduleClient(gameRoundsScheduleClientOptions)
            .AddGameInterfaceFaChaiClient(gameInterfaceFaChaiClientOptions)
            .AddGameRoundsWorkClient()
            .AddGameRoundsWorkManualClient()
            .AddSingleton<IScheduleFormDispatcher, ScheduleFormDispatcher>()
            .AddSingleton<IScheduleFormLock>(sp => ActivatorUtilities.CreateInstance<ScheduleFormLock>(sp, sp.GetRequiredService<RedisConnectionManager>().Database))
            .AddTransient<IUtensil>(sp => ActivatorUtilities.CreateInstance<UtensilRepository>(sp, TimeSpan.FromHours(1)))
            .AddSingleton<IGameRecordUtensil, GameRecordUtensil>()
            .AddSingleton<ISystemClock, SystemClock>();
}

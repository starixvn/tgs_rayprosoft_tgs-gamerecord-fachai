using System.Reflection;
using StackExchange.Redis;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameRecord.FaChai.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecordCore(
		this IServiceCollection services,
		Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
		Action<GameInterfaceFaChaiClientOptions, IServiceProvider> gameInterfaceFaChaiClientOptions,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient)
		.AddGameCommandClient(gameCommandClientOptions)
		.AddGameInterfaceFaChaiClient(gameInterfaceFaChaiClientOptions)
		.AddSingleton(TimeProvider.System)
		.AddTransient<IHangfireUtensil, HangfireUtensil>()
		.AddTransient<IHangfireLock>(sp => ActivatorUtilities.CreateInstance<HangfireLock>(sp, redisDatabaseResolver(sp)))
		.AddSingleton<IUtensil, UtensilRepository>();
}

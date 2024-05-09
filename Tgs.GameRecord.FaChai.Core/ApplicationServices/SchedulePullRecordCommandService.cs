using Microsoft.Extensions.Logging;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

internal class SchedulePullRecordCommandService(
	ILogger<SchedulePullRecordCommandService> logger,
	IGameInterfaceFaChaiClient client,
	ICommandService<HandleRecordCommand> service) : ICommandService<SchedulePullRecordCommand>
{
	public async ValueTask ExecuteAsync(SchedulePullRecordCommand command, CancellationToken cancellationToken = default)
	{
		var gameRecords = await client
			.GameRecordAsync(
				apiUrl: command.ApiUrl,
				agentCode: command.AgentCode,
				agentKey: command.AgentKey,
				currency: command.Currency,
				startDate: command.StartDate,
				endDate: command.EndDate,
				cancellationToken: cancellationToken)
			.ToArrayAsync(
				cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameRecordInfo>();

		logger.LogDebug("LogOn:{logOn} Message: Settlement. GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

		if (!gameRecords.Any())
		{
			return;
		}

		foreach (var gameRecord in gameRecords)
		{
			logger.LogDebug("LogOn:{logOn} Message: Settlement. GameRecords:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

			try
			{
				await service
					.ExecuteAsync(
					command: new(gameRecord),
					cancellationToken: cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "LogOn:{logOn} Message: game record exception.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
			}
		}
	}
}

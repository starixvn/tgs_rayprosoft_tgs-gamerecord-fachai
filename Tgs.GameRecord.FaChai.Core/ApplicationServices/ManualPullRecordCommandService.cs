using Microsoft.Extensions.Logging;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

internal class ManualPullRecordCommandService(
	ILogger<ManualPullRecordCommandService> logger,
	IGameInterfaceFaChaiClient client,
	IUtensil utensil,
	ICommandService<HandleRecordCommand> service) : ICommandService<ManualPullRecordCommand>
{
	public async ValueTask ExecuteAsync(ManualPullRecordCommand command, CancellationToken cancellationToken = default)
	{
		if (utensil.TimeDifferenceWithin2Hours(command.EndDate))
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

			logger.LogDebug("LogOn:{logOn} Message: GameRecord Settlement. GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

			if (!gameRecords.Any())
			{
				return;
			}

			foreach (var gameRecord in gameRecords)
			{
				logger.LogDebug("LogOn:{logOn} Message: GameRecord Settlement. GameRecords:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

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
		else
		{
			var gameRecords = await client
				.GameRecordHistoryAsync(
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

			logger.LogDebug("LogOn:{logOn} Message: GameRecordHistory Settlement. GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

			if (!gameRecords.Any())
			{
				return;
			}

			foreach (var gameRecord in gameRecords)
			{
				logger.LogDebug("LogOn:{logOn} Message: GameRecordHistory Settlement. GameRecords:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

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
}

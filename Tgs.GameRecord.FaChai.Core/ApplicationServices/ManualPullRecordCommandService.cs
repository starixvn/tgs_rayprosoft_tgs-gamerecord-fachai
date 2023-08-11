using Microsoft.Extensions.Logging;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

internal class ManualPullRecordCommandService : ICommandService<ManualPullRecordCommand>
{
    private readonly IGameInterfaceFaChaiClient _client;
    private readonly ILogger<ManualPullRecordCommandService> _logger;
    private readonly ICommandService<HandleRecordCommand> _service;
    private readonly IUtensil _utensil;

    public ManualPullRecordCommandService(
        ILogger<ManualPullRecordCommandService> logger,
        IGameInterfaceFaChaiClient client,
        IUtensil utensil,
        ICommandService<HandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(ManualPullRecordCommand command, CancellationToken cancellationToken = default)
    {
        if (_utensil.TimeDifferenceWithin2Hours(command.EndDate))
        {
            var gameRecords = await _client
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

            _logger.LogDebug("LogOn:{logOn} Message: GameRecord Settlement. GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

            if (!gameRecords.Any())
            {
                return;
            }

            foreach (var gameRecord in gameRecords)
            {
                _logger.LogDebug("LogOn:{logOn} Message: GameRecord Settlement. GameRecords:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

                try
                {
                    await _service
                        .ExecuteAsync(
                        command: new(gameRecord),
                        cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LogOn:{logOn} Message: game record exception.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
                }
            }
        }
        else
        {
            var gameRecords = await _client
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

            _logger.LogDebug("LogOn:{logOn} Message: GameRecordHistory Settlement. GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

            if (!gameRecords.Any())
            {
                return;
            }

            foreach (var gameRecord in gameRecords)
            {
                _logger.LogDebug("LogOn:{logOn} Message: GameRecordHistory Settlement. GameRecords:{gameRecord}", DateTime.Now.ToString("o"), gameRecord);

                try
                {
                    await _service
                        .ExecuteAsync(
                        command: new(gameRecord),
                        cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LogOn:{logOn} Message: game record exception.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
                }
            }
        }
    }
}

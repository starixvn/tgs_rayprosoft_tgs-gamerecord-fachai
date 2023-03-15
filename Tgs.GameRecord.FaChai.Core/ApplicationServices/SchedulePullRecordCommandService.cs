using Microsoft.Extensions.Logging;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

internal class SchedulePullRecordCommandService : ICommandService<SchedulePullRecordCommand>
{
    private readonly IGameInterfaceFaChaiClient _client;
    private readonly ILogger<SchedulePullRecordCommandService> _logger;
    private readonly ICommandService<ScheduleHandleRecordCommand> _service;

    public SchedulePullRecordCommandService(
        ILogger<SchedulePullRecordCommandService> logger,
        IGameInterfaceFaChaiClient client,
        ICommandService<ScheduleHandleRecordCommand> service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async ValueTask ExecuteAsync(SchedulePullRecordCommand command, CancellationToken cancellationToken = default)
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

        _logger.LogInformation("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

        if (!gameRecords.Any())
        {
            return;
        }

        foreach (var gameRecord in gameRecords)
        {
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
                _logger.LogError(ex, "遊戲紀錄處理發生例外.({gameRecord})", gameRecord);
            }
        }
    }
}

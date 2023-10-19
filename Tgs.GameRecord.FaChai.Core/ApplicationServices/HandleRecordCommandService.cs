using Microsoft.Extensions.Options;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

internal class HandleRecordCommandService : ICommandService<HandleRecordCommand>
{
    private readonly IGameRecordHandleClient _client;
    private readonly GameRecordInfoOptions _gameRecordInfo;

    public HandleRecordCommandService(
        IOptions<GameRecordInfoOptions> options,
        IGameRecordHandleClient client)
    {
        _gameRecordInfo = options.Value;
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var externalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset();
        var externalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset();
        var bet = command.Record.Bet.ToDecimal().GetValueOrDefault(0);
        var settle = command.Record.Settle.ToDecimal().GetValueOrDefault(0);
        var commission = 0.0m;
        var winLoss = command.Record.WinLoss.ToDecimal().GetValueOrDefault(0);
        var wagerState = command.Record.WinLoss.ToDecimal().GetValueOrDefault(0) <= 0 ? WagerStates.Lose : WagerStates.Win;

        return _client
            .MessageAsync(
            gameProviderCode: _gameRecordInfo.GameProviderCode,
            playerCode: command.Record.PlayerCode,
            wagerState: wagerState,
            wagerType: WagerTypes.Normal,
            freeGameState: FreeGameStates.None,
            gambleState: GambleStates.None,
            cashoutState: CashoutStates.None,
            resettleState: ResettleStates.None,
            externalRoundId: command.Record.ExternalRoundId,
            externalWagerId: command.Record.ExternalRoundId,
            externalGameId: command.Record.ExternalGameId,
            externalCreatedOn: externalCreatedOn,
            externalClosedOn: externalClosedOn,
            externalPayoffOn: externalClosedOn,
            externalResettleOn: DateTimeOffset.UnixEpoch,
            bet: bet,
            validBet: bet,
            settle: settle,
            commission: commission,
            turnover: bet,
            winLoss: winLoss,
            additionalData: command.Record.AdditionalData,
            cancellationToken: cancellationToken);
    }
}

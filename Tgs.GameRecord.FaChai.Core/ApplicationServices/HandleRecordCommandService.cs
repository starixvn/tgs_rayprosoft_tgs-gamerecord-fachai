using Tgs.GameRounds.Work.Client;
using Tgs.GameRounds.Work.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

internal class HandleRecordCommandService : ICommandService<HandleRecordCommand>
{
    private readonly IGameRoundsWorkClient _client;
    private readonly IUtensil _utensil;

    public HandleRecordCommandService(
        IUtensil utensil,
        IGameRoundsWorkClient client)
    {
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _utensil
           .GetGameProviderIdAsync(
            cancellationToken: cancellationToken)
           .ConfigureAwait(false);

        var gameProviderCode = _utensil
            .GetGameProviderCode();

        var player = await _utensil
            .GetPlayerAsync(
            playerCode: command.Record.PlayerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameId = await _utensil
            .GetGameIdAsync(
            gameProviderId: gameProviderId,
            gameExternalId: command.Record.ExternalGameId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameProviderConfigurationId = await _utensil
            .GetBrandGameProviderConfigurationIdAsync(
            brandId: player.BrandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var externalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset();
        var externalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset();
        var bet = command.Record.Bet.ToDecimal().GetValueOrDefault(0);
        var validBet = command.Record.Bet.ToDecimal().GetValueOrDefault(0);
        var settle = command.Record.Settle.ToDecimal().GetValueOrDefault(0);
        var commission = 0.0m;
        var turnover = command.Record.Bet.ToDecimal().GetValueOrDefault(0);
        var winLoss = command.Record.WinLoss.ToDecimal().GetValueOrDefault(0);

        var status = RoundStatus.Closed;
        var clientStatus = command.Record.WinLoss <= 0 ? RoundClientStatus.Loss : RoundClientStatus.Win;

        await _client
            .RoundExchangeRateBiddingAsync(
                gameProviderId: gameProviderId,
                gameProviderCode: gameProviderCode,
                externalRoundId: command.Record.ExternalRoundId,
                gameId: gameId,
                externalGameId: command.Record.ExternalGameId,
                playerCode: player.PlayerCode,
                walletId: player.DefaultWalletId,
                currencyCode: player.DefaultWalletCurrencyCode,
                brandId: player.BrandId,
                gameProviderConfigurationId: gameProviderConfigurationId,
                status: status,
                clientStatus: clientStatus,
                externalCreatedOn: externalCreatedOn,
                externalClosedOn: externalClosedOn,
                bet: bet,
                validBet: validBet,
                settle: settle,
                commission: commission,
                turnover: turnover,
                playerWinLoss: winLoss,
                additionalData: command.Record.AdditionalData,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}

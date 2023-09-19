using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;

namespace Tgs.GameRecord.FaChai.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async void Loss()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            new GameRecordInfo
            {
                ExternalRoundId = "643583c37c18c573b57bbb2c",
                ExternalGameId = "22020",
                PlayerCode = "4uj06o4c7xue",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-04-11 11:58:59-04:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-04-11 11:58:59-04:00").ToTimestamp(),
                Bet = 3.6m,
                Settle = 0,
                WinLoss = -3.6m,
                Jppoints = 0,
                AdditionalData = "{\"bet\":3.6,\"prize\":0,\"winlose\":-3.6,\"before\":369.4,\"after\":365.8,\"jptax\":0,\"jppoints\":0,\"recordID\":\"643583c37c18c573b57bbb2c\",\"account\":\"4uj06o4c7xue\",\"gameID\":22020,\"gametype\":2,\"jpmode\":0,\"bdate\":\"2023-04-11 11:58:59\",\"isBuyFeature\":false}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Lose),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalWagerId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.WinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Win()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "FACHAI"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

        var command = new HandleRecordCommand(
            new GameRecordInfo
            {
                ExternalRoundId = "643583c37c18c573b57bbb2c",
                ExternalGameId = "22020",
                PlayerCode = "4uj06o4c7xue",
                ExternalCreatedOn = DateTimeOffset.Parse("2023-04-11 11:58:59-04:00").ToTimestamp(),
                ExternalClosedOn = DateTimeOffset.Parse("2023-04-11 11:58:59-04:00").ToTimestamp(),
                Bet = 100m,
                Settle = 200m,
                WinLoss = 100m,
                Jppoints = 0,
                AdditionalData = "{\"recordID\": \"643583c37c18c573b57bbb2c\"}"
            });

        var service = new HandleRecordCommandService(
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Win),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalWagerId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.WinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}

using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.GameRounds.Work.Client;
using Tgs.GameRounds.Work.Common.Protos;

namespace Tgs.GameRecord.FaChai.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async void Normal_Process_Settlement_Loss()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkClient>();

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

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "FACHAI";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "PB2",
            PlayerCode: command.Record.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.Record.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(command.Record.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var service = new HandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundExchangeRateBiddingAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(player.PlayerCode),
                walletId: Arg.Is(player.DefaultWalletId),
                currencyCode: Arg.Is(player.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId),
                status: Arg.Is(RoundStatus.Closed),
                clientStatus: Arg.Is(RoundClientStatus.Loss),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.WinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async void Normal_Process_Settlement_Win()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkClient>();

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

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "FACHAI";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "PB2",
            PlayerCode: command.Record.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.Record.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(command.Record.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var service = new HandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundExchangeRateBiddingAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(player.PlayerCode),
                walletId: Arg.Is(player.DefaultWalletId),
                currencyCode: Arg.Is(player.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId),
                status: Arg.Is(RoundStatus.Closed),
                clientStatus: Arg.Is(RoundClientStatus.Win),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.WinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}

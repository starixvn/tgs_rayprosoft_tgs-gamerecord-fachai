using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameInterface.FaChai.Protos;

namespace Tgs.GameRecord.FaChai.Core.UnitTest;

public class GameRecordUtensilUnitTest
{
    [Fact]
    public async Task Settlement_Brand_Game_Provider_Setting_Does_Not_Exist()
    {
        var fakeUtensil = Substitute.For<IUtensil>();

        var source = new GameRecordInfo
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
        };

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
            PlayerCode: source.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(source.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(source.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<Guid?>(null));

        var gameRecordUtensil = new GameRecordUtensil(fakeUtensil);

        var actual = await gameRecordUtensil
             .SettlementConvertRecordAsync(
                 source: source)
             .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Success);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameProviderCode, actual.GameProviderCode);
        Assert.Equal(source.ExternalRoundId, actual.ExternalRoundId);
        Assert.Equal(gameId, actual.GameId);
        Assert.Equal(source.ExternalGameId.ToString(), actual.ExternalGameId);
        Assert.Equal(source.PlayerCode, actual.PlayerCode);
        Assert.Equal(player.DefaultWalletId, actual.WalletId);
        Assert.Equal(player.DefaultWalletCurrencyCode, actual.CurrencyCode);
        Assert.Equal(player.BrandId, actual.BrandId);
        Assert.Equal(Guid.Empty, actual.GameProviderConfigurationId);
        Assert.True(actual.Closed);
        Assert.Equal(source.ExternalCreatedOn.ToDateTimeOffset(), actual.ExternalCreatedOn);
        Assert.Equal(source.ExternalClosedOn.ToDateTimeOffset(), actual.ExternalClosedOn);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Bet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.ValidBet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Turnover);
        Assert.Equal(source.WinLoss.ToDecimal().GetValueOrDefault(0), actual.WinLoss);
        Assert.Equal(source.Settle.ToDecimal().GetValueOrDefault(0), actual.Settle);
    }

    [Fact]
    public async Task Settlement_Game_Does_Not_Exist()
    {
        var fakeUtensil = Substitute.For<IUtensil>();

        var source = new GameRecordInfo
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
        };

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
            PlayerCode: source.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(source.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(source.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<int?>(null));

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var gameRecordUtensil = new GameRecordUtensil(fakeUtensil);

        var actual = await gameRecordUtensil
             .SettlementConvertRecordAsync(
                 source: source)
             .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Success);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameProviderCode, actual.GameProviderCode);
        Assert.Equal(source.ExternalRoundId, actual.ExternalRoundId);
        Assert.Equal(-1, actual.GameId);
        Assert.Equal(source.ExternalGameId.ToString(), actual.ExternalGameId);
        Assert.Equal(source.PlayerCode, actual.PlayerCode);
        Assert.Equal(player.DefaultWalletId, actual.WalletId);
        Assert.Equal(player.DefaultWalletCurrencyCode, actual.CurrencyCode);
        Assert.Equal(player.BrandId, actual.BrandId);
        Assert.Equal(gameProviderConfigurationId, actual.GameProviderConfigurationId);
        Assert.True(actual.Closed);
        Assert.Equal(source.ExternalCreatedOn.ToDateTimeOffset(), actual.ExternalCreatedOn);
        Assert.Equal(source.ExternalClosedOn.ToDateTimeOffset(), actual.ExternalClosedOn);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Bet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.ValidBet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Turnover);
        Assert.Equal(source.WinLoss.ToDecimal().GetValueOrDefault(0), actual.WinLoss);
        Assert.Equal(source.Settle.ToDecimal().GetValueOrDefault(0), actual.Settle);
    }

    [Fact]
    public async Task Settlement_Normal_Process()
    {
        var fakeUtensil = Substitute.For<IUtensil>();

        var source = new GameRecordInfo
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
        };

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
            PlayerCode: source.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(source.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(source.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var gameRecordUtensil = new GameRecordUtensil(fakeUtensil);

        var actual = await gameRecordUtensil
             .SettlementConvertRecordAsync(
                 source: source)
             .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Success);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameProviderCode, actual.GameProviderCode);
        Assert.Equal(source.ExternalRoundId, actual.ExternalRoundId);
        Assert.Equal(gameId, actual.GameId);
        Assert.Equal(source.ExternalGameId.ToString(), actual.ExternalGameId);
        Assert.Equal(source.PlayerCode, actual.PlayerCode);
        Assert.Equal(player.DefaultWalletId, actual.WalletId);
        Assert.Equal(player.DefaultWalletCurrencyCode, actual.CurrencyCode);
        Assert.Equal(player.BrandId, actual.BrandId);
        Assert.Equal(gameProviderConfigurationId, actual.GameProviderConfigurationId);
        Assert.True(actual.Closed);
        Assert.Equal(source.ExternalCreatedOn.ToDateTimeOffset(), actual.ExternalCreatedOn);
        Assert.Equal(source.ExternalClosedOn.ToDateTimeOffset(), actual.ExternalClosedOn);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Bet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.ValidBet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Turnover);
        Assert.Equal(source.WinLoss.ToDecimal().GetValueOrDefault(0), actual.WinLoss);
        Assert.Equal(source.Settle.ToDecimal().GetValueOrDefault(0), actual.Settle);
    }

    [Fact]
    public async Task Settlement_Player_Does_Not_Exist()
    {
        var fakeUtensil = Substitute.For<IUtensil>();

        var source = new GameRecordInfo
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
        };

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "FACHAI";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(source.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<PlayerInfo?>(null));

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(source.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameRecordUtensil = new GameRecordUtensil(fakeUtensil);

        var actual = await gameRecordUtensil
             .SettlementConvertRecordAsync(
                 source: source)
             .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Success);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameProviderCode, actual.GameProviderCode);
        Assert.Equal(source.ExternalRoundId, actual.ExternalRoundId);
        Assert.Equal(gameId, actual.GameId);
        Assert.Equal(source.ExternalGameId.ToString(), actual.ExternalGameId);
        Assert.Equal(string.Empty, actual.PlayerCode);
        Assert.Equal(Guid.Empty, actual.WalletId);
        Assert.Equal(string.Empty, actual.CurrencyCode);
        Assert.Equal(Guid.Empty, actual.BrandId);
        Assert.Equal(Guid.Empty, actual.GameProviderConfigurationId);
        Assert.True(actual.Closed);
        Assert.Equal(source.ExternalCreatedOn.ToDateTimeOffset(), actual.ExternalCreatedOn);
        Assert.Equal(source.ExternalClosedOn.ToDateTimeOffset(), actual.ExternalClosedOn);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Bet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.ValidBet);
        Assert.Equal(source.Bet.ToDecimal().GetValueOrDefault(0), actual.Turnover);
        Assert.Equal(source.WinLoss.ToDecimal().GetValueOrDefault(0), actual.WinLoss);
        Assert.Equal(source.Settle.ToDecimal().GetValueOrDefault(0), actual.Settle);
    }
}

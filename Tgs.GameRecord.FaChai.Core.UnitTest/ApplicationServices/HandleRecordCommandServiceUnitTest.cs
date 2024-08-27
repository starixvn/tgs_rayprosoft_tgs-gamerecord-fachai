using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.FaChai.Protos;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.GameRecord.Handle.Client;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.FaChai.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "FACHAI",
			GameProviderConfiguration = "FACHAIConfiguration",
			HangfirePrefix = "gamerecord-fachai-hangfire:",
			HangfireLocking = "gamerecord-fachai-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 15,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalRoundId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 3.6m,
				ValidBet = 3.6m,
				Settle = 0.0M,
				Commission = 0.0M,
				Turnover = 3.6m,
				WinLoss = -3.6m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "FACHAI",
			GameProviderConfiguration = "FACHAIConfiguration",
			HangfirePrefix = "gamerecord-fachai-hangfire:",
			HangfireLocking = "gamerecord-fachai-locking:",
			IntervalMinutes = 3,
			IntervalMinutesMax = 15,
			IntervalMinutesMinimum = 1
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

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

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalRoundId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.Record.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 100m,
				ValidBet = 100m,
				Settle = 200m,
				Commission = 0.0M,
				Turnover = 100m,
				WinLoss = 100m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}

using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Tgs.GameInterface.FaChai.Client;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.FaChai.Core.UnitTest.ApplicationServices;

public class ManualPullRecordCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process_TimeDifferenceWithin2Hours_False()
    {
        var fakeLogger = NullLogger<ManualPullRecordCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceFaChaiClient>();
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

        var command = new ManualPullRecordCommand(
            ApiUrl: new("http://localhost/"),
            AgentCode: "agentCode",
            AgentKey: "agentKey",
            Currency: "CNY",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        _ = fakeUtensil
            .TimeDifferenceWithin2Hours(
                source: Arg.Is(command.EndDate))
            .Returns(false);

        var service = new ManualPullRecordCommandService(
            fakeLogger,
            fakeClient,
            fakeUtensil,
            fakeService);

        await service.ExecuteAsync(command: command);

        _ = await fakeClient
            .Received(0)
            .GameRecordAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                agentCode: Arg.Is(command.AgentCode),
                agentKey: Arg.Is(command.AgentKey),
                currency: Arg.Is(command.Currency),
                startDate: Arg.Is(command.StartDate),
                endDate: Arg.Is(command.EndDate),
                cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync(
                cancellationToken: Arg.Any<CancellationToken>());

        _ = await fakeClient
            .Received(1)
            .GameRecordHistoryAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                agentCode: Arg.Is(command.AgentCode),
                agentKey: Arg.Is(command.AgentKey),
                currency: Arg.Is(command.Currency),
                startDate: Arg.Is(command.StartDate),
                endDate: Arg.Is(command.EndDate),
                cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync(
                cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Normal_Process_TimeDifferenceWithin2Hours_True()
    {
        var fakeLogger = NullLogger<ManualPullRecordCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceFaChaiClient>();
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

        var command = new ManualPullRecordCommand(
            ApiUrl: new("http://localhost/"),
            AgentCode: "agentCode",
            AgentKey: "agentKey",
            Currency: "CNY",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        _ = fakeUtensil
            .TimeDifferenceWithin2Hours(
                source: Arg.Is(command.EndDate))
            .Returns(true);

        var service = new ManualPullRecordCommandService(
            fakeLogger,
            fakeClient,
            fakeUtensil,
            fakeService);

        await service.ExecuteAsync(command: command);

        _ = await fakeClient
            .Received(1)
            .GameRecordAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                agentCode: Arg.Is(command.AgentCode),
                agentKey: Arg.Is(command.AgentKey),
                currency: Arg.Is(command.Currency),
                startDate: Arg.Is(command.StartDate),
                endDate: Arg.Is(command.EndDate),
                cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync(
                cancellationToken: Arg.Any<CancellationToken>());

        _ = await fakeClient
            .Received(0)
            .GameRecordHistoryAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                agentCode: Arg.Is(command.AgentCode),
                agentKey: Arg.Is(command.AgentKey),
                currency: Arg.Is(command.Currency),
                startDate: Arg.Is(command.StartDate),
                endDate: Arg.Is(command.EndDate),
                cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync(
                cancellationToken: Arg.Any<CancellationToken>());
    }
}

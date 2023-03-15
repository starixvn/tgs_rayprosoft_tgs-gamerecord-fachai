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
    public async Task Normal_Process()
    {
        var fakeLogger = NullLogger<ManualPullRecordCommandService>.Instance;
        var fakeClient = Substitute.For<IGameInterfaceFaChaiClient>();
        var fakeService = Substitute.For<ICommandService<ManualHandleRecordCommand>>();

        var command = new ManualPullRecordCommand(
            ApiUrl: new("http://localhost/"),
            AgentCode: "agentCode",
            AgentKey: "agentKey",
            Currency: "CNY",
            StartDate: DateTimeOffset.UtcNow.AddMinutes(-10),
            EndDate: DateTimeOffset.UtcNow);

        var service = new ManualPullRecordCommandService(
            fakeLogger,
            fakeClient,
            fakeService);

        await service
            .ExecuteAsync(
                command: command)
            .ConfigureAwait(false);

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
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}

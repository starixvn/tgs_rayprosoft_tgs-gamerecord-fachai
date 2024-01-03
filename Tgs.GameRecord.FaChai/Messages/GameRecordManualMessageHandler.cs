using System.Text;
using System.Text.Json;
using Tgs.GameRecord.FaChai.Common;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;
using Tgs.GameRecord.FaChai.Messages;

namespace Tgs.GameRecord.FaChai.Messages;

internal class GameRecordManualMessageHandler(
   ILogger<GameRecordManualMessageHandler> logger,
   ICommandService<ManualPullRecordCommand> service) : IMessageHandler
{
    public ValueTask HandleAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        var json = Encoding.UTF8.GetString(data.Span);
        var message = JsonSerializer.Deserialize<FaChaiGameRecordMessage>(json)
            ?? throw new NullReferenceException("The request data resolves to a null value");

        logger.LogInformation("LogOn:{logOn} Information Request:{message}", DateTime.Now.ToString("o"), message);

        var command = new ManualPullRecordCommand(
                ApiUrl: message.ApiUrl,
                AgentCode: message.AgentCode,
                AgentKey: message.AgentKey,
                Currency: message.Currency,
                StartDate: message.StartTime,
                EndDate: message.EndTime);

        return service.ExecuteAsync(
            command: command,
            cancellationToken: cancellationToken);
    }
}

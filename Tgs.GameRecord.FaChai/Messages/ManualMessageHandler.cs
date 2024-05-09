using System.Text;
using System.Text.Json;
using Tgs.GameRecord.FaChai.Common;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;
using Tgs.GameRecord.FaChai.Messages;

namespace Tgs.GameRecord.FaChai.Messages;

internal class ManualMessageHandler(
   ILogger<ManualMessageHandler> logger,
   ICommandService<ManualPullRecordCommand> service) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<FaChaiGameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Information Request:{message}",
			DateTime.Now.ToString("o"),
			subject,
			message);

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

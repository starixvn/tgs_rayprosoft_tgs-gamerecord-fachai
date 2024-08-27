using System.Text.Json;
using Tgs.GameRecord.FaChai.Common;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.FaChai.Messages;

internal class ScheduleMessageHandler(
   ILogger<ScheduleMessageHandler> logger,
   ICommandService<SchedulePullRecordCommand> service) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var message = JsonSerializer.Deserialize<FaChaiGameRecordMessage>(data.Span)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Information Request:{message}",
			DateTime.Now.ToString("o"),
			subject,
			message);

		var command = new SchedulePullRecordCommand(
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

using System.Text.Json;
using Tgs.GameRecord.FaChai.Common;
using Tgs.GameRecord.FaChai.Core;
using Tgs.GameRecord.FaChai.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.FaChai.Messages;

internal class ScheduleMessageHandler(
	ILogger<ManualMessageHandler> logger,
	ISchedulePullRecordDispatcher dispatcher) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var message = JsonSerializer.Deserialize<FaChaiGameRecordMessage>(data.Span)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = subject,
				Data = new
				{
					message.ApiUrl,
					message.AgentCode,
					message.AgentKey,
					message.Currency,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u")
				},
				Message = "This message receiving handler."
			}));

		return dispatcher.PushCommandAsync(
			command: new SchedulePullRecordCommand(
				ApiUrl: message.ApiUrl,
				AgentCode: message.AgentCode,
				AgentKey: message.AgentKey,
				Currency: message.Currency,
				StartDate: message.StartTime,
				EndDate: message.EndTime),
			cancellationToken: cancellationToken);
	}
}

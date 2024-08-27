using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.FaChai.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.FaChai.Controllers;

[Route("FaChai")]
[ApiController]
public class FaChaiController : ControllerBase
{
	/// <summary>
	/// Manuals the game record asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("manual")]
	public ValueTask ManualGameRecordAsync(
		[FromServices] IMessageSender messageSender,
		FaChaiGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: FaChaiNatsConst.ManualPublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Schedules the game record asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("schedule")]
	public ValueTask ScheduleGameRecordAsync(
		[FromServices] IMessageSender messageSender,
		FaChaiGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: FaChaiNatsConst.SchedulePublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);
}

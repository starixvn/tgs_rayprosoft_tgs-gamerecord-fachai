using System.Text;
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
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl":"http://api.fcg666.net/",
    ///             "agentCode":"GPICN",
    ///             "agentKey":"EOJC9q31Y7AnPb9i",
    ///             "currency":"CNY",
    ///             "startTime": "2023-03-13T15:45:00+08:00",
    ///             "endTime": "2023-03-13T16:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("manual")]
    public ValueTask ManualGameRecordAsync(
        [FromServices] IMessageSender messageSender,
        FaChaiGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: FaChaiNatsConst.ManualPublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Schedules the game record asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl":"http://api.fcg666.net/",
    ///             "agentCode":"GPICN",
    ///             "agentKey":"EOJC9q31Y7AnPb9i",
    ///             "currency":"CNY",
    ///             "startTime": "2023-03-13T15:45:00+08:00",
    ///             "endTime": "2023-03-13T16:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("schedule")]
    public ValueTask ScheduleGameRecordAsync(
        [FromServices] IMessageSender messageSender,
        FaChaiGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: FaChaiNatsConst.SchedulePublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);
}

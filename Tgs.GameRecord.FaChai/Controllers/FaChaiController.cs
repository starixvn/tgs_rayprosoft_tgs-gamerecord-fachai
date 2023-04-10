using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.GameRecord.FaChai.Core;
using Tgs.GameRecord.FaChai.ViewModels;
using Tgs.GameRounds.Schedule.Client;
using Tgs.GameRounds.Schedule.Protos;

namespace Tgs.GameRecord.FaChai.Controllers;

[Route("FaChai")]
[ApiController]
public class FaChaiController : ControllerBase
{
    private readonly GameRecordInfoOptions _gameRecordInfo;

    public FaChaiController(
        IOptions<GameRecordInfoOptions> options)
    {
        _gameRecordInfo = options.Value; ;
    }

    /// <summary>
    /// Manuals the game record asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
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
        [FromServices] IGameRoundsScheduleClient client,
        FaChaiViewModel source)
        => client.FaChai_ScheduleFormQueuedAsync(
             gameProviderCode: _gameRecordInfo.GameProviderCode,
             name: source.AgentCode,
             subject: "gp.gamerecord.scheduleform.fachai.queued",
             source: ScheduleFormSource.Manual,
             apiUrl: source.ApiUrl,
             agentCode: source.AgentCode,
             agentKey: source.AgentKey,
             currency: source.Currency,
             startTime: source.StartTime,
             endTime: source.EndTime,
             cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Schedules the game record asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
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
        [FromServices] IGameRoundsScheduleClient client,
        FaChaiViewModel source)
        => client.FaChai_ScheduleFormQueuedAsync(
             gameProviderCode: _gameRecordInfo.GameProviderCode,
             name: source.AgentCode,
             subject: "gp.gamerecord.scheduleform.fachai.queued",
             source: ScheduleFormSource.Schedule,
             apiUrl: source.ApiUrl,
             agentCode: source.AgentCode,
             agentKey: source.AgentKey,
             currency: source.Currency,
             startTime: source.StartTime,
             endTime: source.EndTime,
             cancellationToken: HttpContext.RequestAborted);
}

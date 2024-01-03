using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.FaChai.Common;

public record FaChaiGameRecordMessage
{
    [Required]
    [DataType(DataType.Url)]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string AgentCode { get; init; } = default!;
    [Required]
    public string AgentKey { get; init; } = default!;
    [Required]
    public string Currency { get; init; } = default!;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartTime { get; init; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset EndTime { get; init; }
}

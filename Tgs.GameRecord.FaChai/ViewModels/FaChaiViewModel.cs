using System.ComponentModel.DataAnnotations;
using Tgs.GameRecord.FaChai.Attributes;

namespace Tgs.GameRecord.FaChai.ViewModels;

public record FaChaiViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string AgentCode { get; init; } = default!;
    [Required]
    public string AgentKey { get; init; } = default!;
    [Required]
    public string Currency { get; init; } = default!;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartTime { get; init; }
    [Required]
    [DataType(DataType.DateTime)]
    [DateTimeRangeMinutes("StartTime", 15)]
    public DateTime EndTime { get; init; }
}

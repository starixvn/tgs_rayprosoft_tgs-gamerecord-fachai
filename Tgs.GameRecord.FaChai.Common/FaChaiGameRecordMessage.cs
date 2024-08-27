using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.FaChai.Common;

public record FaChaiGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "http://api.fcg666.net/")]
	public Uri ApiUrl { get; init; } = default!;

	[Required]
	[DefaultValue(typeof(string), "GPICN")]
	public string AgentCode { get; init; } = default!;

	[Required]
	[DefaultValue(typeof(string), "EOJC9q31Y7AnPb9i")]
	public string AgentKey { get; init; } = default!;

	[Required]
	[DefaultValue(typeof(string), "CNY")]
	public string Currency { get; init; } = default!;

	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-03-13T15:45:00+08:00")]
	public DateTimeOffset StartTime { get; init; }

	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-03-13T16:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; }
}

namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

public record ManualPullRecordCommand(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);

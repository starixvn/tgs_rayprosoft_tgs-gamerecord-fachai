namespace Tgs.GameRecord.FaChai.Core.ApplicationServices;

public record SchedulePullRecordCommand(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string Currency,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);

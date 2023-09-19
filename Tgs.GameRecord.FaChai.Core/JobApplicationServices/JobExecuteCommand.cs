namespace Tgs.GameRecord.FaChai.Core.JobApplicationServices;

public record JobExecuteCommand(
    string Name,
    Uri ApiUrl,
    string GameProviderCode,
    string AgentCode,
    string AgentKey,
    string Currency);

using Tgs.GameRecord.FaChai.Core.JobApplicationServices;

namespace Tgs.GameRecord.FaChai.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}

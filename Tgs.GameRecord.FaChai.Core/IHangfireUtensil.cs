using Tgs.GameRecord.FaChai.Core.JobApplicationServices;

namespace Tgs.GameRecord.FaChai.Core;

public interface IHangfireUtensil
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}

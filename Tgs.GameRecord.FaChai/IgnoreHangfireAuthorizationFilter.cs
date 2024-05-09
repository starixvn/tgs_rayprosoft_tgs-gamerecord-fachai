using Hangfire.Dashboard;

namespace Tgs.GameRecord.FaChai;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => true;
}

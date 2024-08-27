using Tgs.GameRecord.FaChai.Core.ApplicationServices;

namespace Tgs.GameRecord.FaChai.Core;

public interface ISchedulePullRecordDispatcher
{
	ValueTask PushCommandAsync(SchedulePullRecordCommand command, CancellationToken cancellationToken = default);
}

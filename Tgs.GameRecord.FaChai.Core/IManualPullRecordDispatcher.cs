using Tgs.GameRecord.FaChai.Core.ApplicationServices;

namespace Tgs.GameRecord.FaChai.Core;

public interface IManualPullRecordDispatcher
{
	ValueTask PushCommandAsync(ManualPullRecordCommand command, CancellationToken cancellationToken = default);
}

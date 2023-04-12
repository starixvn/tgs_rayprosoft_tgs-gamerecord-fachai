using Tgs.GameInterface.FaChai.Protos;
using Tgs.GameRecord.FaChai.Core.Models;

namespace Tgs.GameRecord.FaChai.Core;

public interface IGameRecordUtensil
{
    public ValueTask<RecordInfo> SettlementConvertRecordAsync(
        GameRecordInfo source,
        CancellationToken cancellationToken = default);
}

namespace Tgs.GameRecord.FaChai.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}

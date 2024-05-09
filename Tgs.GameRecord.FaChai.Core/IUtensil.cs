namespace Tgs.GameRecord.FaChai.Core;

public interface IUtensil
{
	bool TimeDifferenceWithin2Hours(DateTimeOffset source);
}

namespace Tgs.GameRecord.FaChai.Core;

internal class UtensilRepository(TimeProvider timeProvider) : IUtensil
{
	public bool TimeDifferenceWithin2Hours(DateTimeOffset source)
	{
		var hours = timeProvider.GetUtcNow().Subtract(source).TotalHours;

		return hours is > 0 and <= 2;
	}
}

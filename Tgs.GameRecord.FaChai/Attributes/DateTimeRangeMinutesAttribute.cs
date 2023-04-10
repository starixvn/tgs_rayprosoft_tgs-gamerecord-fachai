#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.FaChai.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DateTimeRangeMinutesAttribute : ValidationAttribute
{
    private int _maxLimitMinutes { get; set; }

    private string _startTime { get; set; }

    public DateTimeRangeMinutesAttribute(string startTime, int maxLimitMinutes)
    {
        _startTime = startTime;
        _maxLimitMinutes = maxLimitMinutes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime endTime = (DateTime)value;

        DateTime startTime = (DateTime)validationContext.ObjectType.GetProperty(_startTime).GetValue(validationContext.ObjectInstance, null);

        TimeSpan diffTimeSpan = endTime.Subtract(startTime);

        return startTime < endTime && diffTimeSpan.TotalMinutes <= _maxLimitMinutes ?
            ValidationResult.Success :
            new ValidationResult($"The start time cannot be greater than the end time, and the time range is limited({_maxLimitMinutes}m).");
    }
}

using Tgs.GameRecord.FaChai.Core;

namespace Tgs.GameRecord.FaChai;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}

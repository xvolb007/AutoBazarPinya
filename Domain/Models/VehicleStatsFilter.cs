using System.Text.Json.Serialization;

namespace Domain.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Operation
    {
        eq,
        gt,
        gte,
        lt,
        lte,
        like
    }

    public sealed class QueryFilter
    {
        public required string Column { get; init; }
        public required Operation Operation { get; init; }
        public required string[] Values { get; init; }
    }
}

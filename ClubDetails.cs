using Azure;
using Azure.Data.Tables;

namespace AzureTableDemo
{
    public record ClubDetails : ITableEntity
    {
        public string PartitionKey { get; set; } = default!;
        public string RowKey { get; set; } = default!;

        public string Name { get; set; } = default!;
        public DateTimeOffset? Timestamp { get ; set; }
        public ETag ETag { get; set ; } = default!;
    }
}

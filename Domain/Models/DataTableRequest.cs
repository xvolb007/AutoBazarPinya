﻿
namespace Domain.Models
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public SearchInfo? Search { get; set; }
        public List<OrderInfo>? Order { get; set; }
        public List<ColumnInfo>? Columns { get; set; }
    }

    public class SearchInfo
    {
        public string? Value { get; set; }
    }

    public class OrderInfo
    {
        public int Column { get; set; }
        public string Dir { get; set; } = "asc";
    }

    public class ColumnInfo
    {
        public string? Data { get; set; }
    }
}

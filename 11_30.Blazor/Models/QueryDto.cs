using System.ComponentModel.DataAnnotations;

public record QueryType
{
    [MinLength(3)]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;
    public string RequestContent { get; set; }
    public string Url { get; set; }
    public Guid Id { get; set; }
    public bool Selected { get; set; }
}

public record QueryAction
{
    public string Name { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public bool Selected { get; set; }
    public string Description { get; set; }
    public Guid GeneralQueryTypeId { get; set; }
}
public record QueryField
{
    public string Name { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public bool Selected { get; set; }
    public string Description { get; set; }
    public Guid GeneralQueryActionId { get; set; }
}

public record EepQueryDto(string? Table, string? Filter, string? Columns, string? Orderby);

// 定义数据项类型
public class DataGridItem
{
    public Dictionary<string, object> Values { get; set; } = new();

    public object this[string columnName]
    {
        get => Values.TryGetValue(columnName, out var value) ? value : null;
        set => Values[columnName] = value;
    }
}
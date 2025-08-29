using System.ComponentModel.DataAnnotations;
public record EJiaNews
{
    public string UserName { get; set; } = string.Empty;
    public string PassWord { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string NewsType { get; set; } = string.Empty;
}


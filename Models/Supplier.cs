namespace DemoMinimalAPI.Models;

public class Supplier
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Document { get; set; }
    public required bool Active { get; set; }
}

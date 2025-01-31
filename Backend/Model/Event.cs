namespace backend.Model;

public record Event{
    public int Id {get; set;}
    public string? Type {get; set;}
    public string? Description {get; set;}
    public string? Location {get; set;}
    public DateTime Date {get; set;}
}
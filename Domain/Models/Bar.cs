namespace Domain.Models;

public abstract class Bar<T>
{
    public double Open { get; set; }
    public double Close { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public DateTime Date { get; set; }
    public T Previous { get; set; }
}
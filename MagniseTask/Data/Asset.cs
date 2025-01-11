namespace MagniseTask.Data;

public class Asset
{
	public int Id { get; set; }
	public string Symbol { get; set; }
	public decimal Price { get; set; }
	public DateTime LastUpdated { get; set; }
}
	
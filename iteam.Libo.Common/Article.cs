namespace iteam.Libo.Common;
public partial class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Author { get; set; }
    public string? Isbn { get; set; }
    public string? Url { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    // public string Publisher { get; set; }

    public virtual ICollection<Item> Items { get; set; }
}

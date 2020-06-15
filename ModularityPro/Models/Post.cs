namespace ModularityPro.Models
{
  public class Post
  {
    public int PostId { get; set; }
    public string Content { get; set; }
    public virtual ApplicationUser User { get; set; }
  }
}
namespace ModularityPro.Models
{
  public class Message
  {
    public int MessageId { get; set; }
    public ApplicationUser ToUser { get; set; }
    public ApplicationUser FromUser { get; set; }
    public string Content { get; set; }
  }
}
namespace ModularityPro.Models
{
  public class Friend
  {
    public int FriendId { get; set; }
    public ApplicationUser User { get; set; }
    public ApplicationUser UserFriend { get; set; }
    public bool Responded { get; set; }
    public bool Accepted { get; set; }
  }
}
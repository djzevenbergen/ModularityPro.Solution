using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ModularityPro.Models;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ModularityPro.Hubs
{
  public class ChatHub : Hub
  {
    private readonly ModularityProContext _db;

    public ChatHub(ModularityProContext db)
    {
      _db = db;
    }

    public async Task SendPrivateMessage(string toUserId, string fromUserName, string message)
    {
      await Clients.User(toUserId).SendAsync("ReceiveMessage", fromUserName, message);
    }

    public async Task NotifyFriendRequest(string toUserName, string fromUserName)
    {
      ApplicationUser toUser = await _db.Users.Where(users => users.UserName == toUserName).FirstOrDefaultAsync();
      ApplicationUser fromUser = await _db.Users.Where(users => users.UserName == fromUserName).FirstOrDefaultAsync();
      await Clients.User(toUser.Id).SendAsync("ReceiveFriendRequest", $"{fromUser.FirstName} {fromUser.LastName}");
    }
  }
}
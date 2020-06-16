using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ModularityPro.Models;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ModularityPro.Hubs
{
  public class ChatHub : Hub
  {
    private readonly ModularityProContext _db;
    public Dictionary<string, string> UserConnections;

    public ChatHub(ModularityProContext db)
    {
      _db = db;
      UserConnections = new Dictionary<string, string>();
    }

    public override async Task OnConnectedAsync()
    {
      string connectionId = Context.ConnectionId;
      await Groups.AddToGroupAsync(connectionId, "OnlineUsers");

      await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      string connectionId = Context.ConnectionId;
      await Groups.RemoveFromGroupAsync(connectionId, "OnlineUsers");

      await base.OnDisconnectedAsync(exception);
    }

    public async Task SendPrivateMessage(string toUserId, string fromUserName, string message)
    {
      await Clients.User(toUserId).SendAsync("ReceiveMessage", fromUserName, message);
    }

    public async Task NotifyFriendRequest(string toUserName, string fromUserName)
    {
      Console.WriteLine("NOTIFYFRIENDREQUEST CALLED");
      ApplicationUser toUser = await _db.Users.Where(users => users.UserName == toUserName).FirstOrDefaultAsync();
      ApplicationUser fromUser = await _db.Users.Where(users => users.UserName == fromUserName).FirstOrDefaultAsync();
      await Clients.User(toUser.Id).SendAsync("ReceiveFriendRequest", $"{fromUser.FirstName} {fromUser.LastName}");
    }

  }
}
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ModularityPro.Models;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Security.Claims;

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
      string userId = Context.UserIdentifier;
      // ApplicationUser toUser = _db.Users.Where(users => users.Id == userId).FirstOrDefault();
      // await AddUserToChatBar($"{toUser.FirstName} {toUser.LastName}", toUser.UserName, toUser.AvatarUrl);
      await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      string connectionId = Context.ConnectionId;
      await Groups.RemoveFromGroupAsync(connectionId, "OnlineUsers");

      await base.OnDisconnectedAsync(exception);
    }

    public async Task SendPrivateMessage(string toUserName, string fromUserName, string message)
    {
      toUserName.Replace("#", "");
      ApplicationUser toUser = await _db.Users.Where(users => users.UserName == toUserName).FirstOrDefaultAsync();
      ApplicationUser fromUser = await _db.Users.Where(users => users.UserName == fromUserName).FirstOrDefaultAsync();
      string newMessage = fromUser.FirstName + " " + fromUser.LastName + ": " + message;
      Message storeMessage = new Message();
      storeMessage.FromUser = fromUser;
      storeMessage.ToUser = toUser;
      storeMessage.Content = newMessage;
      _db.Messages.Add(storeMessage);
      _db.SaveChanges();
      await Clients.User(toUser.Id.ToString()).SendAsync("ReceiveMessage", $"{fromUser.FirstName} {fromUser.LastName}", fromUser.UserName, newMessage);
    }

    public async Task NotifyFriendRequest(string toUserName, string fromUserName)
    {
      Console.WriteLine("NOTIFYFRIENDREQUEST CALLED");
      ApplicationUser toUser = _db.Users.Where(users => users.UserName == toUserName).FirstOrDefault();
      ApplicationUser fromUser = _db.Users.Where(users => users.UserName == fromUserName).FirstOrDefault();
      await Clients.User(toUser.Id).SendAsync("ReceiveFriendRequest", $"{fromUser.FirstName} {fromUser.LastName}");
    }

    public async Task AddUserToChatBar(string userRealName, string userName, string avatarUrl)
    {
      await Clients.All.SendAsync("AddToChat", userRealName, userName, avatarUrl);
    }

    public async Task SendVideoInvite(string toUserName, string fromUserName, string VideoUrl)
    {
      ApplicationUser toUser = _db.Users.Where(users => users.Id == toUserName).FirstOrDefault();
      ApplicationUser fromUser = _db.Users.Where(users => users.UserName == fromUserName).FirstOrDefault();
      await Clients.User(toUser.Id).SendAsync("ReceiveVideoInvite", $"{fromUser.FirstName} {fromUser.LastName}", VideoUrl);
    }

  }
}
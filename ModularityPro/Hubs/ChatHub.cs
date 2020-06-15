using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ModularityPro.Models;
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
  }
}
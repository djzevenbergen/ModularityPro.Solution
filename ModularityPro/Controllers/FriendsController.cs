using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using ModularityPro.Models;
using System.Web;
using System;

namespace ModularityPro.Controllers
{
  public class FriendsController : Controller
  {
    private readonly ModularityProContext _db;

    public FriendsController(ModularityProContext db)
    {
      _db = db;
    }

    public ActionResult Requests()
    {
      var myUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      List<Friend> allSentRequests = _db.Friends.Where(users => users.User.Id == myUserId && users.Responded == false).Include(users => users.UserFriend).ToList();
      ViewBag.SentFriendRequests = allSentRequests;
      List<Friend> allReceivedRequests = _db.Friends.Where(users => users.UserFriend.Id == myUserId && users.Responded == false).Include(users => users.User).ToList();
      ViewBag.ReceivedFriendRequests = allReceivedRequests;
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      return View();
    }

    public ActionResult AddFriend(string userName)
    {
      ApplicationUser userToAdd = _db.Users.Where(users => users.UserName == userName).FirstOrDefault();
      var thisUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      ApplicationUser thisUser = _db.Users.Where(users => users.Id == thisUserId).FirstOrDefault();

      Friend requestExists = _db.Friends.Where(users => users.User.Id == thisUser.Id && users.UserFriend.Id == userToAdd.Id && users.Responded == false).FirstOrDefault();
      if (requestExists == null)
      {
        Friend request = new Friend();
        request.User = thisUser;
        Console.WriteLine(userToAdd.Id);
        request.UserFriend = userToAdd;

        _db.Friends.Add(request);
        _db.SaveChanges();
      }
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      return RedirectToAction("Requests", "Friends");
    }


    public ActionResult ConfirmFriend(string userName)
    {
      var myUserName = User.FindFirstValue(ClaimTypes.Name);
      Friend friendship = _db.Friends.Where(friends => friends.User.UserName == userName).Where(friends => friends.UserFriend.UserName == myUserName).Include(users => users.User).Include(users => users.UserFriend).FirstOrDefault();
      friendship.Accepted = true;
      friendship.Responded = true;

      Friend reverseFriendship = new Friend();
      reverseFriendship.User = friendship.UserFriend;
      reverseFriendship.UserFriend = friendship.User;
      reverseFriendship.Accepted = true;
      reverseFriendship.Responded = true;

      _db.Friends.Add(reverseFriendship);
      _db.Entry(friendship).State = EntityState.Modified;
      _db.SaveChanges();
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      return RedirectToAction("Requests", "Friends");
    }


    public ActionResult DenyFriend(string userName)
    {
      var myUserName = User.FindFirstValue(ClaimTypes.Name);
      Friend friendship = _db.Friends.Where(friends => friends.User.UserName == userName).Where(friends => friends.UserFriend.UserName == myUserName).FirstOrDefault();
      friendship.Accepted = false;
      friendship.Responded = true;
      _db.Friends.Remove(friendship);
      _db.SaveChanges();
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      return RedirectToAction("Requests", "Friends");
    }
  }
}

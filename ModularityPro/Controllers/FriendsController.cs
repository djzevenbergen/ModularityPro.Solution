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
using ModularityPro.Hubs;

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
      return View();
    }

    //public ActionResult AddFriend(string userName)
    //{
    //   ApplicationUser userToAdd = _db.Users.Where(users => users.UserName == userName).FirstOrDefault();
    //   var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //   ApplicationUser thisUser = _db.Users.Where(users => users.Id == userId).FirstOrDefault();
    //   Friend newFriend = new Friend();
    //   newFriend.User = thisUser;
    //   newFriend.UserFriend = userToAdd;
    //   return RedirectToAction("Requests", "Friends");
    // }
  }
}
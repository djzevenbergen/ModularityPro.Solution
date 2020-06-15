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
  public class HomeController : Controller
  {
    private readonly ModularityProContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ModularityProContext db, UserManager<ApplicationUser> userManager)
    {
      //   UserStateViewModel = new UserStateViewModel();
      _db = db;
      _userManager = userManager;
    }

    public ActionResult Index()
    {
      ApplicationUser thisUser = _db.Users.Where(user => user.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
      ViewBag.User = thisUser;
      return View();
    }

    [HttpGet("/FindFriends")]
    public ActionResult FindFriends()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      List<ApplicationUser> allUsers = _db.Users.Where(user => user.Id != userId).ToList();
      return View(allUsers);
    }

    // [HttpGet("/Profile/{name}")]
    // public ActionResult Profile(string name)
    // {
    //   ViewBag.Friends = _db.Friends.Where(user => user.User.UserName == name).Include(user => user.UserFriend).ToList();
    //   ApplicationUser thisUser = _db.Users.Where(user => user.UserName == name).FirstOrDefault();
    //   return View(thisUser);
    // }

    [HttpPost]
    public ActionResult AddFriend(string userName)
    {
      ApplicationUser userToAdd = _db.Users.Where(users => users.UserName == userName).FirstOrDefault();
      Friend newFriend = new Friend();
      string myName = User.FindFirstValue(ClaimTypes.Name);
      newFriend.User = _db.Users.Where(users => users.UserName == myName).FirstOrDefault();
      newFriend.UserFriend = userToAdd;
      _db.Friends.Add(newFriend);
      _db.SaveChanges();
      Console.WriteLine(userName);
      return RedirectToAction("Index", "Home");
    }

    [HttpGet("/Chat")]
    public ActionResult Chat()
    {
      string myName = User.FindFirstValue(ClaimTypes.Name);
      ApplicationUser thisUser = _db.Users.Where(users => users.UserName == myName).FirstOrDefault();
      ViewBag.User = thisUser;
      ViewBag.Friends = _db.Friends.Where(user => user.User.UserName == myName).Include(user => user.UserFriend).ToList();
      ViewBag.Messages = _db.Messages.Where(message => message.FromUser.Id == thisUser.Id);


      return View();
    }

    [HttpGet("/search")]

    public ActionResult Search(string search) //, string searchParam
    {

      var model = from m in _db.Users select m;

      List<ApplicationUser> matchesUser = new List<ApplicationUser> { };

      if (!string.IsNullOrEmpty(search))
      {
        foreach (ApplicationUser user in model)
        {
          if (user.FirstName.ToLower().Contains(search) || user.LastName.ToLower().Contains(search))
          {
            matchesUser.Add(user);
          }
        }
      }
      return View(matchesUser);
    }
    public ActionResult GetData()
    {

      string myName = User.FindFirstValue(ClaimTypes.Name);
      ApplicationUser thisUser = _db.Users.Where(users => users.UserName == myName).FirstOrDefault();
      return Content(thisUser.AvatarUrl); // Of whatever you need to return.
    }

    [HttpGet]
    public ActionResult Test()
    {
      return View();
    }
  }
}
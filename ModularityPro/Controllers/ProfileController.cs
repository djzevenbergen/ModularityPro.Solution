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
  public class ProfileController : Controller
  {
    private readonly ModularityProContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(ModularityProContext db, UserManager<ApplicationUser> userManager)
    {
      _db = db;
      _userManager = userManager;
    }

    [HttpGet("Profile/{name}")]
    public ActionResult Index(string name)
    {
      ViewBag.Friends = _db.Friends.Where(user => user.User.UserName == name).Include(user => user.UserFriend).ToList();
      ApplicationUser thisUser = _db.Users.Where(user => user.UserName == name).FirstOrDefault();
      ViewBag.AvatarUrl = $"https://api.adorable.io/avatars/100/{thisUser.UserName}.png";
      List<Post> userPosts = _db.Posts.Where(posts => posts.User.Id == thisUser.Id).OrderByDescending(posts => posts.PostId).ToList();
      ViewBag.Posts = userPosts;
      return View(thisUser);
    }

    [HttpGet("Profile/Edit/{name}")]
    public ActionResult Edit(string name)
    {
      ViewBag.Friends = _db.Friends.Where(user => user.User.UserName == name).Include(user => user.UserFriend).ToList();
      ApplicationUser thisUser = _db.Users.Where(user => user.UserName == name).FirstOrDefault();
      ViewBag.AvatarUrl = $"https://api.adorable.io/avatars/100/{thisUser.UserName}.png";
      return View(thisUser);
    }

    [HttpPost("Profile/Edit/{name}")]
    public ActionResult Edit(ApplicationUser user)
    {
      ApplicationUser thisUser = _db.Users.Where(users => users.Id == user.Id).FirstOrDefault();
      thisUser.Bio = user.Bio;
      thisUser.FirstName = user.FirstName;
      thisUser.LastName = user.LastName;
      thisUser.Email = user.Email;
      _db.Entry(thisUser).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index", "Profile"); // not sure how it knows the user ID #blackmagic
    }
  }
}
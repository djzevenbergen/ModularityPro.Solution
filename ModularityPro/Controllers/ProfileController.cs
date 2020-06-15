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
      return View(thisUser);
    }

  }

}

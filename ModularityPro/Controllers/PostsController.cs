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
  public class PostsController : Controller
  {
    private readonly ModularityProContext _db;

    public PostsController(ModularityProContext db)
    {
      _db = db;
    }

    [HttpPost]
    public ActionResult ProfilePost(string UserName, string Content)
    {
      Post newPost = new Post();
      ApplicationUser postUser = _db.Users.Where(users => users.UserName == UserName).FirstOrDefault();
      newPost.Content = Content;
      newPost.User = postUser;
      _db.Posts.Add(newPost);
      _db.SaveChanges();
      return RedirectToAction("Index", "Profile", new { name = postUser.UserName });
    }

    [HttpPost]
    public ActionResult HomePost(string UserName, string Content)
    {
      Post newPost = new Post();
      ApplicationUser postUser = _db.Users.Where(users => users.UserName == UserName).FirstOrDefault();
      newPost.Content = Content;
      newPost.User = postUser;
      _db.Posts.Add(newPost);
      _db.SaveChanges();
      return RedirectToAction("Index", "Home");
    }
  }
}
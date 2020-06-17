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

    [HttpDelete]
    public ActionResult DeletePost(int id)
    {
      ApplicationUser ThisUser = _db.Users.Where(user => user.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
      Post SpecificPost = _db.Posts.Where(posts => posts.PostId == id).FirstOrDefault();
      if (SpecificPost.User.Id == ThisUser.Id)
      {
        var PostToDelete = _db.Posts.FirstOrDefault(entry => entry.PostId == id);
        _db.Posts.Remove(PostToDelete);
        _db.SaveChanges();
        return RedirectToAction("Index", "Home");
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }
    }

    [HttpPut]
    public void EditPost(int id, [FromBody] Post post)
    {
      ApplicationUser ThisUser = _db.Users.Where(user => user.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
      Post SpecificPost = _db.Posts.Where(posts => posts.PostId == id).FirstOrDefault();
      if (SpecificPost.User.Id == ThisUser.Id)
      {
        post.PostId = id;
        _db.Entry(post).State = EntityState.Modified;
        _db.SaveChanges();
      }
    }
  }
}
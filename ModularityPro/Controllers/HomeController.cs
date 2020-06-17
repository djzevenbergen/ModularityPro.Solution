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
using Microsoft.AspNetCore.Http.Extensions;
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
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      ViewBag.User = thisUser;

      List<Post> postIds = new List<Post>();

      foreach (Friend f in allFriends)
      {
        List<Post> allPostsByThisUser = _db.Posts.Where(posts => posts.User.UserName == f.UserFriend.UserName || posts.User.UserName == thisUser.UserName).ToList();
        // Console.WriteLine("ALLPOSTSBYUSER " + allPostsByThisUser.Count);
        foreach (Post p in allPostsByThisUser)
        {
          if (!postIds.Contains(p))
          {
            postIds.Add(p);
            // this whole method pull all posts by the user's friends and then returns them in a sorted list
            // Console.WriteLine(p.Content + "by:" + p.User.UserName);
          }
        }
      }
      SortPosts(postIds, 0, postIds.Count - 1);

      ViewBag.Posts = postIds;
      // Console.WriteLine("VIEWBAGPOSTS " + ViewBag.Posts);

      return View();
    }

    public static void SortPosts(List<Post> posts, int left, int right)
    {
      if (left >= right)
      {
        return;
      }

      int pivot = posts[(left + right) / 2].PostId;
      int index = PartitionPosts(posts, left, right, pivot);
      SortPosts(posts, left, index - 1);
      SortPosts(posts, index, right);
    }

    public static int PartitionPosts(List<Post> posts, int left, int right, int pivot)
    {
      while (left <= right)
      {
        while (posts[left].PostId > pivot)
        {
          left++;
        }
        while (posts[right].PostId < pivot)
        {
          right--;
        }

        if (left <= right)
        {
          Post temp = posts[left];
          posts[left] = posts[right];
          posts[right] = temp;
          left++;
          right--;
        }
      }
      return left;
    }

    [HttpGet("/FindFriends")]
    public ActionResult FindFriends()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      List<ApplicationUser> allUsers = _db.Users.Where(user => user.Id != userId).ToList();
      return View(allUsers);
    }

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

    [HttpGet("/Chat/{name}")]
    public ActionResult Chat(string name)
    {
      List<Message> allMessages = _db.Messages.Where(messages => (messages.FromUser.UserName == User.FindFirstValue(ClaimTypes.Name) && messages.ToUser.UserName == name) || (messages.ToUser.UserName == User.FindFirstValue(ClaimTypes.Name) && messages.FromUser.UserName == name)).ToList();
      SortMessages(allMessages, 0, allMessages.Count - 1);
      string myName = User.FindFirstValue(ClaimTypes.Name);
      ApplicationUser thisUser = _db.Users.Where(users => users.UserName == myName).FirstOrDefault();
      ViewBag.User = thisUser;
      ViewBag.Friends = _db.Friends.Where(user => user.User.UserName == myName).Include(user => user.UserFriend).ToList();
      ViewBag.Messages = allMessages;
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;

      return View();
    }

    public static void SortMessages(List<Message> messages, int left, int right)
    {
      if (left >= right)
      {
        return;
      }

      int pivot = messages[(left + right) / 2].MessageId;
      int index = PartitionMessages(messages, left, right, pivot);
      SortMessages(messages, left, index - 1);
      SortMessages(messages, index, right);
    }

    public static int PartitionMessages(List<Message> messages, int left, int right, int pivot)
    {
      while (left <= right)
      {
        while (messages[left].MessageId < pivot)
        {
          left++;
        }
        while (messages[right].MessageId > pivot)
        {
          right--;
        }

        if (left <= right)
        {
          Message temp = messages[left];
          messages[left] = messages[right];
          messages[right] = temp;
          left++;
          right--;
        }
      }
      return left;
    }

    [HttpGet("/Video")]
    public ActionResult Video()
    {
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      return View();
    }

    [HttpGet("/search")]

    public ActionResult Search(string search) //, string searchParam
    {
      var myUserName = User.FindFirstValue(ClaimTypes.Name);
      var model = from m in _db.Users select m;

      List<ApplicationUser> matchesUser = new List<ApplicationUser> { };

      if (!string.IsNullOrEmpty(search))
      {
        foreach (ApplicationUser user in model)
        {
          if ((user.FirstName.ToLower().Contains(search) || user.LastName.ToLower().Contains(search)) && user.UserName != myUserName)
          {
            matchesUser.Add(user);
          }
        }
      }
      else
      {
        var usersMinus = model.Where(m => m.UserName != myUserName);
        matchesUser = usersMinus.ToList();
      }
      ViewBag.SearchString = search;
      List<Friend> allFriends = _db.Friends.Where(users => users.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && users.Accepted == true).Include(users => users.UserFriend).ToList();
      ViewBag.AllFriends = allFriends;
      return View(matchesUser);
    }

    public string GetData()
    {
      string myName = User.FindFirstValue(ClaimTypes.Name);
      ApplicationUser thisUser = _db.Users.Where(users => users.UserName == myName).FirstOrDefault();
      return thisUser.AvatarUrl;
    }

    [HttpGet]
    public ActionResult Test()
    {
      return View();
    }

    public ActionResult Edit(int id)
    {
      Post EditablePost = _db.Posts.FirstOrDefault(entry => entry.PostId == id);
      return View(EditablePost);
    }


    [HttpPut]
    public void Edit(int id, [FromBody] Post post)
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




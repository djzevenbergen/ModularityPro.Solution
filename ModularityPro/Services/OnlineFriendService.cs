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
using Microsoft.AspNetCore.Http;

namespace ModularityPro.Services
{
  public class OnlineFriendService
  {
    private readonly ModularityProContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public Dictionary<string, string> OnlineFriends { get; set; }

    public OnlineFriendService(ModularityProContext db, UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
      _db = db;
      OnlineFriends = new Dictionary<string, string>();
    }

    public void GetOnlineFriends()
    {
      List<ApplicationUser> allUsers = _db.Users.Where(users => users.LoggedIn == true).ToList();

      foreach (ApplicationUser user in allUsers)
      {
        if (!OnlineFriends.ContainsKey(user.UserName))
        {
          OnlineFriends.Add(user.UserName, $"{user.FirstName} {user.LastName}");
        }
      }
      //List<Friend> allFriends = _db.Friends.Where()
    }

    //     public void GetUser()
    //     {
    //       ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
    // ManagementObjectCollection collection = searcher.Get();
    // string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
    //     }
  }

}
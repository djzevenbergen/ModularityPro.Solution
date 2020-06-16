using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ModularityPro.ViewModels;
using ModularityPro.Models;
using System.Text;
using System.ServiceModel;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Microsoft.AspNetCore.Session;

namespace ModularityPro.Controllers
{
  public class AccountController : Controller
  {
    private readonly ModularityProContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    //private readonly ISession session;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ModularityProContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public ActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      string url = $"https://api.adorable.io/avatars/100/{model.UserName}.png";
      ApplicationUser user = new ApplicationUser { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, AvatarUrl = url };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);

      // var userWithClaims = (ClaimsPrincipal)User;
      // var avatar = userWithClaims.Claims.First(c => c.Type == "AvatarUrl");

      // ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
      // if (null != principal)
      // {
      //   foreach (Claim claim in principal.Claims)
      //   {
      //     Response.Write("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value + "</br>");
      //   }
      // }

      if (result.Succeeded)
      {
        return RedirectToAction("Login", "Account");
      }
      else
      {
        return View();
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
          .PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);

      // var identity = (ClaimsIdentity)User.Identity;
      // IEnumerable<Claim> claims = identity.Claims;
      // claims.Add()
      // var identity = (ClaimsIdentity)User.Identity;
      // IEnumerable<Claim> claims = identity.Claims;
      // identity.AddClaim(new Claim("AvatarUrl", $"https://api.adorable.io/avatars/100/{model.UserName}.png"));

      if (result.Succeeded)
      {
        //HttpContext.Session("AvatarUrl", $"https://api.adorable.io/avatars/100/{model.UserName}.png");
        byte[] bytes = Encoding.ASCII.GetBytes(model.UserName);
        HttpContext.Session.Set("AvatarUrl", bytes);
        //HttpContext.Session.
        return RedirectToAction("Index", "Home");
      }
      else
      {
        return View();
      }
    }

    [HttpPost]
    public async Task<ActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}
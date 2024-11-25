using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class AccountController : Controller
{
    private UserManager<IdentityUser> userManager;
    private SignInManager<IdentityUser> signInManager;

    public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
    {
        userManager = userMgr;
        signInManager = signInMgr;
    }

    public ViewResult Login(string returnUrl) =>
        View(new LoginModel { Name = string.Empty, Password = string.Empty, ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityUser? user = await userManager.FindByNameAsync(model.Name);
            if (user != null)
            {
                await signInManager.SignOutAsync();
                if ((await signInManager.PasswordSignInAsync(user,
                        model.Password, false, false)).Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid name or password");
        }
        return View(model);
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }
}
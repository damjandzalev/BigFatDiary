using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Linq;
using BigFatDiary.Models.View;

namespace BigFatDiary.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        private ApplicationUserManager _userManager;
        public AdministratorController() { }
        public AdministratorController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Home()
        {
            return View();
        }

        public async Task<ActionResult> ListUsers()
        {
            List<AdministratorUser> users = new List<AdministratorUser>();
            foreach (var u in UserManager.Users)
            {
                if (u.UserName != this.User.Identity.Name)
                {
                    users.Add(new AdministratorUser
                    {
                        Id = u.Id,
                        Username = u.UserName,
                        Email = u.Email
                    });
                }
            }
            foreach (var u in users)
            {
                List<string> role = new List<string>();
                role = (List<string>)await UserManager.GetRolesAsync(u.Id);
                u.Role = "";
                foreach (string s in role)
                {
                    if (u.Role != "")
                        u.Role += ",";
                    u.Role += s;
                }
            }

            if (User.IsInRole("Administrator"))
            {
                var model = new List<AdministratorUser>();
                foreach (var u in users)
                {
                    if (u.Role != "Administrator" && u.Role != "MasterAdmin")
                    {
                        model.Add(u);
                    }
                }
                users = model;
            }
            return View(users);
        }

        public async Task<ActionResult> EditUser(string Id)
        {
            var u = await UserManager.FindByIdAsync(Id);
            AdministratorUser user = new AdministratorUser
            {
                Id = Id,
                Username = u.UserName,
                Email = u.Email,
            };
            List<string> role = new List<string>();
            role = (List<string>)await UserManager.GetRolesAsync(u.Id);
            user.Role = "";
            foreach (string s in role)
            {
                if (user.Role != "")
                    user.Role += ",";
                user.Role += s;
            }
            if (user.Username == this.User.Identity.Name ||
                (!User.IsInRole("MasterAdmin") && user.Role.Contains("Administrator")))
            {
                return Redirect("~/Administrator/ListUsers");
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> EditUser(AdministratorUser user)
        {
            if (user.Role == "Administrator" || user.Role == "Moderator" || user.Role == "User")
            {
                var roles = await UserManager.GetRolesAsync(user.Id);
                await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
                await UserManager.AddToRoleAsync(user.Id, user.Role);
                return Redirect("~/Administrator/ListUsers");
            }
            else
            {
                return await EditUser(user.Id);
            }
        }
    }
}
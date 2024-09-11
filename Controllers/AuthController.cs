using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using SMS.Models;
using SMSWEBAPI.Models;
using System.Text;

namespace SMS.Controllers
{
    public class AuthController : Controller
    {
        HttpClient client;

        public AuthController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }
        public IActionResult Index()
        {
            return View();
        }
        [AcceptVerbs("Post", "Get")]
        public IActionResult CheckExistingEmailId(string username)
        {
            string url = $"https://localhost:44386/api/Admin/CheckExistingEmailId/{username}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json($"username {username} already in used");
            }
            return Json(true);


        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User u)
        {
            string url = "https://localhost:44386/api/Auth/Login";
            var jsondata = JsonConvert.SerializeObject(u);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
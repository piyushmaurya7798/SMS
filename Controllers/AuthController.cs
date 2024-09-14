using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
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
            int id;
            if (response.IsSuccessStatusCode)
            {
            var jsondata2 = response.Content.ReadAsStringAsync().Result;
            User s = JsonConvert.DeserializeObject<User>(jsondata2);
            HttpContext.Session.SetString("urole", s.URole);

                    if (s.URole=="Admin") { 
                
                HttpContext.Session.SetString("suser", s.username);
                return RedirectToAction("Calendar","Admin");
                }
                else if (s.URole=="Student")
                {
                HttpContext.Session.SetString("suser", s.username);
                string url2 = $"https://localhost:44386/api/Auth/GetStudentId/{s.username}";
                HttpResponseMessage responseMessage2 = client.GetAsync(url2).Result;
                    if (responseMessage2.IsSuccessStatusCode)
                    {
                        var jsondata3 = responseMessage2.Content.ReadAsStringAsync().Result;
                        id = JsonConvert.DeserializeObject<int>(jsondata3);
                        HttpContext.Session.SetString("sid", id.ToString());
                        return RedirectToAction("GetStudent","Students");
                    }
                    return View();
                } else if (s.URole=="Teacher")
                {
                HttpContext.Session.SetString("suser", s.username);
                    string url2 = $"https://localhost:44386/api/Auth/GetTeacherId/{s.username}";
                    HttpResponseMessage responseMessage2 = client.GetAsync(url2).Result;
                    if (responseMessage2.IsSuccessStatusCode)
                    {
                        var jsondata3 = responseMessage2.Content.ReadAsStringAsync().Result;
                        id = JsonConvert.DeserializeObject<int>(jsondata3);
                        HttpContext.Session.SetString("sid", id.ToString());
                        return RedirectToAction("AddAttendance", "Teacher");
                    }
                    return View();
                } else if (s.URole=="Librarian")
                {
                HttpContext.Session.SetString("suser", s.username);
                return RedirectToAction("Index","Librarian");
                    
                }

            }
            return View();
        }
    }
}
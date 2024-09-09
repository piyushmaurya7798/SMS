using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMS.Models;
using SMSWEBAPI.Models;
using System.Text;

namespace SMS.Controllers
{
    public class AdminController : Controller
    {
        HttpClient client;

        public AdminController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }
        public IActionResult DashBoard()
        {
            return View();
        }
        public IActionResult AddClass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddClass(Class c)
        {
            string url = "https://localhost:44386/api/Admin/AddClass";
            var jsondata = JsonConvert.SerializeObject(c);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return View();
        }
        
        public IActionResult AddTeacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTeacher(Teacher c)
        {
            string url = "https://localhost:44386/api/Admin/AddTeacher";
            var jsondata = JsonConvert.SerializeObject(c);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return View();
        }
        public IActionResult AddSubject()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSubject(Subject c)
        {
            string url = "https://localhost:44386/api/Admin/AddSubject";
            var jsondata = JsonConvert.SerializeObject(c);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return View();
        }
    }
}

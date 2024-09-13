using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMS.Models;
using SMSWEBAPI.Models;
using System.Text;
namespace SMS.Controllers
{
    public class TeacherController : Controller
    {
        HttpClient client;

        public TeacherController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }
      
        public IActionResult ApplyLeave()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ApplyLeave(TeacherLeave tl)
        {

            string url = "https://localhost:7264/api/Academic/AddLeave";
            var jsondata = JsonConvert.SerializeObject(tl);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Leave Applied Successfully";
                return RedirectToAction("ViewLeaves");
            }
            else
            {
                TempData["Msg"] = "Something Went Wrong";
                return RedirectToAction("ViewLeaves");
            }
        }
        [HttpGet]

        public IActionResult ViewLeaves(int id)
        {
            List<TeacherLeave> teacherLeaves = new List<TeacherLeave>();
            string url = $"https://localhost:7264/api/Academic/GetLeave/{id}";
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsondata = responseMessage.Content.ReadAsStringAsync().Result;
                teacherLeaves = JsonConvert.DeserializeObject<List<TeacherLeave>>(jsondata);
                if (teacherLeaves.Count == 0)
                {
                    TempData["Msg"] = "No Leaves Applied ";
                    return View(teacherLeaves);
                }
                return View(teacherLeaves);

            }
            else
            {
                TempData["Msg"] = "No Leaves";
                return View();
            }

        }

    }
}




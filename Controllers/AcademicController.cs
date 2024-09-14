using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMS.Models;
using System.Text;
using System.Xml;

namespace SMS.Controllers
{
    public class AcademicController : Controller
    {
        HttpClient client;

        public AcademicController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }


        public IActionResult Index()
        {
            List<Curriculum> curricula = new List<Curriculum>();
            string url = "https://localhost:44386/api/Academic/GetAllCurricula";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                curricula = JsonConvert.DeserializeObject<List<Curriculum>>(jsondata);
            }

            return View(curricula);
        }


        public IActionResult AddCurriculum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCurriculum(Curriculum curriculum)
        {
            string url = "https://localhost:44386/api/Academic/AddCurriculum";
            var jsonData = JsonConvert.SerializeObject(curriculum);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(curriculum);
        }
        public IActionResult AddTimetable()
        {
            return View();

        }
        [HttpPost]
        public IActionResult AddTimetable(TimeTable tt)
        {
            string url = "https://localhost:44386/api/Academic/AddTimeTable";
            var jsondata = JsonConvert.SerializeObject(tt);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Msg"] = "TimeTable Added Successfully";
                return RedirectToAction("AddTimetable");
            }
            else
            {
                TempData["Msg"] = "Couldn't add Timetable please try again";
                return View();
            }
        }
        public IActionResult GetTimeTable()
        {

            List<TimeTable> teach = new List<TimeTable>();

            string url = "https://localhost:44386/api/Academic/GetTimeTable";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                teach = JsonConvert.DeserializeObject<List<TimeTable>>(jsondata);
            }
            return View(teach);
        }
        public IActionResult AddSubjects()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSubjects(Subject sub)
        {
            string url = "https://localhost:44386/api/Academic/AddSub";
            var jsondata = JsonConvert.SerializeObject(sub);
            StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, stringContent).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Subject Added Succesfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Msg"] = "Something Went Wrong Please Try Again Later";
                return View();

            }
        }
        public IActionResult FetchSubjects()
        {
            List<Subject> sub = new List<Subject>();
            string url = "https://localhost:44386/api/Academic/GetSub";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                sub = JsonConvert.DeserializeObject<List<Subject>>(jsondata);
                return Json(sub);
            }
            else
            {
                return Json(null);
            }
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMS.Models;
using SMSWEBAPI.Models;
using System.Text;

namespace SMS.Controllers
{
    public class StudentsController : Controller
    {
        HttpClient client;
        public StudentsController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslpolicyErrors) =>
            { return true; };
            client = new HttpClient(clientHandler);

        }
        public IActionResult Index()
        {
            List<Student> emps = new List<Student>();
            string url = "https://localhost:44386/api/Students/GetAllStudents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<Student>>(jsondata);
                if (obj != null)
                {
                    emps = obj;
                }
            }

            return View(emps);

        }
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            string url = "https://localhost:44386/api/Students/AddStudent";
            var jsonData = JsonConvert.SerializeObject(student);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult GetStudent(int id)
        {
            string url = $"https://localhost:44386/api/Students/GetStudent/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Student student = new Student();
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<Student>(jsondata);
            }

            return View(student);
        }
        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            string url = "https://localhost:44386/api/Students/UpdateStudent";
            var jsonData = JsonConvert.SerializeObject(student);
            var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public IActionResult UpdateStudent(int id)
        {
            string url = $"https://localhost:44386/api/Students/GetStudent/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Student student = new Student();
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<Student>(jsondata);
            }

            return View(student);
        }

        public IActionResult DeleteStudent(int id)
        {
            string url = $"https://localhost:44386/api/Students/DeleteStudent/{id}";
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult AddAttendance()
        { return View(); }


        public IActionResult GetAllPerformances()
        {
            string url = "https://localhost:44386/api/Students/GetAllPerformances";
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<Performance> performances = new List<Performance>();
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                performances = JsonConvert.DeserializeObject<List<Performance>>(jsondata);
            }
            return Json(performances);
        }

        public IActionResult GetPerformance(int id)
        {
            string url = $"https://localhost:44386/api/Students/GetPerformance/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Performance performance = null;

            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                performance = JsonConvert.DeserializeObject<Performance>(jsondata);
            }

            return View(performance);
        }

        public IActionResult AddPerformance()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult AddPerformance(Performance performance)
        //{
        //    string url = "https://localhost:44386/api/Students/AddPerformance";
        //    var jsonData = JsonConvert.SerializeObject(performance);
        //    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = client.PostAsync(url, content).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("GetPerformances");
        //    }

        //    return View(performance);



        //}

        [HttpPost]
        public IActionResult AddPerformance(Performance tl)
        {

            string url = "https://localhost:44386/api/Students/AddPerformance";
            var jsondata = JsonConvert.SerializeObject(tl);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Added Performance Applied Successfully";
                return RedirectToAction("ViewLeaves");
            }
            else
            {
                TempData["Msg"] = "Something Went Wrong";
                return RedirectToAction("ViewLeaves");
            }
        }
        [HttpGet]
        public IActionResult GetTeacher()
        {
            List<Teacher> teach = new List<Teacher>();

            string url = "https://localhost:44386/api/Students/GetTeacher";
            //var jsondata = JsonConvert.SerializeObject(c);
            //StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                teach = JsonConvert.DeserializeObject<List<Teacher>>(jsondata);
            }
            return Json(teach);
        }
        [HttpGet]
        public IActionResult GetClass()
        {

            List<Class> teach = new List<Class>();

            string url = "https://localhost:44386/api/Students/GetClass";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                teach = JsonConvert.DeserializeObject<List<Class>>(jsondata);
            }
            return Json(teach);
        }

    }
}

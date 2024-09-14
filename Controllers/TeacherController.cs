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
            tl.TeacherId = HttpContext.Session.GetString("sid");
            string url = "https://localhost:44386/api/teacher/AddLeave";
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

        public IActionResult ViewLeaves()
        {
            string id = HttpContext.Session.GetString("sid");
            List<TeacherLeave> teacherLeaves = new List<TeacherLeave>();
            string url = $"https://localhost:44386/api/Teacher/GetLeave/{id}";
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
        public IActionResult AddAttendance()
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
        [HttpPost]
        public IActionResult AddAttendance(int[] id)
        {
            List<Student> students = new List<Student>();

            string url = $"https://localhost:44386/api/Students/AddAttendance/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<Student>>(jsondata);
                return View(students);

            }
            else
            {
                TempData["Msg"] = "";
                return View();
            }

        }
        public IActionResult GetAttendance(int id)
        {
            string url = $"https://localhost:44386/api/Students/GetAttendance/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudentAttendance attendance = new StudentAttendance();
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                attendance = JsonConvert.DeserializeObject<StudentAttendance>(jsondata);

            }

            return View(attendance);
        }
        public IActionResult ApprovetheRequest(int id)
        {

            string url = $"https://localhost:44386/api/User/ApprovLeaveRequest/{id}";

            HttpResponseMessage message = client.PutAsync(url, null).Result;
            if (message.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Successfully Approved request";
                return RedirectToAction("ViewLeaveRequest");
            }
            else
            {
                return View();
            }

        }
        public IActionResult RejecttheRequest(int id)
        {

            string url = $"https://localhost:44386/api/User/RejectLeaveRequest/{id}";

            HttpResponseMessage message = client.PutAsync(url, null).Result;
            if (message.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Successfully Rejected request";
                return RedirectToAction("ViewLeaveRequest");
            }
            else
            {
                return View();
            }

        }


    }
}




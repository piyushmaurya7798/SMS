using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMS.Models;
using SMSWEBAPI.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            
            student.Fees = 1.0;
            string url = "https://localhost:44386/api/Admin/AddStudent";
            var jsonData = JsonConvert.SerializeObject(student);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

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
        
        public IActionResult GetTeacher()
        {
            List<Teacher> teach = new List<Teacher>();

            string url = "https://localhost:44386/api/Admin/GetTeacher";
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
        
        public IActionResult NoOfTeachers()
        {
            var jsondata = "";
            string url = "https://localhost:44386/api/Admin/NoOfTeachers";
            
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                 jsondata = response.Content.ReadAsStringAsync().Result;
               
            }
            return Json(jsondata);
        }
        
        public IActionResult GetNoOfStudents()
        {
            var jsondata = "";
            string url = "https://localhost:44386/api/Admin/GetNoOfStudents";
            
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                 jsondata = response.Content.ReadAsStringAsync().Result;
               
            }
            return Json(jsondata);
        }

        //public IActionResult GetTeacher()
        //{
        //    List<Teacher> teach = new List<Teacher>();

        //    string url = "https://localhost:44386/api/Admin/GetTeacher";
        //    //var jsondata = JsonConvert.SerializeObject(c);
        //    //StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = client.GetAsync(url).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsondata = response.Content.ReadAsStringAsync().Result;
        //        teach = JsonConvert.DeserializeObject<List<Teacher>>(jsondata);
        //    }
        //    return Json(teach);
        //}


        public IActionResult ChatFunction(Subject c)
        {
            string url = "https://localhost:44386/api/Admin/AddSubject";
            var jsondata = JsonConvert.SerializeObject(c);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return View();
        }

        public IActionResult OnlineApplication()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OnlineApplication(ApplicationViewModel c)
        {
            c.Date = DateTime.Now;
            string url = "https://localhost:44386/api/Admin/Application";
            using (var form = new MultipartFormDataContent())
            {
                // Add other form fields
                form.Add(new StringContent(c.Email ?? ""), "Email");
                form.Add(new StringContent(c.Phone ?? ""), "Phone");
                form.Add(new StringContent(c.HighestQualification ?? ""), "HighestQualification");
                form.Add(new StringContent(c.Grade ?? ""), "Grade");
                form.Add(new StringContent(c.ApplyingFor ?? ""), "ApplyingFor");
                form.Add(new StringContent(c.Date.ToString("o")), "Date");

                if (c.LatestRecords != null && c.LatestRecords.Length > 0)
                {
                    var fileContent = new StreamContent(c.LatestRecords.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(c.LatestRecords.ContentType);
                    form.Add(fileContent, "LatestRecords", c.LatestRecords.FileName);
                }

                // Send the request to the Web API
                HttpResponseMessage response = await client.PostAsync(url, form);
                return View();
            }
        }
        public async Task<IActionResult> GetAllApplications()
        {
            string url = "https://localhost:44386/api/Admin/GetApplication";
            HttpResponseMessage response = await client.GetAsync(url);
            var jsonData = await response.Content.ReadAsStringAsync();
            var applications = JsonConvert.DeserializeObject<List<SMS.Models.Application>>(jsonData);

            // Pass the data to the view
            return View(applications);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest model)
        {
            if (model.Emails == null || !model.Emails.Any() || string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("Invalid request.");
            }

            // Prepare the data to send to the Web API
            var apiUrl = "https://localhost:44386/api/Admin/SendEmails"; // Your Web API URL
            var jsonData = JsonConvert.SerializeObject(model);

            // Use HttpClient to call the Web API
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true, message = "Emails sent successfully!" });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error sending emails via Web API.");
                }
            }
        }

        public IActionResult Fees()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Fees(Fees c)
        {
            string url = "https://localhost:44386/api/Admin/Fees";
            var jsondata = JsonConvert.SerializeObject(c);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return View();
        }

        [HttpGet]
        public IActionResult GetClass()
        {

            List<Class> teach = new List<Class>();

            string url = "https://localhost:44386/api/Admin/GetClass";
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

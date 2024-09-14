//using Microsoft.AspNetCore.Mvc;
//using SMS.Models;
//using SMSWEBAPI.Models;
//using System;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//namespace SMS.Controllers
//{
//    public class ChatController : Controller
//    {
//        private readonly IHttpClientFactory _clientFactory;

//        public ChatController(IHttpClientFactory clientFactory)
//        {
//            _clientFactory = clientFactory;
//        }
//        public async Task<IActionResult> Index()
//        {
//            var client = _clientFactory.CreateClient();
//            var response = await client.GetAsync("https://localhost:44386/api/Students/GetAllUsers");
//            var users = await response.Content.ReadFromJsonAsync<List<User>>();
//            return View(users);  // Load users into the view for the dropdown
//        }
//        [HttpGet]
//        public async Task<IActionResult> Chat(int receiverId)
//        {
//            var senderId = 1;  // Assume the sender is logged in and has id 1 (you can change this based on session)
//            var client = _clientFactory.CreateClient();

//            // Get messages between the logged-in user and the selected user
//            var response = await client.GetAsync($"https://localhost:44386/api/Chat/GetMessages?senderId={senderId}&receiverId={receiverId}");
//            var messages = await response.Content.ReadFromJsonAsync<List<Chat>>();

//            var chatViewModel = new ChatViewModel
//            {
//                SenderId = senderId,
//                ReceiverId = receiverId,
//                Messages = messages
//            };

//            return View(chatViewModel);
//        }

//        // Send a new message
//        [HttpPost]
//        public async Task<IActionResult> SendMessage(ChatViewModel model)
//        {
//            var client = _clientFactory.CreateClient();

//            var chat = new Chat
//            {
//                SenderId = model.SenderId,
//                ReceiverId = model.ReceiverId,
//                Message = model.Message,
//                Timestamp = DateTime.Now
//            };

//            // Call the Web API to send the message
//            var response = await client.PostAsJsonAsync("https://localhost:5001/api/Chat/SendMessage", chat);

//            return RedirectToAction("Chat", new { receiverId = model.ReceiverId });
//        }
//    }
//}

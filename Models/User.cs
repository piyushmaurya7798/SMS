using Microsoft.AspNetCore.Mvc;

namespace SMSWEBAPI.Models
{
    public class User
    {
        public int id { get; set; }
        [Remote(action: "CheckExistingEmailId", controller: "Auth")]

        public string? username { get; set; }
        public string? Password { get; set; }
        public string? URole { get; set; }
                     
    }
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Remote(action: "CheckExistingEmailId", controller: "Auth")]

        public string? username { get; set; }
        public string? Password { get; set; }
        public string? URole { get; set; }
    }
}

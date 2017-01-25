using System;

namespace DBSD.Services
{

    public class UserProperties
    {
        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Validate {
        public bool isAdmin { get; set; }
        public bool isUser { get; set; }
        public string GreetingName { get; set; }

    }


}

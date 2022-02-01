using System;
using System.Collections.Generic;
using System.Text;

namespace pomodoro.Data
{
    class Users
    {

        public Users()
        {
            Email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        public Users(string email, string username, string password)
        {
            Email = email;
            Username = username;
            Password = password;

        }

        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
      

    }
}

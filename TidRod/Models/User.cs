using System;
using System.Collections.Generic;
using System.Text;

namespace TidRod.Models
{
    public class User
    {
        public string Id { get; set; }

        public FileImage Image { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }
    }
}

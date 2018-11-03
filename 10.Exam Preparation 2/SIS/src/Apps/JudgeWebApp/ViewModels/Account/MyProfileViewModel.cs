using System.Collections.Generic;
using JudgeWebApp.Models;

namespace JudgeWebApp.ViewModels.Account
{
    public class MyProfileViewModel
    {
        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public ICollection<Submission> Submissions { get; set; }
    }
}
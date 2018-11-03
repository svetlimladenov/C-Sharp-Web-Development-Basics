namespace JudgeWebApp.Controllers
{
    public class SubmissionDetailsViewModel
    {
        public int SubmissionId { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public string Code { get; set; }

        public string ContestName { get; set; }

        public bool IsSuccessfull { get; set; }

    }
}
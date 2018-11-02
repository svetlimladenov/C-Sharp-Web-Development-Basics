namespace JudgeWebApp.ViewModels.Submissions
{
    public class BaseSubmissionViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public bool IsSuccessfull { get; set; }
    }
}

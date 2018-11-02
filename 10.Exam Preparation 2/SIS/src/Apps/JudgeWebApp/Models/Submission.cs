namespace JudgeWebApp.Models
{
    public class Submission
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public bool IsSuccessfull { get; set; } = false;

        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}

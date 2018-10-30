namespace JudgeWebApp.ViewModels.Contests
{
    public class BaseContestViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubmissionsCount { get; set; }

        public bool CreatedByUser { get; set; }
    }
}

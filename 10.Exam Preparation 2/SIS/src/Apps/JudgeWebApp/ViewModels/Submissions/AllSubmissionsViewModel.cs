using System.Collections.Generic;

namespace JudgeWebApp.ViewModels.Submissions
{
    public class AllSubmissionsViewModel
    {
        public ICollection<BaseContestAndSubmissionViewModel> ContestAndSubmission { get; set; }
    }
}

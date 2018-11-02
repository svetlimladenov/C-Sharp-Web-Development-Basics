using System.Collections.Generic;

namespace JudgeWebApp.ViewModels.Submissions
{
    public class BaseContestAndSubmissionViewModel
    {
        public string Name { get; set; }

        public ICollection<BaseSubmissionViewModel> Submissions { get; set; }
    }
}

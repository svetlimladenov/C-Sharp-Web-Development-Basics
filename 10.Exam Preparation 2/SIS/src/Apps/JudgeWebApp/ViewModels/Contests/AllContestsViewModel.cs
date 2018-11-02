using System.Collections.Generic;

namespace JudgeWebApp.ViewModels.Contests
{
    public class AllContestsViewModel
    {
        public string ContestNameSelected { get; set; }

        public ICollection<BaseContestViewModel> Contests { get; set; }

    }
}

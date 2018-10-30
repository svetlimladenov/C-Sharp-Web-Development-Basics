using TorshiaWebApp.Models;

namespace TorshiaWebApp.ViewModels.Reports
{
    public class ReportDetailsViewModel
    {
        public string Reporter { get; set; }

        public Task Task { get; set; }

        public Report Report { get; set; }

        public int TaskLevel { get; set; }

        public string TaskDueDate { get; set; }

        public string ReportReportedOn { get; set; }

        public string AffectedSectors { get; set; }

        public string TaskStatus { get; set; }
    }
}

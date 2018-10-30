using System;

namespace TorshiaWebApp.Models
{
    public class Report
    {
        public Report()
        {
            this.ReportedOn = DateTime.UtcNow;
        }
        public int Id { get; set; }

        public Status Status { get; set; }

        public DateTime ReportedOn { get; set; }

        public Task Task { get; set; }

        public User Reporter { get; set; }
    }
}

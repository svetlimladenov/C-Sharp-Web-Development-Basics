using System;
using System.Collections.Generic;

namespace TorshiaWebApp.Models
{
    public class Task
    {
        public Task()
        {
            this.TaskSectors = new HashSet<TaskSector>();
            this.DueDate = DateTime.UtcNow;
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Participants { get; set; }

        public bool IsReported { get; set; } = false;

        public virtual ICollection<TaskSector> TaskSectors { get; set; }
    }
}

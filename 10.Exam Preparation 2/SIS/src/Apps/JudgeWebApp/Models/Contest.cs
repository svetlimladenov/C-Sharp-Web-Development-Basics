using System.Collections.Generic;

namespace JudgeWebApp.Models
{
    public class Contest
    {
        public Contest()
        {
            this.Submissions = new HashSet<Submission>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;
 
        public int UserId { get; set; }

        public User User { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }    
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        public ICollection<TeamAssignment> PlayerAssignments { get; set; } = new List<TeamAssignment>();
    }
}

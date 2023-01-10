using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsManager.Application.Model
{
    [Table("Team")]
    public class Team 
    {
        public Team(string name, string schoolclass)
        {
            Name = name;
            Schoolclass = schoolclass;
        }

#pragma warning disable CS8618
        protected Team()
#pragma warning restore CS8618
        {
        }

        [Key] // System.ComponentModel.DataAnnotations; data... nur für int 
        [MaxLength(64)]
        public string Name { get; private set; }

        [MaxLength(16)]
        public string Schoolclass { get; set; }

        public string Id => Name;

        public virtual ICollection<Task> Tasks { get; } = new List<Task>();

        //In der Klasse Team liefert die Methode GetActiveTasks() alle Tasks zurück, wo das ExpirationDate größer als das übergebene Datum ist.
        public IReadOnlyCollection<Task> GetActiveTasks(DateTime dateTime) => Tasks.Where(t => t.ExpirationDate > dateTime).ToList();

        
    }
}

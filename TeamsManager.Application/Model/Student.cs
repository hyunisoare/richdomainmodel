using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsManager.Application.Model
{
    [Table("Student")]
    public class Student 
    {
        public Student(Name name)
        {
            Name = name;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Student() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int Id { get; private set; }
        public virtual Name Name { get; set; }  
        

        protected List<HandIn> _handIns = new();
        public virtual ICollection<HandIn> HandIns => _handIns;

    }
}

    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsManager.Application.Model
{
    //public interface IEntity<T>
    //{
    //    T Id { get; }
    //}
    [Table("HandIn")]
    public class HandIn 
    {
        public HandIn(Student student, DateTime date)
        {
            Student = student;
            StudentId = student.Id;
            Date = date;
            // Navigation set by EF Core.
            Task = default!;
        }

        /// <summary>
        /// Copyconstructor for changing the state from HandIn to ReviewedHandIn.
        /// Das muss nicht immer zugängig sein, sondern nur von Task geführt werden. deshalb ist es protected 
        /// </summary>
        /// <param name="handIn"></param>
        protected HandIn(HandIn handIn)
        {
            Id = handIn.Id;
            Student = handIn.Student;
            StudentId = handIn.StudentId;
            Date = handIn.Date;
            Task = handIn.Task;
            TaskId = handIn.TaskId;
        }



#pragma warning disable CS8618
        protected HandIn()
#pragma warning restore CS8618
        {
        }

        public int Id { get; private set; }

        public virtual Student Student { get; private set; }
        public int TaskId { get; private set; }
        public int StudentId { get; set; }
        public virtual Task Task { get; private set; }
        public DateTime Date { get; set; }

        public Guid Guid { get; }
        public string HandInType { get; private set; } = default!;  

    }
}

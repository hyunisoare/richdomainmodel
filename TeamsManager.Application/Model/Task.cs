using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TeamsManager.Application.Model
{
    [Table("Task")]
    public class Task 
    {
        public Task(string subject, string title, Team team, Teacher teacher, DateTime expirationDate)
        {
            Subject = subject;
            Title = title;
            Team = team;
            Teacher = teacher;
            ExpirationDate = expirationDate;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Task()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }
        public int Id { get; private set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public virtual Team Team { get; set; }
        public virtual Teacher Teacher { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? MaxPoints { get; set; }

        protected List<HandIn> _handIns = new();
        public virtual IReadOnlyCollection<HandIn> HandIns => _handIns;

        //protected List<ReviewedHandIn> _reviewedHandIns = new();
        //public virtual IReadOnlyCollection<ReviewedHandIn> ReviewedHandIns => _reviewedHandIns;

        //TryHandIn() fügt eine Abgabe hinzu, wenn das gespeicherte Abgabedatum innerhalb des Abgabezeitraumes(ExpirationDate) liegt.Ist dem nicht so, liefert die Methode false.
        public bool TryHandIn(HandIn handIn)
        {
            if (handIn.Date <= ExpirationDate)
            {
                _handIns.Add(handIn);
                return true;
            }
                
            else
                return false;
        }

        //ReviewHandIn() aktualisiert die übergebene Abgabe und erzeugt ein ReviewedHandIn.
        // statt Parameter habe ich alle ReviewedDate und Points gleich gegeben. 
        public void ReviewedHandIn(HandIn handIn, DateTime reviewDate, int points)
        {
            if (handIn is ReviewedHandIn) { return; }
                var reviewedHandIn = new ReviewedHandIn(handIn, reviewDate, points);

                _handIns.Remove(handIn);
                _handIns.Add(reviewedHandIn); 
        }

        public void AddHandIn(HandIn handIn)
        {
            _handIns.Add(handIn);
        }

        //todo
        //CalculateAveragePoints() kann als Property ausgeführt werden und berechnet die durchschnittlichen Punkte der Abgaben.Berücksichtige nur Abgaben vom Typ ReviewedHandIn.
        public decimal? CalculateAveragePoints => (decimal?)_handIns.FindAll(h => h is ReviewedHandIn).Cast<ReviewedHandIn>().Average(h => h.Points);

        public decimal CalculateAveragePoints2
        {
            get
            {
                return (decimal)_handIns.OfType<ReviewedHandIn>().Average(h => h.Points);
            }
        }
        

        //FirstHandInDate() kann ebenfalls als Property ausgeführt werden und liest das Datum jener Abgabe aus, die zeitlich gesehen als erstes abgegeben wurde.
        public DateTime FirstHandInDate() => _handIns.Min(h=>h.Date);
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RichDomainModelDemo.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TeamsManager.Application.Model;
using Xunit;
using Task = TeamsManager.Application.Model.Task;

namespace TeamsManager.Test
{
    public class TaskTests : DatabaseTest
    {
        public TaskTests()
        {
            _db.Database.EnsureCreated();
            var team1 = new Team(name: "theBestTeam", schoolclass: "5BKIF");
            _db.Teams.Add(team1);
          
            var teacher1 = new Teacher(new Name("Max", "Mustermann", "mustermann@spengergasse.at"));
            _db.Teachers.Add(teacher1);

            var task1 = new Task(
                            subject: "math",
                            title: "matrix",
                            team: team1,
                            teacher: teacher1,
                            expirationDate: new DateTime(2023, 01, 01)
                            );
           _db.Tasks.Add(task1);

            var student1 = new Student(new Name("Jun", "Oh", "oh@spengergasse.at"));
            _db.Students.Add(student1);

            var handIn1 = new HandIn(
                                student: student1,
                                date: new DateTime(2023, 01, 01));
            task1.AddHandIn(handIn1);

            _db.SaveChanges(); 

        }

        [Fact]
        //In der Klasse Team liefert die Methode GetActiveTasks() alle Tasks zurück, wo das ExpirationDate größer als das übergebene Datum ist.
        public void GetActiveTasksTest()
        {
            var team = _db.Teams.First();

            var task1 = new Task(
                           subject: "math",
                           title: "matrix",
                           team: _db.Teams.First(),
                           teacher: _db.Teachers.First(),
                           expirationDate: new DateTime(2023, 01, 01)
                           ) ;
            var task2 = new Task(
                           subject: "math",
                           title: "matrix",
                           team: _db.Teams.First(),
                           teacher: _db.Teachers.First(),
                           expirationDate: new DateTime(2023, 01, 02)
                           );
            var task3 = new Task(
                           subject: "math",
                           title: "matrix",
                           team: _db.Teams.First(),
                           teacher: _db.Teachers.First(),
                           expirationDate: new DateTime(2023, 01, 03)
                           );

            team.Tasks.Add(task1);
            team.Tasks.Add(task2);
            team.Tasks.Add(task3);

            Assert.Equal(1, team.GetActiveTasks(new DateTime(2023, 01, 02)).Count());


        }

        [Fact]
        //TryHandIn() fügt eine Abgabe hinzu, wenn das gespeicherte Abgabedatum innerhalb des Abgabezeitraumes(ExpirationDate) liegt.Ist dem nicht so, liefert die Methode false.
        public void TryHandInTrueTest()
        {
            var handIn = _db.HandIns.First(); // 01.02
            var task = _db.Tasks.First(); // 01.01

            Assert.True(task.TryHandIn(handIn));
        }

        //ReviewHandIn() aktualisiert die übergebene Abgabe und erzeugt ein ReviewedHandIn.
        [Fact]
        public void ReviewHandInSuccessTest()
        {
            var task = _db.Tasks.First();
            task.ReviewedHandIn(task.HandIns.First(), new DateTime(2023,01,02), 10);
            _db.SaveChanges(); 
            _db.ChangeTracker.Clear();

            task = _db.Tasks.First();
            Assert.True(task.HandIns.OfType<ReviewedHandIn>().Count()==1);
        }

        //CalculateAveragePoints() kann als Property ausgeführt werden und berechnet die durchschnittlichen Punkte der Abgaben.Berücksichtige nur Abgaben vom Typ ReviewedHandIn.
        [Fact]
        public void CalculateAveragePointsTest()
        {
            var task = _db.Tasks.First();
            var handIn1 = new HandIn(
                                student: _db.Students.First(),
                                date: new DateTime(2023, 01, 01));
            var handIn2 = new HandIn(
                                student: _db.Students.First(),
                                date: new DateTime(2023, 01, 01));

            task.ReviewedHandIn(handIn1, new DateTime(2023, 01, 04), 20);
            task.ReviewedHandIn(handIn2, new DateTime(2023, 01, 04), 30);

            Assert.Equal(25, task.CalculateAveragePoints); // = 50 / 2 
        }

        //FirstHandInDate() kann ebenfalls als Property ausgeführt werden und liest das Datum jener Abgabe aus, die zeitlich gesehen als erstes abgegeben wurde.
        [Fact]
        public void FirstHandInDateTest()
        {
            var task = _db.Tasks.First();

            var handIn1 = new HandIn(
                                student: _db.Students.First(),
                                date: new DateTime(2023, 01, 03));
            var handIn2 = new HandIn(
                                student: _db.Students.First(),
                                date: new DateTime(2023, 01, 02));
            var handIn3 = new HandIn(
                               student: _db.Students.First(),
                               date: new DateTime(2023, 01, 01));

            task.AddHandIn(handIn1);
            task.AddHandIn(handIn2);
            task.AddHandIn(handIn3);

            task.ReviewedHandIn(handIn3, new DateTime(2023, 01, 04), 7);

            Assert.True(task.FirstHandInDate() == new DateTime(2023, 01, 01));

        }
    }
}

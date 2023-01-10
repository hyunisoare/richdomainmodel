using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace TeamsManager.Application.Model
{
    //Der Name wird als value object definiert.Verwende C# 9 Records zur Definition des Objektes.
    //Achte auf die korrekte Konfiguration im Context mittels OwnsOne().
    public record Name([MaxLength(255)] string Firstname, [MaxLength(255)] string Lastname, [MaxLength(255)] string Email);


}

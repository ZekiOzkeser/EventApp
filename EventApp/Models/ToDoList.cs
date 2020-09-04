using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApp.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        public string ToDo { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateNotify { get; set; }

        // Kullanıcının uyarıyı görüp-görmediğini takip etmek için.
        public bool IsNotify { get; set; }

        //süresi olan
        public bool NotifyExpired() =>
            IsNotify || DateTime.Now >= DateNotify;

        //süresi yaklaşan 10dk die ayarladım, değiştirilebilir.
        public bool NotifyRequired()
        {
            var sure = (DateTime.Now - TimeSpan.FromMinutes(10));

            return !IsNotify &&
                DateNotify.Year >= sure.Year &&
                DateNotify.Month >= sure.Month &&
                DateNotify.Day >= sure.Day &&
                DateNotify.Hour >= sure.Hour &&
                DateNotify.Minute >= sure.Minute;
        }
        public bool NextDay { get; set; }

        public bool NextMonth { get; set; }
        public bool NextYear { get; set; }

        // değer girilmez ise 1 otomatik atanacak.
        public ToDoList AddDay(int days = 1)
        {
            if (days < 1)
                days = 1;

            return new ToDoList
            {
                ToDo = ToDo,
                DateAdded = DateAdded,
                IsNotify = false,
                NextDay = false,
                NextMonth = false,
                NextYear = false,
                DateNotify = DateNotify.AddDays(+days)
            };
        }

        public ToDoList AddMonth(int months = 1)
        {
            if (months < 1)
                months = 1;

            return new ToDoList
            {
                ToDo = ToDo,
                DateAdded = DateAdded,
                IsNotify = false,
                NextDay = false,
                NextMonth = false,
                NextYear = false,
                DateNotify = DateNotify.AddMonths(+months)
            };
        }

        public ToDoList AddYear(int years = 1)
        {
            if (years < 1)
                years = 1;

            return new ToDoList
            {
                ToDo = ToDo,
                DateAdded = DateAdded,
                IsNotify = false,
                NextDay = false,
                NextMonth = false,
                NextYear = false,
                DateNotify = DateNotify.AddYears(+years)
            };
        }
    }
}

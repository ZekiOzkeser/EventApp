using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Data
{
    public class DataContext
        :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {
          
        }

        public DbSet<ToDoList> ToDoLists { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Models;

namespace To_Do_List.Data
{
    public class To_Do_ListContext : IdentityDbContext<IdentityUser>
    {
        public To_Do_ListContext (DbContextOptions<To_Do_ListContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Exercise> Exercise { get; set; } = default!;
        public DbSet<To_Do_List.Models.Department> Department { get; set; } = default!;
        public DbSet<To_Do_List.Models.Vacancy> Vacancy { get; set; } = default!;
        public DbSet<To_Do_List.Models.AssignmentUser> AssignmentUser { get; set; } = default!;
        public DbSet<To_Do_List.Models.UserExerciseAssignment> UserExerciseAssignment { get; set; } = default!;
    }
}

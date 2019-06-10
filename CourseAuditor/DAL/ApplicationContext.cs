using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.DAL
{
    public class ApplicationContext : DbContext
    {
       
        public ApplicationContext() : base("default")
        {
            
        }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<PersonParent> PersonParents { get; set; }
        public DbSet<MedicalDoc> MedicalDocs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Assessment> Assessments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Group>()
            //    .HasRequired(x => x.Course)
            //    .WithMany(x => x.Groups)
            //    .HasForeignKey(x => x.Course_ID)
            //    .WillCascadeOnDelete(true);
        }
    }
}

using Diary.Models.Domains;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Configurations
{
    class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            ToTable("dbo.Students");
            HasKey(x => x.Id);
            Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}

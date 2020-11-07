using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WizLib_Model.Models;

namespace WizLib_DataAccess.FluentConfig
{
    class FluentAuthorConfig : IEntityTypeConfiguration<Fluent_Author>
    {
        public void Configure(EntityTypeBuilder<Fluent_Author> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Author_Id);
            modelBuilder.Property(a => a.FirstName).IsRequired();
            modelBuilder.Property(a => a.LastName).IsRequired();
            modelBuilder.Ignore(a => a.FullName);
        }
    }
}

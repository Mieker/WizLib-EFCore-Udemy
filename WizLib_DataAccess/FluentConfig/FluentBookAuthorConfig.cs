using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WizLib_Model.Models;

namespace WizLib_DataAccess.FluentConfig
{
    class FluentBookAuthorConfig : IEntityTypeConfiguration<Fluent_BookAuthor>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookAuthor> modelBuilder)
        {
            modelBuilder.HasKey(b => new { b.Book_Id, b.Author_Id });
            modelBuilder
                .HasOne(b => b.Fluent_Book)
                .WithMany(b => b.Fluent_BookAuthors)
                .HasForeignKey(b => b.Book_Id);
            modelBuilder
                .HasOne(b => b.Fluent_Author)
                .WithMany(b => b.Fluent_BookAuthors)
                .HasForeignKey(b => b.Author_Id);

        }
    }
}

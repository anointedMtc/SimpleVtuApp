using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Config;

public class AppRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {

        builder.Ignore(b => b.DomainEvents);

        // WHAT THIS ALL MEANS IS THAT... MY ID COLUMN IS ALREADY BY DEFAULT
        // MY CLUSTERED INDEX... AND ANY OTHER INDEX I AM DEFINING HERE ARE 
        // GOING TO BE NON-CLUSTERED INDEXES

        // by default, a unique clustered index is created on all primary keys
        // YOU CAN ONLY HAVE ONE "CLUSTERED INDEX" buy you can have several
        // non-clustered indexes.... - non-clustered indexes will create a new
        // seperate table where they would store info, then point back to the 
        // main table in case you want further info... (like back of book indexes)

        // clustered indexes are ideal for range-based queries and sorting,
        // while non-clustered indexes excel in optimizing specific lookups
        // and dynamic queries


        // Single Column Index
        // by default, indexes aren't unique: multiple rows are allowed to 
        // have the same value(s) for the index's column set. Attempting to
        // insert more than one entity with the same values for the index's
        // column set will cause an exception to be thrown
        builder.HasIndex(b => b.Name)
               .IsUnique();

        // Composite Index
        // An index can also span more than one column: Indexes over multiple
        // columns also known as "composite indexes" speed up queries which
        // filter on index's columns, BUT ALSO queries which only filter on
        // the first columns covered by the index... 

        //builder.HasIndex(b => new { b.Name, b.Id })
        //       .IsUnique();

        // to create multiple indexes over the same set of properties, 
        // pass a name to the HasIndex() method, which will be used to
        // identify the index in the EF model, and to distinguish it from
        // other indexes over the same properties

        //builder.HasIndex(b => new { b.Name, b.Id }, "IX_Names_Ascending")
        //       .IsUnique();

        //builder.HasIndex(b => new { b.Name, b.Id }, "IX_Names_Descending")
        //       .IsDescending()
        //       .IsUnique();

    }
}

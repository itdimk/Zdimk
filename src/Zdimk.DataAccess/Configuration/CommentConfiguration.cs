using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zdimk.Domain.Entities;

namespace Zdimk.DataAccess.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId);

            builder.HasOne(c => c.Picture)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PictureId);
        }
    }
}
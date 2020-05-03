using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zdimk.Domain.Entities;

namespace Zdimk.DataAccess.Configuration
{
    public class PictureTagConfiguration : IEntityTypeConfiguration<PictureTag>
    {
        public void Configure(EntityTypeBuilder<PictureTag> builder)
        {
            builder.HasKey(o => new { o.PictureId, o.TagName });

            builder.HasOne(o => o.Picture)
                .WithMany(p => p.PictureTags)
                .HasForeignKey(o => o.PictureId);

            builder.HasOne(o => o.Tag)
                .WithMany(t => t.PictureTags)
                .HasForeignKey(o => o.TagName);
        }
    }
}
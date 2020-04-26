using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zdimk.Domain.Entities;

namespace Zdimk.DataAccess.Configuration
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.HasOne(p => p.Album)
                .WithMany(a => a.Pictures)
                .HasForeignKey(p => p.AlbumId);
        }
    }
}
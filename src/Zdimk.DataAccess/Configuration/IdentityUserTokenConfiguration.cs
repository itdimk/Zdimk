using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zdimk.DataAccess.Configuration
{
    public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            builder.HasKey(t => new {t.UserId, t.Value});
            builder.HasIndex(t => t.Value);
        }
    }
}
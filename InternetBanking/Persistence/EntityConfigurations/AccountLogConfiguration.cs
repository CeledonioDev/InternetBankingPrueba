using Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    internal sealed class AccountLogConfiguration : IEntityTypeConfiguration<AccountLog>
    {
        public void Configure(EntityTypeBuilder<AccountLog> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).ValueGeneratedOnAdd();
        }
    }
}

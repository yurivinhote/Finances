using System.Transactions;
using Finances.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finances.api.Data.Mappings
{
    public class TransactionsMapping : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).
                IsRequired().
                HasColumnType("NVARCHAR")
                .HasMaxLength(80);
            builder.Property(x => x.Type).
                IsRequired(true).
                HasColumnType("SMALLINT")
                .HasMaxLength(80);
            builder.Property(x => x.Amount).
                IsRequired(true).
                HasColumnType("MONEY");
             builder.Property(x => x.CreateAt).
                 IsRequired(true);

             builder.Property(x => x.PaidOrReceiveAt).
                 IsRequired(false);

             builder.Property(x => x.UserId).
                 IsRequired(true).
                 HasColumnType("NVARCHAR").
                 HasMaxLength(160);
        }
    }
}

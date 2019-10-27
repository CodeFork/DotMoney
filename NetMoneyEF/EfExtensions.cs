using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetMoney;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace NetMoneyEF {
    public static class EfExtensions {
        public static EntityTypeBuilder<T> OwnsOneMoney<T>(this EntityTypeBuilder<T> builder,
                                                          [NotNull] Expression<Func<T, Money>> expression,
                                                          string moneyAmountColumnName = "Price",
                                                          string moneyCurrencyCodeColumnName = "Currency")
                where T: class
        {
            return builder.OwnsOne(expression, p => {
                p.Property(p => p.Amount).HasColumnName(moneyAmountColumnName).IsRequired();
                p.OwnsOne(c => c.Currency, c => {
                    c.Property(c => c.IsoCode).HasColumnName(moneyCurrencyCodeColumnName).IsRequired();
                    c.Ignore(c => c.DisplayingSubType);
                });
            });
        }
    }
}

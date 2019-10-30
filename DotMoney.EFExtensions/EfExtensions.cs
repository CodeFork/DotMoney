using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DotMoney.EFExtensions {
    public static class EFExtensions {
        public static EntityTypeBuilder<T> OwnsOneMoney<T>(this EntityTypeBuilder<T> builder,
                                                          [NotNull] Expression<Func<T, Money>> expression,
                                                          string moneyAmountColumnName = null,
                                                          string moneyCurrencyCodeColumnName = null)
                where T: class
        {
            return builder.OwnsOne(expression, m => {
                var amountProp = m.Property(p => p.Amount).IsRequired(true);

                if (moneyAmountColumnName != null)
                    amountProp.HasColumnName(moneyAmountColumnName);

                var isoCodeProp = m.Property(c => c.IsoCode).IsRequired(true);

                if (moneyCurrencyCodeColumnName != null)
                    isoCodeProp.HasColumnName(moneyCurrencyCodeColumnName);

                m.Ignore(p => p.Currency);
            });
        }
    }
}

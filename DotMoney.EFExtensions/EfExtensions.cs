﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DotMoney.EFExtensions {
    public static class EFExtensions {
        public static EntityTypeBuilder<T> OwnsOneMoney<T>(this EntityTypeBuilder<T> builder,
                                                          [NotNull] Expression<Func<T, Money>> expression,
                                                          [NotNull] string moneyAmountColumnName = "Price",
                                                          [NotNull] string moneyCurrencyCodeColumnName = "Currency")
                where T: class
        {
            return builder.OwnsOne(expression, m => {
                m.Property(p => p.Amount).HasColumnName(moneyAmountColumnName).IsRequired(true);
                m.Property(c => c.IsoCode).HasColumnName(moneyCurrencyCodeColumnName).IsRequired(true);
                m.Ignore(p => p.Currency);
            });
        }
    }
}

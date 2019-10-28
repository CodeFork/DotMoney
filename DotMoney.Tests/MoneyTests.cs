using Xunit;

namespace DotMoney.Tests {
    public class MoneyTests {
        [Fact]
        public void CurrencyIsEqual() {
            var cur1 = Currency.Get("BTC");
            var cur2 = Currency.Get("BTC");

            Assert.Equal(cur1, cur2);
        }

        [Fact]
        public void CurrencyIsNotEqual() {
            var cur1 = Currency.Get("HKD");
            var cur2 = Currency.Get("BTC");

            Assert.NotEqual(cur1, cur2);
        }

        [Fact]
        public void MoneyCurrencyIsNotEqual() {
            var m1 = new Money(1m, "HKD");
            var m2 = new Money(1m, "MOP");

            Assert.NotEqual(m1, m2);
        }

        [Fact]
        public void MoneyIsNotEqualAmount() {
            var m1 = new Money(1m, "HKD");
            var m2 = new Money(2m, "HKD");

            Assert.NotEqual(m1, m2);
        }

        [Fact]
        public void MoneyIsEqual() {
            var m1 = new Money(1.00000001m, "BTC");
            var m2 = new Money(1.00000001m, "BTC");

            Assert.Equal(m1, m2);
        }

        [Fact]
        public void MoneyAddMoney() {
            var m1 = new Money(1.00000001m, "BTC");
            var m2 = new Money(10.000000019m, "BTC");
            var result = m1 + m2;

            Assert.Equal(result, new Money(11.000000029m, "BTC"));
        }

        [Fact]
        public void MoneyAddDecimal() {
            var m1 = new Money(1.00000001m, "BTC");
            var result = m1 + 10.000000019m;

            Assert.True(result.Amount == 11.000000029m);
        }

        [Fact]
        public void MoneySubtractMoney() {
            var m1 = new Money(1.00000001m, "MOP");
            var m2 = new Money(10.000000019m, "MOP");
            var result = m2 - m1;

            Assert.Equal(result, new Money(9.000000009m, "MOP"));
        }

        [Fact]
        public void MoneySubtractDecimal() {
            var m1 = new Money(1.00000001m, "BTC");
            var result = m1 - 10.000000019m;

            Assert.True(result.Amount == -9.000000009m);
        }

        [Fact]
        public void MoneyMultiplyDecimal() {
            var m1 = new Money(1.02m, "MOP");
            var result = m1 * 2.5m;

            Assert.Equal(result, new Money(2.55m, "MOP"));
        }

        [Fact]
        public void MoneyMultiplyInt() {
            var m1 = new Money(1.000000014m, "BTC");
            var result = m1 * 2;

            Assert.True(result.Amount == 2.000000028m);
        }

        [Fact]
        public void MoneyMultiplySmallDecimal() {
            var m1 = new Money(1.000000005m, "BTC");
            var result = m1 * (5m / 1000m);

            Assert.Equal(result, new Money(0.005000000025m, "BTC"));
        }

        [Fact]
        public void MoneyDividedByInt() {
            var m1 = new Money(2.5m, "BTC");
            var result = m1 / 2;

            Assert.Equal(result, new Money(1.25m, "BTC"));
        }

        [Fact]
        public void AddCurrencyTest() {
            CurrencyTypeRepository.Register("QQQ", true, "QCurrency", "q");

            var m1 = new Money(2.5m, "QQQ");
            var m2 = new Money(2.5m, "QQQ");

            Assert.Equal(new Money(5m, "QQQ"), m1 + m2);

            var cur1 = Currency.Get("QQQ");
            var cur2 = Currency.Get("USD");

            Assert.NotEqual(cur1, cur2);
        }
    }
}
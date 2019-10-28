using System;
using System.Collections;
using System.Collections.Generic;

namespace DotMoney {
    public class Currency : IEquatable<Currency> {
        public string IsoCode { get; }
        public readonly bool IsDigital;
        public readonly string GeneralName;
        public readonly string Symbol;
        public readonly int DecimalPlace;
        //public readonly int BaseDecimalPlace;
        public readonly string DecimalMark;
        public readonly string ThousandMark;
        public readonly Dictionary<string, CurrencySubType> SubTypes;
        public CurrencySubType DisplayingSubType { get; private set; }

        public static Currency Get(string isoCode) => CurrencyTypeRepository.Get(isoCode.ToUpper());

        internal Currency(string isoCode) {
            if (!CurrencyTypeRepository.Exists(isoCode))
                throw new ArgumentException($"ISO Currency Code ({isoCode}) is not registered. Invoke the CurrencyTypeRepository.Register() to register.");

            var newCurrency = CurrencyTypeRepository.Get(isoCode);
            IsoCode = newCurrency.IsoCode;
            IsDigital = newCurrency.IsDigital;
            GeneralName = newCurrency.GeneralName;
            Symbol = newCurrency.Symbol;
            DecimalPlace = newCurrency.DecimalPlace;
            //BaseDecimalPlace = newCurrency.BaseDecimalPlace;
            DecimalMark = newCurrency.DecimalMark;
            ThousandMark = newCurrency.ThousandMark;
        }

        internal Currency(string isoCode, bool isDigital, string generalName, string symbol, int decimalPlace, string decimalMark, string thousandMark) {
            IsoCode = isoCode;
            IsDigital = isDigital;
            GeneralName = generalName;
            Symbol = symbol;
            DecimalPlace = decimalPlace;
            //BaseDecimalPlace = baseDecimalPlace;
            DecimalMark = decimalMark;
            ThousandMark = thousandMark;
        }

        internal Currency(string isoCode, bool isDigital, string generalName, string symbol, int decimalPlace, string decimalMark, string thousandMark, Dictionary<string, CurrencySubType> subtypes) {
            IsoCode = isoCode;
            IsDigital = isDigital;
            GeneralName = generalName;
            Symbol = symbol;
            DecimalPlace = decimalPlace;
            //BaseDecimalPlace = baseDecimalPlace;
            DecimalMark = decimalMark;
            ThousandMark = thousandMark;
            SubTypes = subtypes;
        }

        #region Equals and !Equals

        public bool Equals(Currency other) {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsoCode == other.IsoCode;
        }

        public override bool Equals(object obj) {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Currency)) return false;
            return Equals((Currency)obj);
        }

        public override int GetHashCode() {
            return IsoCode.GetHashCode();
        }

        public static bool operator ==(Currency left, Currency right) {
            return Equals(left, right);
        }

        public static bool operator !=(Currency left, Currency right) {
            return !Equals(left, right);
        }

        #endregion

        public CurrencySubType GetSubType(string key) {
            if (SubTypes != null) {
                if (SubTypes.ContainsKey(key))
                    return SubTypes[key];
            }

            return null;
        }

        public void DisplayAsSubType(string key) {
            DisplayingSubType = GetSubType(key);
        }

        public string GetStringFormat() {
            string decimalZero = "";
            for (int i = 1; i <= this.DecimalPlace; i++) {
                decimalZero += "0";
            }
            string specifier = "#" + this.ThousandMark + "0" + this.DecimalMark + decimalZero + ";(#,0." + decimalZero + ")";
            return specifier;
        }

        public override string ToString() {
            return IsoCode + Symbol;
        }

        /// <summary>
        /// Display any passed in Money object decorated by its own Currency object
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public string ToString(Money m) {
            string displaySymbol = m.Currency.Symbol;
            decimal displayAmount = m.Amount;

            if (m.Currency.DisplayingSubType != null) {
                displaySymbol = m.Currency.DisplayingSubType.Symbol;
                displayAmount = m.Currency.DisplayingSubType.ScaleToUnit * m.Amount;
            }

            return displaySymbol + displayAmount.ToString(GetStringFormat());
        }
    }


    public class CurrencySubType {
        public string Symbol { get; set; }
        public int ScaleToUnit { get; set; }
    }


    public static class CurrencyTypeRepository {
        // List of all currencies with their properties.
        public static IReadOnlyDictionary<string, Currency> Currencies => currencies;

        private static Dictionary<string, Currency> currencies =
            new Dictionary<string, Currency>()
            {
            {"BTC", new Currency("BTC", true, "Bitcoin", "฿",                   8, ".", "," ,
                new Dictionary<string, CurrencySubType>(){
                    {"mBTC", new CurrencySubType{Symbol="mBTC ", ScaleToUnit=1000}}
                }
            )},
            {"LTC", new Currency("LTC", true, "Litecoin", "L",                  8, ".", "," ,
                new Dictionary<string, CurrencySubType>(){
                    {"mLTC", new CurrencySubType{Symbol="mLTC ", ScaleToUnit=1000}}
                }
            )},
            {"AUD", new Currency("AUD", false, "Australian dollar", "$",        2, ".", ",")},
            {"CAD", new Currency("CAD", false, "Canadian dollar", "$",          2, ".", ",")},
            {"CNY", new Currency("CNY", false, "Renminbi", "¥",                 2, ".", ",")},
            {"EUR", new Currency("EUR", false, "Euro", "€" ,                    2, ".", ",")},
            {"GBP", new Currency("GBP", false, "Pound sterling", "£",           2, ".", ",")},
            {"HKD", new Currency("HKD", false, "Hong Kong dollar", "HKD$",      2, ".", ",")},
            {"IDR", new Currency("IDR", false, "Indonesian rupiah", "Rp",       2, ".", ",")},
            {"INR", new Currency("INR", false, "Indian rupee", "Rs",            2, ".", ",")},
            {"JPY", new Currency("JPY", false, "Japanese yen", "¥",             0, ".", ",")},
            {"KRW", new Currency("KRW", false, "South Korean won", "₩",         0, ".", ",")},
            {"MOP", new Currency("MOP", false, "Pataca", "MOP$",                2, ".", ",")},
            {"NZD", new Currency("NZD", false, "New Zealand dollar", "$",       2, ".", ",")},
            {"PHP", new Currency("PHP", false, "Philippine peso", "P",          2, ".", ",")},
            {"RUB", new Currency("RUB", false, "Russian ruble", "₽",            2, ".", ",")},
            {"SGD", new Currency("SGD", false, "Singapore dollar", "S$",        2, ".", ",")},
            {"TWD", new Currency("TWD", false, "New Taiwan dollar", "$",        2, ".", ",")},
            {"USD", new Currency("USD", false, "US dollar", "$",                2, ".", ",")},
            {"VND", new Currency("VND", false, "Vietnamese dong", "₫",          2, ".", ",")},
            {"ZAR", new Currency("ZAR", false, "South African rand", "R",       2, ".", ",")}
            };

        public static Currency Get(string isoCode) {
            if (Currencies.ContainsKey(isoCode)) {
                return Currencies[isoCode];
            } else {
                return null;
            }
        }

        public static bool Exists(string isoCode) {
            return Currencies.ContainsKey(isoCode);
        }

        public static void Register(string isoCode, bool isDigital, string generalName, string symbol, int decimalPlace = 2, string decimalMark = ".", string thousandMark = ",", Dictionary<string, CurrencySubType> subtypes = null) {
            currencies.Add(isoCode, new Currency(isoCode, isDigital, generalName, symbol, decimalPlace, decimalMark, thousandMark, subtypes));
        }
    }
}
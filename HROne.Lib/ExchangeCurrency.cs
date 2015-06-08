using System;
using System.Data;
using System.Configuration;


namespace HROne.Lib
{
    /// <summary>
    /// Summary description for ExchangeCurrency
    /// </summary>
    public abstract class ExchangeCurrency
    {
        public static string DefaultCurrency()
        {
            return "HKD";
        }

        public static double ExchangeRate(string CurrencyFrom)
        {
            return ExchangeRate(CurrencyFrom, DefaultCurrency());
        }

        public static double ExchangeRate(string CurrencyFrom, string CurrencyTo)
        {
            return CurrencyRatio(CurrencyTo) / CurrencyRatio(CurrencyFrom);
        }

        public static double Exchange(double amount, string CurrencyFrom, bool RoundResult)
        {
            return Exchange(amount, CurrencyFrom, DefaultCurrency(), RoundResult);
        }

        public static double Exchange(double amount, string CurrencyFrom, string CurrenctTo, bool RoundResult)
        {
            double result = amount * ExchangeRate(CurrencyFrom, CurrenctTo);
            if (RoundResult)
                result = Math.Round(amount, CurrencyDecimalPlaces(CurrenctTo), MidpointRounding.AwayFromZero);
            return result;
        }

        public static double CurrencyRatio(string Currency)
        {
            if (string.IsNullOrEmpty(Currency))
                return 1;
            else if (Currency.Equals("HKD"))
                return 1;
            else if (Currency.Equals("USD"))
                return 7.8;
            else
                return 1;
        }

        public static int CurrencyDecimalPlaces(string Currency)
        {
            return 2;
        }

        public static int DefaultCurrencyDecimalPlaces()
        {
            return CurrencyDecimalPlaces(DefaultCurrency());
        }

    }
}
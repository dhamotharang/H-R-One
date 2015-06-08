using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.CommonLib
{
    public class GenericRoundingFunctions
    {
        /**
         * Rounding a number larger than the original number
         * e.g. 0.1 round up to 1, -0.1 round up to 0
         **/
        public static double RoundingUp(double amount, int decimalPlace, int defaultDecimalPlace)
        {
            //  Tested Number
            //  -   6049.30
            int multipilier = (int)Math.Pow(10, decimalPlace);
            amount *= multipilier;
            amount = Math.Round(amount, 2 + defaultDecimalPlace - decimalPlace, MidpointRounding.AwayFromZero);  //  DO NOT DELETE!! To prevent the truncation error after multipler.
            amount = Math.Ceiling(amount);
            return Math.Round(amount / multipilier, defaultDecimalPlace, MidpointRounding.AwayFromZero);
        }
        public static double RoundingUp(double amount, int decimalPlace, int defaultDecimalPlace, bool AbsoluteValueOnly)
        {
            if (amount > 0 || !AbsoluteValueOnly)
                return RoundingUp(amount, decimalPlace, defaultDecimalPlace);
            else
                return -RoundingUp(Math.Abs(amount), decimalPlace, defaultDecimalPlace);
        }
        /**
         * Rounding a number smaller than the original number
         * e.g. 0.1 round up to 1, -0.1 round up to 0
         **/
        public static double RoundingDown(double amount, int decimalPlace, int defaultDecimalPlace)
        {
            int multipilier = (int)Math.Pow(10, decimalPlace);
            amount *= multipilier;
            amount = Math.Round(amount, 2 + defaultDecimalPlace - decimalPlace, MidpointRounding.AwayFromZero);  //  DO NOT DELETE!! To prevent the truncation error after multipler.
            amount = Math.Floor(amount);
            return Math.Round(amount / multipilier, defaultDecimalPlace, MidpointRounding.AwayFromZero);
        }
        public static double RoundingDown(double amount, int decimalPlace, int defaultDecimalPlace, bool AbsoluteValueOnly)
        {
            if (amount > 0 || !AbsoluteValueOnly)
                return RoundingDown(amount, decimalPlace, defaultDecimalPlace);
            else
                return -RoundingDown(Math.Abs(amount), decimalPlace, defaultDecimalPlace);
        }
        public static double RoundingTo(double amount, int decimalPlace, int defaultDecimalPlace)
        {
            int multipilier = (int)Math.Pow(10, decimalPlace);
            amount *= multipilier;
            amount = Math.Round(amount, 2 + defaultDecimalPlace - decimalPlace, MidpointRounding.AwayFromZero);  //  DO NOT DELETE!! To prevent the truncation error after multipler.
            amount = Math.Round(amount, 0, MidpointRounding.AwayFromZero);
            return Math.Round(amount / multipilier, defaultDecimalPlace, MidpointRounding.AwayFromZero);
        }
    }
}

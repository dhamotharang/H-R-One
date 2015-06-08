using System;
using System.Collections.Generic;
using System.Text;

namespace HROne.CommonLib
{
    public class IntegerRoundingFunctions
    {
        public static int RoundingUp(int amount, int unit)
        {
            if (unit.Equals(0))
                return amount;
            int quotient = amount / unit;
            int remainder = amount % unit;
            if (remainder != 0)
                quotient++;
            return quotient * unit;
        }
        public static int RoundingDown(int amount, int unit)
        {
            if (unit.Equals(0))
                return amount;
            int quotient = amount / unit;
            int remainder = amount % unit;
            return quotient * unit;
        }
        public static int RoundingTo(int amount, int unit)
        {
            if (unit.Equals(0))
                return amount;
            int quotient = amount / unit;
            int remainder = amount % unit;
            if (remainder * 2 >= unit)
                quotient++;
            return quotient * unit;
        }
    }
}

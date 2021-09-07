using System.Collections.Generic;

namespace RandomVariable
{
    public static class CharExtension
    {
        public static bool IsMathOperator(this char symbol)
        {
            return symbol.Equals('+')
                   || symbol.Equals('-')
                   || symbol.Equals('/')
                   || symbol.Equals('*')
                   || symbol.Equals('d'); 
        }
    }
}
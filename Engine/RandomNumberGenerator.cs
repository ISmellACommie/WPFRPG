using System;
using System.Security.Cryptography;

namespace Engine
{
    public static class RandomNumberGenerator
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int NumberBetween(int minvalue, int maxvalue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            /*
             * We are using Math.Max, and subtracting 0.00000000001,
             * to ensure that "multiplier" will always be between 0.0 and 0.99999999999
             * Otherwise it is possible for it to be 1 which causes problems when rounding
             */
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            //We need to add one to the range to allow for the rounding done with Math.Floor
            int range = maxvalue - minvalue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minvalue + randomValueInRange);
        }
        
        //less random version below, rename to number between to use
        private static readonly Random _simpleGenerator = new Random();

        public static int SimpleNumberBetween(int minvalue, int maxvalue)
        {
            return _simpleGenerator.Next(minvalue, maxvalue + 1);
        }
    }
}

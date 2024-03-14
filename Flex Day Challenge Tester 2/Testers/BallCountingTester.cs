using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    // flag: 24d9ebf4ca86fef131adc8a82f89aa2d40778e0ae77c28aa05b42ab9d0624592
    public class BallCountingTester : Tester<BigInteger, BigInteger>
    {
        public override string TestName => "Ball Counting";

        public override IList<BigInteger> GetTests()
        {
            return new List<BigInteger>
            {
                Parse("1234567890"),

                // Keyboard mash
                Parse("9283702370498570293847901283740918726405198274902340697309485"),

                Parse("643"),
                Parse("1"),
                Parse("2"),
                Parse("10"),
                Parse("100"),
                Parse("1000000"),

                // this is the largest known prime number under 10^100
                Parse("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999203"),

                Parse("7777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777"),

                // for those pesky off-by-one errors
                Parse("63"),
                Parse("64"),
                Parse("65"),

                Parse("8768273472908357293874298374234"),
                Parse("21384127024985298342"),
                Parse("230842918347029385932480398475209834029378402835"),

                // courtesy of ChatGPT:
                Parse("101413219619834264053479185829588805881"),
                Parse("172879912157672429583964377679080067163"),
                Parse("186616899091973563190865287174370987071"),
                Parse("294691313615632948141830274551297102541"),
                Parse("456980680067899531849897129933731522373"),
                Parse("523949764549800646991821951449662707823"),
                Parse("645274729525781144685077753344685612719"),
                Parse("683438240207152868139239940868778979787"),
                Parse("759685016175601719372003602993835052107"),
                Parse("935546924248840324261199709362625019091"),
            };
        }

        protected static BigInteger Parse(string text)
        {
            return BigInteger.Parse(text);
        }

        public override BigInteger SolutionFunction(BigInteger input)
        {
            return input - GetSquareRoot(input);
        }

        private static BigInteger GetSquareRoot(BigInteger num)
        {
            BigInteger lowBoundary = 1;
            BigInteger highBoundary = 2;

            // double both boundaries until they encompass the target
            while (BigInteger.Pow(highBoundary, 2) < num)
            {
                lowBoundary <<= 1;
                highBoundary <<= 1;
            }

            BigInteger mid;

            while (lowBoundary < highBoundary)
            {
                mid = (lowBoundary + highBoundary) >> 1;
                BigInteger midSquared = BigInteger.Pow(mid, 2);

                if (midSquared == num)
                {
                    return mid;
                }
                else if (midSquared > num)
                {
                    highBoundary = mid - 1;
                }
                else
                {
                    lowBoundary = mid + 1;
                }
            }

            if (BigInteger.Pow(highBoundary, 2) > num)
            {
                return highBoundary - 1;
            }

            return highBoundary;
        }

    }

    public class BallCountingTesterEasy : BallCountingTester
    {
        // flag: 48b7ca5c188b18679814861a61fbe40f053fb410d3f9c2f73105dc4eea947826
        public override string TestName => base.TestName + ", EASY MODE";
        public override IList<BigInteger> GetTests()
        {
            return new List<BigInteger>()
            {
                56,
                63,
                64,
                65,
                144,

                256,
                512,
                195,
                1234,
                12,

                333,
                444,
                555,
                666,
                777,

                1,
                10,
                100,
                1000,
                10000
            };
        }
    }
}

// input num 1e100:

// O(n) solution
// check every number
// ~1e100 operations

// O(sqrt(n)) solution
// count the squares only
// ~1e50 operations

// O(log(n)) solution
// just figure out the sqrt of n and subtract
// ~100 operations

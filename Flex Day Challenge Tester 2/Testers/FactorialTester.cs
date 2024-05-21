using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    public class FactorialTester : Tester<int, int>
    {
        public override string TestName => "Factorial Calculator";

        public override IList<int> GetTests()
        {
            return new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
            };
        }

        public override int SolutionFunction(int input)
        {
            int result = 1;
            for (int i = input; i >= 2; i--)
            {
                result *= i;
            }
            return result;
        }
    }
}

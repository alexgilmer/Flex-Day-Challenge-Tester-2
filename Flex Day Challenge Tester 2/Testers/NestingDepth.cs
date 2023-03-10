using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    internal sealed class NestingDepthTester : Tester<string, string>
    {
        protected override string TestName => "Nesting Depth";
        protected override IList<string> GetTests()
        {
            string test0 = "0000";
            string test1 = "101";
            string test2 = "111000";
            string test3 = "1";
            string test4 = "123456789";
            string test5 = "1222333445233014";
            string test6 = "4";
            string test7 = "3141592653589793238462643383279502884197169399078";
            string test8 = "2718281828";
            string test9 = "0000111122223333444455556666777788889999888877776666555544443333222211110000";
            string test10 = "5555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555";
            string test11 = "131131131131131131131";
            string test12 = "0123456789";
            string test13 = "123456789";
            string test14 = "01234567890";
            string test15 = "1234567890";
            string test16 = "987654321";
            string test17 = "9876543210";
            string test18 = "0987654321";
            string test19 = "09876543210";
            string test20 = "9090909090909090909090909090909090909090909090909090909090909090909090909090909090909090909090909090";

            List<string> tests = new()
            {
                test0, test1, test2, test3, test4,
                test5, test6, test7, test8, test9,
                test10, test11, test12, test13, test14,
                test15, test16, test17, test18, test19,
                test20
            };

            return tests;
        }

        protected override string SolutionFunction(string input)
        {
            int curDepth = 0;
            StringBuilder result = new();

            for (int i = 0; i < input.Length; i++)
            {
                int thisDigit = int.Parse(input[i].ToString());

                while (curDepth < thisDigit)
                {
                    result.Append('(');
                    curDepth++;
                }

                while (curDepth > thisDigit)
                {
                    result.Append(')');
                    curDepth--;
                }

                result.Append(thisDigit);
            }

            while (curDepth > 0)
            {
                result.Append(')');
                curDepth--;
            }

            return result.ToString();
        }
    }
}

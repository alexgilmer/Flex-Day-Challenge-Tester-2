using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    // Flag: 0e6f78728493d1f85cef0ecc262e4aa
    public sealed class AppendSortTester : Tester<IList<int>, int>
    {
        public override string TestName => "Append Sort";

        public override IList<IList<int>> GetTests()
        {
            List<IList<int>> tests = new();

            // test names are zero-indexed for easier finding, given program output.

            List<int> test0 = new() { 1299, 12, 12 };
            tests.Add(test0);

            List<int> test1 = new() { 1, 2, 3, 4, 5, 6, 7 };
            tests.Add(test1);

            List<int> test2 = new();
            for (int i = 0; i < 10000; i++)
            {
                test2.Add(1);
            }
            tests.Add(test2);

            List<int> test3 = new() { 10, 4, 3, 11, 15, 22 };
            tests.Add(test3);

            List<int> test4 = new() { int.MaxValue, 1, 1, 1, 1, 1, 1, 1 };
            tests.Add(test4);

            List<int> test5 = new() { 100, 10, 1, 50, 24, 297 };
            tests.Add(test5);

            List<int> test6 = new() { 297, 29, 2, 2, 2, 2 };
            tests.Add(test6);

            List<int> test7 = new() { 7, 6, 5, 4, 3, 2, 1 };
            tests.Add(test7);

            List<int> test8 = new();
            for (int i = 1; i <= 100; i++)
            {
                test8.Add((int)Math.Pow(i, 2));
            }
            tests.Add(test8);

            return tests;
        }

        public override int SolutionFunction(IList<int> input)
        {
            List<BigInteger> alteredNums = new() { input[0] };

            int result = 0;
            for (int i = 1; i < input.Count; i++)
            {
                if (input[i] > alteredNums[i - 1])
                {
                    // no need to alter a number that's already bigger
                    alteredNums.Add(input[i]);
                }
                else if (input[i] == alteredNums[i - 1])
                {
                    // if the numbers perfectly match, add a single zero
                    result++;
                    alteredNums.Add(BigInteger.Parse(input[i].ToString() + "0"));
                }
                else
                {
                    string prevNumString = alteredNums[i - 1].ToString();
                    string curNumString = input[i].ToString();

                    if (prevNumString.StartsWith(curNumString))
                    {
                        // 12345, 123 => 12345, 12346
                        // 100, 1 => 100, 101
                        // 1299, 12 => 1299, 12000
                        // match previous number +1, if possible
                        // otherwise all zeroes till bigger
                        string chunkToOvercome = prevNumString[curNumString.Length..];
                        string extraPortion = (BigInteger.Parse(chunkToOvercome) + 1)
                            .ToString()
                            .PadLeft(chunkToOvercome.Length, '0');

                        if (extraPortion.Length > chunkToOvercome.Length)
                        {
                            // we don't want 1299, 12 => 1299, 12100
                            // instead change to 1299, 12 => 1299, 12000
                            extraPortion = new('0', extraPortion.Length);
                        }
                        result += extraPortion.Length;
                        alteredNums.Add(BigInteger.Parse(curNumString + extraPortion));
                    }
                    else
                    {
                        // 12345, 14 => 12345, 14000
                        // 12345, 41 => 12345, 41000
                        // 12345, 11 => 12345, 110000
                        // all zeroes until bigger
                        BigInteger newNum = input[i];
                        while (newNum < alteredNums[i - 1])
                        {
                            newNum *= 10;
                            result++;
                        }
                        alteredNums.Add(newNum);
                    }
                }
            }

            // Uncomment this to see the resulting list
            //Console.WriteLine("Alex code list output:");
            //foreach(var item in alteredNums)
            //{
            //    Console.WriteLine(item);
            //}
            return result;
        }

        public override string GetInputString(IList<int> input)
        {
            return $"[ {string.Join(", ", input)} ]";
        }
    }
}

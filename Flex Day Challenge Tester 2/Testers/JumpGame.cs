using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    internal class JumpGameTester : Tester<int[], bool>
    {
        protected override string TestName => "Jump Game";

        protected override IList<int[]> GetTests()
        {
            IList<int[]> tests = new List<int[]>();
            int[] test0 = new int[] { 1, 2, 3 };
            tests.Add(test0);

            int[] test1 = new int[] { 0, 1 };
            tests.Add(test1);

            int[] test2 = new int[] { 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            tests.Add(test2);

            int[] test3 = new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 1 };
            tests.Add(test3);

            int[] test4 = new int[] { 3, 2, 1, 0, 4 };
            tests.Add(test4);

            int[] test5 = new int[] { 0 };
            tests.Add(test5);

            int[] test6 = new int[10000];
            Array.Fill(test6, 100_000);
            tests.Add(test6);

            int[] test7 = new int[10000];
            Array.Fill(test7, 1);
            tests.Add(test7);

            int[] test8 = new int[10000];
            Array.Fill(test8, 0);
            tests.Add(test8);

            int[] test9 = new int[10000];
            Array.Fill(test9, 0);
            test9[9998] = 100;
            tests.Add(test9);

            int[] test10 = new int[] { 2, 3, 1, 1, 4 };
            tests.Add(test10);

            int[] test11 = new int[] { 3, 1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 4 };
            tests.Add(test11);

            int[] test12 = new int[] { 1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3, 4 };
            tests.Add(test12);

            return tests;
        }

        protected override bool SolutionFunction(int[] input)
        {
            if (input.Length == 1)
            {
                return true;
            }

            bool[] canJump = new bool[input.Length];
            canJump[0] = true;

            for (int i = 0; i < input.Length; i++)
            {
                if (!canJump[i])
                {
                    continue;
                }

                int jumpPower = input[i];
                for (int j = i + 1; j <= i + jumpPower && j < input.Length; j++)
                {
                    if (j == input.Length - 1)
                    {
                        return true;
                    }
                    canJump[j] = true;
                }
            }

            return false;
        }
    }
}

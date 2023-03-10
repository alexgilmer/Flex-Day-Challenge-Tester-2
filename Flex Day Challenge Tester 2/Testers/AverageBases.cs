using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    internal class AverageBasesTester : Tester<int, string>
    {
        protected override string TestName => "Average Bases";

        protected override IList<int> GetTests()
        {
            return new List<int>
            {
                5, 3, 123, 777, 1000,
                902, 411, 333, 420, 69,
                14, 22, 64, 362, 722
            };
        }

        protected override string SolutionFunction(int input)
        {
            int digitSum = 0;
            int baseCount = 0;

            for (int i = 2; i < input; i++)
            {
                baseCount++;

                IList<int> digits = GetNumberInBase(input, i);
                foreach (int digit in digits)
                {
                    digitSum += digit;
                }
                Console.WriteLine();
            }

            int gcf = GetEuclideanGCF(digitSum, baseCount);
            return $"{digitSum / gcf}/{baseCount / gcf}";
        }

        private IList<int> GetNumberInBase(int num, int @base)
        {
            List<int> result = new();

            while (num > 0)
            {
                int digit = num % @base;
                result.Add(digit);

                num -= digit;
                num /= @base;
            }

            return result;
        }

        private int GetEuclideanGCF(int num1, int num2)
        {
            if (num2 == 0)
                return num1;

            return GetEuclideanGCF(num2, num1 % num2);
        }
    }
}

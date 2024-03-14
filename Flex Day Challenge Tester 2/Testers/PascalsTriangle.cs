using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    public class PascalsTriangleTester : Tester<int, IList<IList<int>>>
    {
        public override string TestName => "Pascal's Triangle";

        public override IList<int> GetTests()
        {
            return new List<int>()
            {
                1,
                2,
                3,
                10,
                25,
                100,
                250,
                1000
            };
        }

        public override IList<IList<int>> SolutionFunction(int input)
        {
            List<IList<int>> result = new()
            {
                new List<int> { 1 }
            };

            while (result.Count < input)
            {
                result.Add(GetNextRow(result[^1]));
            }

            return result;
        }

        private IList<int> GetNextRow(IList<int> row)
        {
            List<int> result = new();
            for (int i = 0; i <= row.Count; i++)
            {
                if (i == 0 || i == row.Count)
                {
                    result.Add(1);
                }
                else
                {
                    result.Add(row[i - 1] + row[i]);
                }
            }
            return result;
        }
    }
}

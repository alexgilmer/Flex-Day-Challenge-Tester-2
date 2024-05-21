using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    public class CalculatorTester : Tester<string, decimal>
    {
        public override string TestName => "Calculator Test";

        public override IList<string> GetTests()
        {
            return new List<string>()
            {
                "4+6",
                "17+1",
                "9+0",
                "8.5+1.1",
                "1.1+1.1",
                "2.0+9.9",
                "999+1",

                "4-22",
                "12-5",
                "1.4-0.9",
                "2-0",
                "123-59",
                "14.44-12.57",
                "7-7.00",

                "1*1",
                "2*2",
                "4*9",
                "100*0",
                "1.1*1.111",
                "5.5*0.1",
                "999*999",

                "999/999",
                "999/909",
                "14/2",
                "5/7",
                "0/1",
                "1/0.001",
                "4/100000"
            };
        }

        public override decimal SolutionFunction(string input)
        {
            if (input.Contains('+'))
            {
                string[] nums = input.Split('+');
                decimal num1 = decimal.Parse(nums[0]);
                decimal num2 = decimal.Parse(nums[1]);
                return num1 + num2;
            }

            if (input.Contains('-'))
            {
                string[] nums = input.Split('-');
                decimal num1 = decimal.Parse(nums[0]);
                decimal num2 = decimal.Parse(nums[1]);
                return num1 - num2;
            }

            if (input.Contains('*'))
            {
                string[] nums = input.Split('*');
                decimal num1 = decimal.Parse(nums[0]);
                decimal num2 = decimal.Parse(nums[1]);
                return num1 * num2;
            }

            if (input.Contains('/'))
            {
                string[] nums = input.Split('/');
                decimal num1 = decimal.Parse(nums[0]);
                decimal num2 = decimal.Parse(nums[1]);
                return num1 / num2;
            }

            throw new Exception("No operator found");
        }
    }
}

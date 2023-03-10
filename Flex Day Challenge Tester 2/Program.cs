using Flex_Day_Challenge_Tester_2.Testers;
using System;
using System.Text;
namespace Flex_Day_Challenge_Tester_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tester = new JustificationTester();

            tester.RunTests(StudentFunction);
        }

        static string[] StudentFunction(string test, int num)
        {
            return new string[] { "", "", "" };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    // Flag: 366a2221b50695e34be4f41ea517254a22a
    public class PhoneWordsTester : Tester<string, IList<string>>
    {
        public override string TestName => "Phone Words";

        public override IList<string> GetTests()
        {
            return new List<string>
            {
                "23", "", "2", "97", "444",
                "2345", "9999", "888", "867", "4",
                "499", "66", "33", "54", "82"
            };
        }

        protected override IList<string> SolutionFunction(string input)
        {
            return Solve(input, "");
        }

        private readonly Dictionary<char, string> PhoneLetters = new()
        {
            { '2', "abc" },
            { '3', "def" },
            { '4', "ghi" },
            { '5', "jkl" },
            { '6', "mno" },
            { '7', "pqrs" },
            { '8', "tuv" },
            { '9', "wxyz" }
        };

        private IList<string> Solve(string input = "", string curLetters = "")
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(input))
            {
                return result;
            }

            char digit = input[0];

            if (input.Length == 1)
            {
                foreach (char letter in PhoneLetters[digit])
                {
                    result.Add($"{curLetters}{letter}");
                }
                return result;
            }
            else
            {
                foreach (char letter in PhoneLetters[digit])
                {
                    result.AddRange(Solve(input[1..], $"{curLetters}{letter}"));
                }
                return result;
            }
        }

        public override string GetOutputString(IList<string> output)
        {
            StringBuilder result = new("[ ");
            foreach (string word in output)
            {
                result.Append($"{word} ");
            }
            result.Append(']');
            return result.ToString();
        }

        public override bool SolutionsMatch(IList<string> s1, IList<string> s2)
        {
            HashSet<string> h1 = new(s1);

            if (s1.Count() != s2.Count())
                return false;

            foreach (string word in s2)
            {
                if (!h1.Contains(word))
                    return false;
            }

            return true;
        }
    }
}

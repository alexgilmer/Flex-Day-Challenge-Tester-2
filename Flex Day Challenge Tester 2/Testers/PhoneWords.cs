﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    internal class PhoneWordsTester : Tester<string, IList<string>>
    {
        protected override string TestName => "Phone Words";

        protected override IList<string> GetTests()
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

        private Dictionary<int, string> PhoneLetters = new()
        {
            { 2, "abc" },
            { 3, "def" },
            { 4, "ghi" },
            { 5, "jkl" },
            { 6, "mno" },
            { 7, "pqrs" },
            { 8, "tuv" },
            { 9, "wxyz" }
        };

        private IList<string> Solve(string input = "", string curLetters = "")
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(input))
            {
                return result;
            }

            int digit = int.Parse(input[0].ToString());

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

        protected override string GetOutputString(IList<string> output)
        {
            StringBuilder result = new("[ ");
            foreach (string word in output)
            {
                result.Append($"{word} ");
            }
            result.Append(']');
            return result.ToString();
        }

        protected override bool SolutionsMatch(IList<string> s1, IList<string> s2)
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
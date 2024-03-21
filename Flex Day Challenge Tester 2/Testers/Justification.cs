using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    // Flag: 042d4d5753d55d9c1064ca90f766ded9e1
    public class JustificationTester : Tester<string, int, IList<string>>
    {
        public override string TestName => "String Justifier";

        public override IList<Tuple<string, int>> GetTests()
        {
            IList<Tuple<string, int>> tests = new List<Tuple<string, int>>();

            tests.Add(new("This is an example of text justification.", 16));
            tests.Add(new("What must be acknowledgment shall be", 16));
            tests.Add(new("Science is what we understand well enough to explain to a computer. Art is everything else we do", 20));
            tests.Add(new("What can I do now? This is a dumb idea", 4));
            tests.Add(new(
                "According to all known laws of aviation, there is no way a bee should be able to fly. " +
                "Its wings are too small to get its fat little body off the ground. " +
                "The bee, of course, flies anyway because bees don't care what humans think is impossible. " +
                "Yellow, black. Yellow, black. Yellow, black. Yellow, black. Ooh, black and yellow! " +
                "Let's shake it up a little. Barry!", 60));

            tests.Add(new("Have you ever been so far even as decided to do want look more like?", 10));
            tests.Add(new("What is good in life? To crush your enemies, to see them driven before you, and to hear the lamentations of their women.", 16));
            tests.Add(new("Straight from the horses mouth.", 16));
            tests.Add(new("Git gud scrubs!", 7));
            tests.Add(new(
                "It's supercalifragilisticexpialidocious Even though the sound of it is something quite atrocious " +
                "If you say it loud enough you'll always sound precocious Supercalifragilisticexpialidocious " +
                "Um-dittle-ittl-um-dittle-I Um-dittle-ittl-um-dittle-I Um-dittle-ittl-um-dittle-I Um-dittle-ittl-um-dittle-I " +
                "Because I was afraid to speak when I was just a lad Me father gave me nose a tweak, told me I was bad " +
                "But then one day I learned a word that saved me achin' nose The biggest word you ever heard and this is how it goes " +
                "Oh, supercalifragilisticexpialidocious Even though the sound of it is something quite atrocious " +
                "If you say it loud enough you'll always sound precocious Supercalifragilisticexpialidocious " +
                "Um-dittle-ittl-um-dittle-I Um-dittle-ittl-um-dittle-I Um-dittle-ittl-um-dittle-I Um-dittle-ittl-um-dittle-I " +
                "He traveled all around the world and everywhere he went He'd use his word and all would say there goes a clever gent " +
                "When dukes or Maharajas pass the time of day with me", 34));

            tests.Add(new(
                "Hobbiton, The Shire - A scandal has rocked the peaceful community of The Shire, following allegations of misconduct by one of its most prominent members. " +
                "Bilbo Baggins, the former ring-bearer, has been accused of stealing a valuable artifact from a fellow hobbit's home during a dinner party. " +
                "Witnesses claim that Bilbo was caught red-handed with the artifact and ejected from the party. " +
                "The victim, who wished to remain anonymous, revealed that it was a rare and valuable piece of jewelry with significant sentimental value. " +
                "Bilbo denies any wrongdoing, claiming no recollection of the events. " +
                "This incident has sent shockwaves through the tight-knit hobbit community, with many calling for Bilbo to be held accountable for his actions.", 30));

            return tests;
        }

        public override IList<string> SolutionFunction(string input1, int maxLength)
        {
            IList<string> result = new List<string>();
            string[] inputWords = input1.Split(' ');

            StringBuilder curLine = new();
            foreach (string word in inputWords)
            {
                if (curLine.Length == 0)
                {
                    curLine.Append(word);
                    continue;
                }

                if (curLine.Length + 1 + word.Length > maxLength)
                {
                    JustifyLine(curLine, maxLength);
                    result.Add(curLine.ToString());
                    curLine = new(word);
                }
                else
                {
                    curLine.Append(" " + word);
                }
            }

            if (curLine.Length > 0)
            {
                string lastLine = curLine.ToString().PadRight(maxLength, ' ');
                result.Add(lastLine);
            }

            return result;
        }

        private void JustifyLine(StringBuilder line, int maxLength)
        {
            // extract words from line
            // NOTE: do not reassign reference
            // determine quantity of spaces to add
            // redistribute spaces evenly, with leftovers going in left-to-right order
            string[] words = line.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int curChars = 0;
            foreach (string word in words)
            {
                curChars += word.Length;
            }

            int totalSpaces = maxLength - curChars;
            int wordGaps = words.Length - 1;

            if (wordGaps == 0)
            {
                while (line.Length < maxLength)
                {
                    line.Append(' ');
                }
                return;
            }

            line.Clear();

            int spaceCount = totalSpaces / wordGaps;
            int singletonCount = totalSpaces % wordGaps;

            for (int i = 0; i < words.Length - 1; i++)
            {
                line.Append(words[i]);
                for (int j = 0; j < spaceCount; j++)
                {
                    line.Append(' ');
                }

                if (i < singletonCount)
                {
                    line.Append(' ');
                }
            }

            line.Append(words[words.Length - 1]);
        }

        public override bool SolutionsMatch(IList<string> s1, IList<string> s2)
        {
            if (s1.Count != s2.Count)
            {
                return false;
            }

            for(int i = 0; i < s1.Count; i++)
            {
                if (s1[i] != s2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override string GetInputString(Tuple<string, int> input)
        {
            return $"{input.Item1}({input.Item2})";
        }

        public override string GetOutputString(IList<string> output)
        {
            StringBuilder result = new();
            foreach (string s in output)
            {
                result.AppendLine(s);
            }

            return result.ToString();
        }
    }
}

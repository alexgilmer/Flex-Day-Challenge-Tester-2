using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2
{
    internal abstract class Tester<TInputType, TOutputType>
    {
        protected abstract string TestName { get; }
        protected abstract IList<TInputType> GetTests();
        protected abstract TOutputType SolutionFunction(TInputType input);

        internal void RunTests(
            Func<TInputType, TOutputType> studentFunction,
            bool printAllResults = false,
            bool printDetailedFailures = false,
            bool breakOnTestFailure = true
        )
        {
            Console.WriteLine($"Running tests for {TestName}...");

            IList<TInputType> tests = GetTests();
            int passes = 0;
            int failures = 0;

            for (int i = 0; i < tests.Count; i++)
            {
                var test = tests[i];
                TestResult result = RunSingleTest(test, studentFunction);

                if (result.Failed)
                {
                    failures++;

                    if (printDetailedFailures || printAllResults)
                    {
                        Console.WriteLine($"Test index {i} failed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        Console.WriteLine($"Exptected output: \n{result.ExpectedOutput}");
                    }

                    if (breakOnTestFailure)
                    {
                        break;
                    }
                }
                else
                {
                    passes++;
                    if (printAllResults)
                    {
                        Console.WriteLine($"Test index {i} passed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                    }
                }
            }

            Console.WriteLine($"Final testing results: {passes} passes.  {failures} failures.");
        }

        internal TestResult RunSingleTest(TInputType test, Func<TInputType, TOutputType> studentFunction)
        {
            TOutputType solution = SolutionFunction(test);

            try
            {
                TOutputType studentResult = studentFunction(test);

                return new TestResult()
                {
                    Passed = SolutionsMatch(studentResult, solution),
                    Input = GetInputString(test),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = GetOutputString(studentResult)
                };
            }
            catch (Exception e)
            {
                return new TestResult()
                {
                    Passed = false,
                    Input = GetInputString(test),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = e.Message
                };
            }
        }

        internal void PrintSolutions()
        {
            var tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(GetInputString(tests[i]));
                Console.WriteLine(GetOutputString(SolutionFunction(tests[i])));
            }
        }

        protected virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1.Equals(es2);
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }

        protected virtual string GetInputString(TInputType input)
        {
            return input.ToString();
        }

        protected virtual string GetOutputString(TOutputType output)
        {
            return output.ToString();
        }
    }

    internal abstract class Tester<TInputType1, TInputType2, TOutputType>
    {
        protected abstract string TestName { get; }
        protected abstract IList<Tuple<TInputType1, TInputType2>> GetTests();
        protected abstract TOutputType SolutionFunction(TInputType1 input1, TInputType2 input2);
        protected virtual TOutputType SolutionFunction(Tuple<TInputType1, TInputType2> inputs)
        {
            return SolutionFunction(inputs.Item1, inputs.Item2);
        }

        internal void RunTests(
            Func<TInputType1, TInputType2, TOutputType> studentFunction,
            bool printAllResults = false,
            bool printDetailedFailures = false,
            bool breakOnTestFailure = true
        )
        {
            Console.WriteLine($"Running tests for {TestName}...");

            IList<Tuple<TInputType1, TInputType2>> tests = GetTests();
            int passes = 0;
            int failures = 0;

            for (int i = 0; i < tests.Count; i++)
            {
                var test = tests[i];
                TestResult result = RunSingleTest(test, studentFunction);

                if (result.Failed)
                {
                    failures++;

                    if (printDetailedFailures || printAllResults)
                    {
                        Console.WriteLine($"Test index {i} failed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        Console.WriteLine($"Exptected output: \n{result.ExpectedOutput}");
                    }

                    if (breakOnTestFailure)
                    {
                        break;
                    }
                }
                else
                {
                    passes++;
                    if (printAllResults)
                    {
                        Console.WriteLine($"Test index {i} passed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                    }
                }
            }

            Console.WriteLine($"Final testing results: {passes} passes.  {failures} failures.");
        }

        internal TestResult RunSingleTest(Tuple<TInputType1, TInputType2> test, Func<TInputType1, TInputType2, TOutputType> studentFunction)
        {
            TOutputType solution = SolutionFunction(test);

            try
            {
                TOutputType studentResult = studentFunction(test.Item1, test.Item2);

                return new TestResult()
                {
                    Passed = SolutionsMatch(studentResult, solution),
                    Input = GetInputString(test),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = GetOutputString(studentResult)
                };
            }
            catch (Exception e)
            {
                return new TestResult()
                {
                    Passed = false,
                    Input = GetInputString(test),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = e.Message
                };
            }
        }

        internal void PrintSolutions()
        {
            var tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(GetInputString(tests[i]));
                Console.WriteLine(GetOutputString(SolutionFunction(tests[i])));
            }
        }

        protected virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1.Equals(es2);
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }

        protected virtual string GetInputString(Tuple<TInputType1, TInputType2> input)
        {
            return input.ToString();
        }

        protected virtual string GetOutputString(TOutputType output)
        {
            return output.ToString();
        }
    }

    internal abstract class Tester<TInputType1, TInputType2, TInputType3, TOutputType>
    {
        protected abstract string TestName { get; }
        protected abstract IList<Tuple<TInputType1, TInputType2, TInputType3>> GetTests();
        protected abstract TOutputType SolutionFunction(TInputType1 input1, TInputType2 input2, TInputType3 input3);
        protected virtual TOutputType SolutionFunction(Tuple<TInputType1, TInputType2, TInputType3> inputs)
        {
            return SolutionFunction(inputs.Item1, inputs.Item2, inputs.Item3);
        }

        internal void RunTests(
            Func<TInputType1, TInputType2, TInputType3, TOutputType> studentFunction,
            bool printAllResults = false,
            bool printDetailedFailures = false,
            bool breakOnTestFailure = true
        )
        {
            Console.WriteLine($"Running tests for {TestName}...");

            IList<Tuple<TInputType1, TInputType2, TInputType3>> tests = GetTests();
            int passes = 0;
            int failures = 0;

            for (int i = 0; i < tests.Count; i++)
            {
                var test = tests[i];
                TestResult result = RunSingleTest(test, studentFunction);

                if (result.Failed)
                {
                    failures++;

                    if (printDetailedFailures || printAllResults)
                    {
                        Console.WriteLine($"Test index {i} failed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        Console.WriteLine($"Exptected output: \n{result.ExpectedOutput}");
                    }

                    if (breakOnTestFailure)
                    {
                        break;
                    }
                }
                else
                {
                    passes++;
                    if (printAllResults)
                    {
                        Console.WriteLine($"Test index {i} passed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                    }
                }
            }

            Console.WriteLine($"Final testing results: {passes} passes.  {failures} failures.");
        }

        internal TestResult RunSingleTest(Tuple<TInputType1, TInputType2, TInputType3> test, Func<TInputType1, TInputType2, TInputType3, TOutputType> studentFunction)
        {
            TOutputType solution = SolutionFunction(test);

            try
            {
                TOutputType studentResult = studentFunction(test.Item1, test.Item2, test.Item3);

                return new TestResult()
                {
                    Passed = SolutionsMatch(studentResult, solution),
                    Input = GetInputString(test),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = GetOutputString(studentResult)
                };
            }
            catch (Exception e)
            {
                return new TestResult()
                {
                    Passed = false,
                    Input = GetInputString(test),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = e.Message
                };
            }
        }

        internal void PrintSolutions()
        {
            var tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(GetInputString(tests[i]));
                Console.WriteLine(GetOutputString(SolutionFunction(tests[i])));
            }
        }

        protected virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1.Equals(es2);
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }

        protected virtual string GetInputString(Tuple<TInputType1, TInputType2, TInputType3> input)
        {
            return input.ToString();
        }

        protected virtual string GetOutputString(TOutputType output)
        {
            return output.ToString();
        }
    }

    internal class TestResult
    {
        internal bool Passed { get; init; }
        internal bool Failed { 
            get
            {
                return !Passed;
            } 
        }
        internal string Input { get; init; }
        internal string ExpectedOutput { get; init; }
        internal string ActualOutput { get; init; }
    }
}

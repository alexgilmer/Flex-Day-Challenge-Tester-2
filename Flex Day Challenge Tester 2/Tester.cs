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

        internal bool RunTests(Func<TInputType, TOutputType> studentFunction, bool printIndividualResults = false, bool printErrorMessages = false)
        {
            IList<TInputType> tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                var test = tests[i];

                try
                {
                    TOutputType solution = SolutionFunction(test);
                    TOutputType studentResult = studentFunction(test);

                    bool passed = SolutionsMatch(studentResult, solution);

                    if (printIndividualResults)
                    {
                        Console.WriteLine($"Test case: {i}.  Expected result: {solution}.  Student result: {studentResult}.  Test Passed: {passed}.");
                    }
                    if (!passed)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    if (printErrorMessages)
                    {
                        Console.WriteLine($"Error thrown on zero-indexed test case: {i}.  Error message: {e.Message}");
                    }
                    else
                    {
                        Console.WriteLine("Error thrown.");
                    }
                    return false;
                }
            }

            return true;
        }

        internal void PrintSolutions()
        {
            var tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(tests[i].ToString());
                Console.WriteLine(SolutionFunction(tests[i]).ToString());
            }
        }

        protected virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1 == es2;
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
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

        internal bool RunTests(Func<TInputType1, TInputType2, TOutputType> studentFunction, bool printIndividualResults = false, bool printErrorMessages = false)
        {
            IList<Tuple<TInputType1, TInputType2>> tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                var test = tests[i];

                try
                {
                    TOutputType solution = SolutionFunction(test);
                    TOutputType studentResult = studentFunction(test.Item1, test.Item2);

                    bool passed = SolutionsMatch(studentResult, solution);

                    if (printIndividualResults)
                    {
                        Console.WriteLine($"Test case: {i}.  Expected result: {solution}.  Student result: {studentResult}.  Test Passed: {passed}.");
                    }
                    if (!passed)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    if (printErrorMessages)
                    {
                        Console.WriteLine($"Error thrown on zero-indexed test case: {i}.  Error message: {e.Message}");
                    }
                    else
                    {
                        Console.WriteLine("Error thrown.");
                    }
                    return false;
                }
            }

            return true;
        }

        internal void PrintSolutions()
        {
            var tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(tests[i].ToString());
                Console.WriteLine(SolutionFunction(tests[i]).ToString());
            }
        }

        protected virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1 == es2;
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
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

        internal bool RunTests(Func<TInputType1, TInputType2, TInputType3, TOutputType> studentFunction, bool printIndividualResults = false, bool printErrorMessages = false)
        {
            IList<Tuple<TInputType1, TInputType2, TInputType3>> tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                var test = tests[i];

                try
                {
                    TOutputType solution = SolutionFunction(test);
                    TOutputType studentResult = studentFunction(test.Item1, test.Item2, test.Item3);

                    bool passed = SolutionsMatch(studentResult, solution);

                    if (printIndividualResults)
                    {
                        Console.WriteLine($"Test case: {i}.  Expected result: {solution}.  Student result: {studentResult}.  Test Passed: {passed}.");
                    }
                    if (!passed)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    if (printErrorMessages)
                    {
                        Console.WriteLine($"Error thrown on zero-indexed test case: {i}.  Error message: {e.Message}");
                    }
                    else
                    {
                        Console.WriteLine("Error thrown.");
                    }
                    return false;
                }
            }

            return true;
        }

        internal void PrintSolutions()
        {
            var tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(tests[i].ToString());
                Console.WriteLine(SolutionFunction(tests[i]).ToString());
            }
        }

        protected virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1 == es2;
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }
    }
}

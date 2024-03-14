using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Flex_Day_Challenge_Tester_2
{
    public abstract class Tester<TInputType, TOutputType>
    {
        public abstract string TestName { get; }
        public abstract IList<TInputType> GetTests();

        public virtual int TimeLimitMilliseconds { get; } = 1_000;

        /// <summary>
        ///   The system solution for this problem.  All child classes must implement their own solution to the problem.
        /// </summary>
        /// <param name="input">
        ///   The input for the solution function. 
        /// </param>
        /// <returns>
        ///   The output from the solution function. 
        /// </returns>
        public abstract TOutputType SolutionFunction(TInputType input);

        /// <summary>
        ///   Runs the student function through all test cases against the system solution. 
        /// </summary>
        /// <param name="studentFunction">
        ///   The function to be tested.
        /// </param>
        /// <param name="printAllResults">
        ///   Print the result of each test, instead of only the final verdict.
        /// </param>
        /// <param name="printDetailedFailures">
        ///   Print additional details when the student's code fails
        /// </param>
        /// <param name="breakOnTestFailure">
        ///   Halts testing when the student's code fails a test
        /// </param>
        /// <param name="printComputationTimes">
        ///   Prints details on computation time.  Only works if printAllResults or printDetailedFailures is true.
        /// </param>
        public void RunTests(
            Func<TInputType, TOutputType> studentFunction,
            bool printAllResults = false,
            bool printDetailedFailures = false,
            bool breakOnTestFailure = true,
            bool printComputationTimes = false
        )
        {
            Console.WriteLine($"Running tests for {TestName}...");

            // two identical copies of test data, to protect against reference-based errors
            IList<TInputType> solutionTestList = GetTests();
            IList<TInputType> studentTestList = GetTests();

            int passes = 0;
            int failures = 0;

            for (int i = 0; i < solutionTestList.Count; i++)
            {
                TInputType solutionTestData = solutionTestList[i];
                TInputType studentTestData = studentTestList[i];

                TestResult result = RunSingleTest(solutionTestData, studentTestData, studentFunction);

                if (result.Failed)
                {
                    failures++;

                    if (printDetailedFailures || printAllResults)
                    {
                        Console.WriteLine($"\nTest index {i} failed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        Console.WriteLine($"Exptected output: \n{result.ExpectedOutput}");
                        if (printComputationTimes)
                        {
                            Console.WriteLine($"System computation time: {result.SystemSolutionComputationTime}");
                            Console.WriteLine($"Student computation time: {result.StudentSolutionComputationTime}");
                        }
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
                        Console.WriteLine($"\nTest index {i} passed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        if (printComputationTimes)
                        {
                            Console.WriteLine($"System computation time: {result.SystemSolutionComputationTime}");
                            Console.WriteLine($"Student computation time: {result.StudentSolutionComputationTime}");
                        }
                    }
                }
            }

            Console.WriteLine($"\nFinal testing results: {passes} pass(es).  {failures} failure(s).");
        }

        public TestResult RunSingleTest(TInputType solutionTestData, TInputType studentTestData, Func<TInputType, TOutputType> studentFunction)
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            TOutputType solution = SolutionFunction(solutionTestData);
            stopwatch.Stop();
            long systemSolutionTimeMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            Task<TestResult> studentTask = Task.Run(() =>
            {
                try
                {
                    stopwatch.Start();
                    TOutputType studentResult = studentFunction(studentTestData);
                    stopwatch.Stop();

                    return new TestResult()
                    {
                        Passed = SolutionsMatch(studentResult, solution),
                        Input = GetInputString(solutionTestData),
                        ExpectedOutput = GetOutputString(solution),
                        ActualOutput = GetOutputString(studentResult),
                        StudentSolutionComputationTime = stopwatch.ElapsedMilliseconds,
                        SystemSolutionComputationTime = systemSolutionTimeMilliseconds
                    };
                }
                catch (Exception e)
                {
                    stopwatch.Stop();
                    return new TestResult()
                    {
                        Passed = false,
                        Input = GetInputString(solutionTestData),
                        ExpectedOutput = GetOutputString(solution),
                        ActualOutput = "Run time error: " + e.Message,
                        SystemSolutionComputationTime = systemSolutionTimeMilliseconds,
                        StudentSolutionComputationTime = stopwatch.ElapsedMilliseconds,
                    };
                }
            });
            Task<TestResult> timerTask = Task.Run(() =>
            {
                Thread.Sleep(TimeLimitMilliseconds);
                return new TestResult()
                {
                    Passed = false,
                    Input = GetInputString(solutionTestData),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = "Time Limit Exceeded",
                    SystemSolutionComputationTime = systemSolutionTimeMilliseconds,
                    StudentSolutionComputationTime = TimeLimitMilliseconds,
                };
            });

            int result = Task.WaitAny(studentTask, timerTask);
            return result switch
            {
                0 => studentTask.Result,
                1 => timerTask.Result,
                _ => throw new Exception("Task.WaitAny returned an unexpected value")
            };
        }

        public void PrintSolutions()
        {
            IList<TInputType> tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(GetInputString(tests[i]));
                var watch = new Stopwatch();
                watch.Start();
                var result = SolutionFunction(tests[i]);
                watch.Stop();
                Console.WriteLine(GetOutputString(result));
                Console.WriteLine($"Computation time: {watch.ElapsedMilliseconds}ms.\n");
            }
        }

        public virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 == null)
                return false;
            if (s2 == null)
                return false;

            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1.Equals(es2);
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }

        public virtual string GetInputString(TInputType input)
        {
            return input.ToString();
        }

        public virtual string GetOutputString(TOutputType output)
        {
            return output.ToString();
        }
    }

    public abstract class Tester<TInputType1, TInputType2, TOutputType>
    {
        public abstract string TestName { get; }
        public abstract IList<Tuple<TInputType1, TInputType2>> GetTests();

        public virtual int TimeLimitMilliseconds { get; } = 1_000;

        /// <summary>
        ///   The system solution for this problem.  All child classes must implement their own solution to the problem.
        /// </summary>
        /// <param name="input1">
        ///   The first input for the solution function. 
        /// </param>
        /// <param name="input2">
        ///   The second input for the solution function. 
        /// </param>
        /// <returns>
        ///   The output from the solution function. 
        /// </returns>
        protected abstract TOutputType SolutionFunction(TInputType1 input1, TInputType2 input2);

        /// <summary>
        ///   Runs the student function through all test cases against the system solution. 
        /// </summary>
        /// <param name="studentFunction">
        ///   The function to be tested.
        /// </param>
        /// <param name="printAllResults">
        ///   Print the result of each test, instead of only the final verdict.
        /// </param>
        /// <param name="printDetailedFailures">
        ///   Print additional details when the student's code fails
        /// </param>
        /// <param name="breakOnTestFailure">
        ///   Halts testing when the student's code fails a test
        /// </param>
        /// <param name="printComputationTimes">
        ///   Prints details on computation time.  Only works if printAllResults or printDetailedFailures is true.
        /// </param>
        public void RunTests(
            Func<TInputType1, TInputType2, TOutputType> studentFunction,
            bool printAllResults = false,
            bool printDetailedFailures = false,
            bool breakOnTestFailure = true,
            bool printComputationTimes = false
        )
        {
            Console.WriteLine($"Running tests for {TestName}...");

            // two identical copies of test data, to protect against reference-based errors
            IList<Tuple<TInputType1, TInputType2>> solutionTestList = GetTests();
            IList<Tuple<TInputType1, TInputType2>> studentTestList = GetTests();

            int passes = 0;
            int failures = 0;

            for (int i = 0; i < solutionTestList.Count; i++)
            {
                Tuple<TInputType1, TInputType2> solutionTestData = solutionTestList[i];
                Tuple<TInputType1, TInputType2> studentTestData = studentTestList[i];

                TestResult result = RunSingleTest(solutionTestData, studentTestData, studentFunction);

                if (result.Failed)
                {
                    failures++;

                    if (printDetailedFailures || printAllResults)
                    {
                        Console.WriteLine($"\nTest index {i} failed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        Console.WriteLine($"Exptected output: \n{result.ExpectedOutput}");
                        if (printComputationTimes)
                        {
                            Console.WriteLine($"System computation time: {result.SystemSolutionComputationTime}");
                            Console.WriteLine($"Student computation time: {result.StudentSolutionComputationTime}");
                        }
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
                        Console.WriteLine($"\nTest index {i} passed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        if (printComputationTimes)
                        {
                            Console.WriteLine($"System computation time: {result.SystemSolutionComputationTime}");
                            Console.WriteLine($"Student computation time: {result.StudentSolutionComputationTime}");
                        }
                    }
                }
            }

            Console.WriteLine($"\nFinal testing results: {passes} pass(es).  {failures} failure(s).");
        }

        private TestResult RunSingleTest(Tuple<TInputType1, TInputType2> solutionTestData, Tuple<TInputType1, TInputType2> studentTestData, Func<TInputType1, TInputType2, TOutputType> studentFunction)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            TOutputType solution = SolutionFunction(solutionTestData.Item1, solutionTestData.Item2);
            stopwatch.Stop();
            long systemSolutionTimeMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            Task<TestResult> studentTask = Task.Run(() =>
            {
                try
                {
                    stopwatch.Start();
                    TOutputType studentResult = studentFunction(studentTestData.Item1, studentTestData.Item2);
                    stopwatch.Stop();

                    return new TestResult()
                    {
                        Passed = SolutionsMatch(studentResult, solution),
                        Input = GetInputString(solutionTestData),
                        ExpectedOutput = GetOutputString(solution),
                        ActualOutput = GetOutputString(studentResult),
                        StudentSolutionComputationTime = stopwatch.ElapsedMilliseconds,
                        SystemSolutionComputationTime = systemSolutionTimeMilliseconds
                    };
                }
                catch (Exception e)
                {
                    stopwatch.Stop();
                    return new TestResult()
                    {
                        Passed = false,
                        Input = GetInputString(solutionTestData),
                        ExpectedOutput = GetOutputString(solution),
                        ActualOutput = "Run time error: " + e.Message,
                        SystemSolutionComputationTime = systemSolutionTimeMilliseconds,
                        StudentSolutionComputationTime = stopwatch.ElapsedMilliseconds,
                    };
                }
            });
            Task<TestResult> timerTask = Task.Run(() =>
            {
                Thread.Sleep(TimeLimitMilliseconds);
                return new TestResult()
                {
                    Passed = false,
                    Input = GetInputString(solutionTestData),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = "Time Limit Exceeded",
                    SystemSolutionComputationTime = systemSolutionTimeMilliseconds,
                    StudentSolutionComputationTime = TimeLimitMilliseconds,
                };
            });

            int result = Task.WaitAny(studentTask, timerTask);
            return result switch
            {
                0 => studentTask.Result,
                1 => timerTask.Result,
                _ => throw new Exception("Task.WaitAny returned an unexpected value")
            };
        }

        public void PrintSolutions()
        {
            IList<Tuple<TInputType1, TInputType2>> tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(GetInputString(tests[i]));
                var watch = new Stopwatch();
                watch.Start();
                var result = SolutionFunction(tests[i].Item1, tests[i].Item2);
                watch.Stop();
                Console.WriteLine(GetOutputString(result));
                Console.WriteLine($"Computation time: {watch.ElapsedMilliseconds}ms.\n");
            }
        }

        public virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 == null)
                return false;
            if (s2 == null)
                return false;

            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1.Equals(es2);
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }

        public virtual string GetInputString(Tuple<TInputType1, TInputType2> input)
        {
            return $"[{input.Item1}, {input.Item2}]";
        }

        public virtual string GetOutputString(TOutputType output)
        {
            return $"{output}";
        }
    }

    public abstract class Tester<TInputType1, TInputType2, TInputType3, TOutputType>
    {
        public abstract string TestName { get; }
        public abstract IList<Tuple<TInputType1, TInputType2, TInputType3>> GetTests();

        public virtual int TimeLimitMilliseconds { get; } = 1_000;

        /// <summary>
        ///   The system solution for this problem.  All child classes must implement their own solution to the problem.
        /// </summary>
        /// <param name="input1">
        ///   The first input for the solution function. 
        /// </param>
        /// <param name="input2">
        ///   The second input for the solution function. 
        /// </param>
        /// <param name="input3">
        ///   The third input for the solution function. 
        /// </param>
        /// <returns>
        ///   The output from the solution function. 
        /// </returns>
        protected abstract TOutputType SolutionFunction(TInputType1 input1, TInputType2 input2, TInputType3 input3);

        /// <summary>
        ///   Runs the student function through all test cases against the system solution. 
        /// </summary>
        /// <param name="studentFunction">
        ///   The function to be tested.
        /// </param>
        /// <param name="printAllResults">
        ///   Print the result of each test, instead of only the final verdict.
        /// </param>
        /// <param name="printDetailedFailures">
        ///   Print additional details when the student's code fails
        /// </param>
        /// <param name="breakOnTestFailure">
        ///   Halts testing when the student's code fails a test
        /// </param>
        /// <param name="printComputationTimes">
        ///   Prints details on computation time.  Only works if printAllResults or printDetailedFailures is true.
        /// </param>
        public void RunTests(
            Func<TInputType1, TInputType2, TInputType3, TOutputType> studentFunction,
            bool printAllResults = false,
            bool printDetailedFailures = false,
            bool breakOnTestFailure = true,
            bool printComputationTimes = false
        )
        {
            Console.WriteLine($"Running tests for {TestName}...");

            // two identical copies of test data, to protect against reference-based errors
            IList<Tuple<TInputType1, TInputType2, TInputType3>> solutionTestList = GetTests();
            IList<Tuple<TInputType1, TInputType2, TInputType3>> studentTestList = GetTests();

            int passes = 0;
            int failures = 0;

            for (int i = 0; i < solutionTestList.Count; i++)
            {
                Tuple<TInputType1, TInputType2, TInputType3> solutionTestData = solutionTestList[i];
                Tuple<TInputType1, TInputType2, TInputType3> studentTestData = studentTestList[i];

                TestResult result = RunSingleTest(solutionTestData, studentTestData, studentFunction);

                if (result.Failed)
                {
                    failures++;

                    if (printDetailedFailures || printAllResults)
                    {
                        Console.WriteLine($"\nTest index {i} failed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        Console.WriteLine($"Exptected output: \n{result.ExpectedOutput}");
                        if (printComputationTimes)
                        {
                            Console.WriteLine($"System computation time: {result.SystemSolutionComputationTime}");
                            Console.WriteLine($"Student computation time: {result.StudentSolutionComputationTime}");
                        }
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
                        Console.WriteLine($"\nTest index {i} passed.");
                        Console.WriteLine($"Input: \n{result.Input}");
                        Console.WriteLine($"Output: \n{result.ActualOutput}");
                        if (printComputationTimes)
                        {
                            Console.WriteLine($"System computation time: {result.SystemSolutionComputationTime}");
                            Console.WriteLine($"Student computation time: {result.StudentSolutionComputationTime}");
                        }
                    }
                }
            }

            Console.WriteLine($"\nFinal testing results: {passes} pass(es).  {failures} failure(s).");
        }

        private TestResult RunSingleTest(Tuple<TInputType1, TInputType2, TInputType3> solutionTestData, Tuple<TInputType1, TInputType2, TInputType3> studentTestData, Func<TInputType1, TInputType2, TInputType3, TOutputType> studentFunction)
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            TOutputType solution = SolutionFunction(solutionTestData.Item1, solutionTestData.Item2, solutionTestData.Item3);
            stopwatch.Stop();
            long systemSolutionTimeMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            Task<TestResult> studentTask = Task.Run(() =>
            {
                try
                {
                    stopwatch.Start();
                    TOutputType studentResult = studentFunction(studentTestData.Item1, studentTestData.Item2, studentTestData.Item3);
                    stopwatch.Stop();

                    return new TestResult()
                    {
                        Passed = SolutionsMatch(studentResult, solution),
                        Input = GetInputString(solutionTestData),
                        ExpectedOutput = GetOutputString(solution),
                        ActualOutput = GetOutputString(studentResult),
                        StudentSolutionComputationTime = stopwatch.ElapsedMilliseconds,
                        SystemSolutionComputationTime = systemSolutionTimeMilliseconds
                    };
                }
                catch (Exception e)
                {
                    stopwatch.Stop();
                    return new TestResult()
                    {
                        Passed = false,
                        Input = GetInputString(solutionTestData),
                        ExpectedOutput = GetOutputString(solution),
                        ActualOutput = "Run time error: " + e.Message,
                        SystemSolutionComputationTime = systemSolutionTimeMilliseconds,
                        StudentSolutionComputationTime = stopwatch.ElapsedMilliseconds,
                    };
                }
            });
            Task<TestResult> timerTask = Task.Run(() =>
            {
                Thread.Sleep(TimeLimitMilliseconds);
                return new TestResult()
                {
                    Passed = false,
                    Input = GetInputString(solutionTestData),
                    ExpectedOutput = GetOutputString(solution),
                    ActualOutput = "Time Limit Exceeded",
                    SystemSolutionComputationTime = systemSolutionTimeMilliseconds,
                    StudentSolutionComputationTime = TimeLimitMilliseconds,
                };
            });

            int result = Task.WaitAny(studentTask, timerTask);
            return result switch
            {
                0 => studentTask.Result,
                1 => timerTask.Result,
                _ => throw new Exception("Task.WaitAny returned an unexpected value")
            };
        }

        public void PrintSolutions()
        {
            IList<Tuple<TInputType1, TInputType2, TInputType3>> tests = GetTests();
            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"Performing test {i}.");
                Console.WriteLine(GetInputString(tests[i]));
                var watch = new Stopwatch();
                watch.Start();
                var result = SolutionFunction(tests[i].Item1, tests[i].Item2, tests[i].Item3);
                watch.Stop();
                Console.WriteLine(GetOutputString(result));
                Console.WriteLine($"Computation time: {watch.ElapsedMilliseconds}ms.\n");
            }
        }

        public virtual bool SolutionsMatch(TOutputType s1, TOutputType s2)
        {
            if (s1 == null)
                return false;
            if (s2 == null)
                return false;

            if (s1 is IEquatable<TOutputType> es1 && s2 is IEquatable<TOutputType> es2)
            {
                return es1.Equals(es2);
            }
            else
            {
                throw new Exception("Data types aren't equatable, and SolutionsMatch method requires override");
            }
        }

        public virtual string GetInputString(Tuple<TInputType1, TInputType2, TInputType3> input)
        {
            return $"[{input.Item1}, {input.Item2}, {input.Item3}]";
        }

        public virtual string GetOutputString(TOutputType output)
        {
            return $"{output}";
        }
    }

    public class TestResult
    {
        public bool Passed { get; init; }
        public bool Failed { 
            get
            {
                return !Passed;
            } 
        }
        public string Input { get; init; }
        public string ExpectedOutput { get; init; }
        public string ActualOutput { get; init; }
        public long StudentSolutionComputationTime { get; init; }
        public long SystemSolutionComputationTime { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex_Day_Challenge_Tester_2.Testers
{
    public class RestockingSuppliesTesterEasy : Tester<int, Dictionary<int, decimal>, decimal>
    {
        public override string TestName => "Restocking Supplies, EASY MODE";

        public override IList<Tuple<int, Dictionary<int, decimal>>> GetTests()
        {
            List<Tuple<int, Dictionary<int, decimal>>> tests = new();

            // test index 0
            tests.Add(new(5, new()
            {
                {1, 1.2m },
                {2, 2.3m },
                {3, 3.3m },
                {4, 4.3m },
                {5, 5m }
            }));

            // test index 1
            tests.Add(new(45, new()
            {
                {1, 1m },
                {5, 4.99m },
                {10, 9.49m },
                {20, 18.49m },
                {40, 35.99m },
                {80, 69.99m }
            }));

            // test index 2
            tests.Add(new(5, new()
            {
                {1, 1.99m},
                {4, 4.99m}
            }));

            // test index 3
            tests.Add(new(444, new(){
                {1, 0.49m},
                {5, 2.20m},
                {10, 3.99m},
                {50, 18.99m},
                {100, 35.99m}
            }));

            // test index 4
            tests.Add(new(0, new(){
                {1, 1.49m},
                {2, 2.89m},
                {4, 5.74m},
                {8, 11.22m},
                {16, 21.99m},
                {32, 39.99m},
                {64, 74.99m},
                {128, 140.00m},
                {256, 274.99m},
                {512, 512.00m}
            }));

            // test index 5
            tests.Add(new(22, new()
            {
                {1, 1m },
                {55, 1.01m }
            }));

            // test index 6
            tests.Add(new(100, new()
            {
                {1, 10m }
            }));

            // test index 7
            tests.Add(new(544, new()
            {
                {1, 0.01m },
                {10, 0.02m },
                {100, 0.03m }
            }));

            return tests;
        }

        public override decimal SolutionFunction(int numberToBuy, Dictionary<int, decimal> prices)
        {
            KeyValuePair<int, decimal>[] orderedDeals = prices.OrderByDescending(kvp => kvp.Value == 0 ? decimal.MaxValue : kvp.Key / kvp.Value).ToArray();
            return RecursiveSolution(numberToBuy, orderedDeals);
        }

        /// <returns>-1 if no solution possible given inputs, otherwise the cheapest result</returns>
        private decimal RecursiveSolution(int numberToBuy, KeyValuePair<int, decimal>[] remainingOrderedDeals)
        {
            if (numberToBuy == 0)
            {
                return 0;
            }

            if (remainingOrderedDeals.Length == 0)
            {
                return -1;
            }


            // the best deal is always in the 0th position of the list
            var bestDeal = remainingOrderedDeals[0];
            int maxPurchaseableOfBestDeal = numberToBuy / bestDeal.Key;

            List<decimal> allPossibleResults = new List<decimal>();

            for (int i = maxPurchaseableOfBestDeal; i >= 0; i--)
            {
                try
                {
                    decimal thisPurchase = i * bestDeal.Value;
                    int remainingToBuy = numberToBuy - (i * bestDeal.Key);
                    if (remainingToBuy == 0)
                    {
                        return thisPurchase;
                    }
                    else if (remainingOrderedDeals.Length == 1)
                    {
                        return -1;
                    }

                    var result = RecursiveSolution(remainingToBuy, remainingOrderedDeals[1..]);

                    if (result != -1)
                    {
                        decimal thisResult = thisPurchase + result;
                        allPossibleResults.Add(thisResult);
                    }
                }
                catch (OverflowException) { }
            }

            if (allPossibleResults.Count == 0)
                return -1;

            return allPossibleResults.Min();
        }

        public override string GetInputString(Tuple<int, Dictionary<int, decimal>> input)
        {
            StringBuilder result = new();
            result.AppendLine($"Number to buy: {input.Item1}");
            result.AppendLine("Prices:");
            result.Append('[');
            result.Append(
                string.Join(", ",
                input.Item2.Select(kvp => $"{{{kvp.Key}, {kvp.Value}}}").ToArray())
                );

            result.Append(']');
            return result.ToString();
        }
    }

    public sealed class RestockingSuppliesTester : RestockingSuppliesTesterEasy
    {
        public override string TestName => "Restocking Supplies";
        public override IList<Tuple<int, Dictionary<int, decimal>>> GetTests()
        {
            IList<Tuple<int, Dictionary<int, decimal>>> tests = base.GetTests();

            // the base class has 8 tests, indexed 0-7
            // test index 8
            tests.Add(new(667, new()
            {
                {334, 0.01m },
                {333, 864m }
            }));

            // test index 9
            tests.Add(new(335, new()
            {
                {81, 0m },
                {27, 0m },
                {9, 0m },
                {6, 0m },
                {3, 0m },
                {2, 50m }
            }));

            // test index 10
            tests.Add(new(1000, new()
            {
                {997, 40m },
                {996, 80m },
                {995, 4m },
                {10, 200m },
                {501, 60m },
                {19, 400m },
                {1, 9999999m },
                {5, 9999999m }
            }));

            // test index 11
            tests.Add(new(90, new()
            {
                {20, 20.99m },
                {14, 17.99m },
                {11, 11.99m }
            }));

            // test index 12
            tests.Add(new(100, new()
            {
                {41, 99.01m },
                {62, 14.55m },
                {7, 101.19m },
                {3, 999.99m },
                {1, 999999m }
            }));

            // test index 13
            tests.Add(new(100, new()
            {
                {50, 59421121885698253195157962751m }, // ~75% of decimal.MaxValue
                {1, 500m }
            }));

            tests.Add(new(100, new()
            {
                {99, 336m },
                {1, 79228162514264337593543950000m }, // decimal.MaxValue - 335
                {50, 400m }
            }));

            return tests;
        }
    }
}

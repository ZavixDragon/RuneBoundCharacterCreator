using System.Collections.Generic;
using TokenCalculator.Core.Calculations;
using Xunit;

namespace TokenCalculatorTests.Core.Calculations
{
    public class CalculatedOutcomesAggregateTests
    {
        [Fact]
        public void GivenNoOutcomes_GivesZeroValues()
        {
            var result = new CalculatedOutcomesAggregate(new List<CalculatedOutcome>()).Get();

            AssertCorrectTypeDetials(0, 0, 0, result.Value);
        }

        [Fact]
        public void GivenValues_ValuesCorrectly()
        {
            var outcome = new CalculatedOutcome
            {
                Value = 1,
                Initiative = 1,
                Damage = 2,
                Block = 0,
                Surge = 1,
                Charge = 0,
                OpenWings = 1,
                OpenTactics = 2
            };
            var outcome2 = new CalculatedOutcome
            {
                Value = 3,
                Initiative = 1,
                Damage = 5,
                Block = 0,
                Surge = 1,
                Charge = 7,
                OpenWings = 0,
                OpenTactics = 0
            };

            var result = new CalculatedOutcomesAggregate(new List<CalculatedOutcome> { outcome, outcome2 }).Get();

            AssertCorrectTypeDetials(1, 2, 3, result.Value);
            AssertCorrectTypeDetials(1, 1, 1, result.Initiative);
            AssertCorrectTypeDetials(2, 3.5m, 5, result.Damage);
            AssertCorrectTypeDetials(0, 0, 0, result.Block);
            AssertCorrectTypeDetials(1, 1, 1, result.Surge);
            AssertCorrectTypeDetials(0, 3.5m, 7, result.Charge);
            AssertCorrectTypeDetials(0, 0.5m, 1, result.OpenWings);
            AssertCorrectTypeDetials(0, 1, 2, result.OpenTactics);
        }

        private void AssertCorrectTypeDetials(decimal expectedMin, decimal expectedAvg, decimal expectedMax, TypeDetails details)
        {
            Assert.Equal(expectedMin, details.Min);
            Assert.Equal(expectedAvg, details.Avg);
            Assert.Equal(expectedMax, details.Max);
        }
    }
}
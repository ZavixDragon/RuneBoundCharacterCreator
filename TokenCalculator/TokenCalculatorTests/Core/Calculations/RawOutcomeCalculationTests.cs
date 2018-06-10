using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Calculations;
using TokenCalculator.Core.Evaluators;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculatorTests.Core.Evaluators.Fakes;
using Xunit;

namespace TokenCalculatorTests.Core.Calculations
{
    public class RawOutcomeCalculationTests
    {
        [Fact]
        public void WithBlank_CorrectResult()
        {
            var token = new RawTokenResult { Type = RawTokenType.Blank };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(0, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithDamage_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.Damage };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(1, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithCharge_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.Charge };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(1, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(1, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithBlock_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.Block };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(0, result.Damage);
            Assert.Equal(1, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithSurge_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.Surge };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(0, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(1, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithOpenWings_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.OpenWings };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(0, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(1, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithOpenTactics_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.OpenTactics };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(0, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(1, result.OpenTactics);
        }

        [Fact]
        public void WithOpenDouble_CorrectProvided()
        {
            var token = new RawTokenResult { Type = RawTokenType.OpenDouble };

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(0, result.Value);
            Assert.Equal(0, result.Initiative);
            Assert.Equal(0, result.Damage);
            Assert.Equal(0, result.Block);
            Assert.Equal(0, result.Surge);
            Assert.Equal(0, result.Charge);
            Assert.Equal(0, result.OpenWings);
            Assert.Equal(0, result.OpenTactics);
        }

        [Fact]
        public void WithInitiative_CorrectInitiative()
        {
            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Initiative = 1 }).Get();

            Assert.Equal(1, result.Initiative);
        }

        [Fact]
        public void WithHigherQuantity_FactorsInQuantity()
        {
            var tokens = new List<RawTokenResult>
            {
                new RawTokenResult { Type = RawTokenType.Charge, Quantity = 3 },
                new RawTokenResult { Type = RawTokenType.Block, Quantity = 3 },
                new RawTokenResult { Type = RawTokenType.Surge, Quantity = 3 }
            }; 

            var result = new RawOutcomeCalculation(new List<IEvaluator>(), new RawOutcome { Tokens = tokens }).Get();

            Assert.Equal(3, result.Damage);
            Assert.Equal(3, result.Charge);
            Assert.Equal(3, result.Block);
            Assert.Equal(3, result.Surge);
        }

        [Fact]
        public void AppliesEvaluator_ValueSet()
        {
            var token = new RawTokenResult { Type = RawTokenType.OpenDouble };
            var evaluator = new EvaluatorFake(outcome => outcome.Tokens.Count(x => x.Type == RawTokenType.OpenDouble));

            var result = new RawOutcomeCalculation(new List<IEvaluator> { evaluator }, 
                new RawOutcome { Tokens = new List<RawTokenResult> { token } }).Get();

            Assert.Equal(1, result.Value);
        }
    }
}
using TokenCalculator.Core.RawOutcomes.Applications;
using TokenCalculator.Core.RawOutcomes.Domain;
using Xunit;

namespace TokenCalculatorTests.Core.RawOutcomes.Applications
{
    public class TacticsApplicationTests
    {
        private readonly RawOutcome _outcome = new RawOutcome();

        [Fact]
        public void NoTactics_ResultGivenBack()
        {
            var results = new TacticsApplication(_outcome, 0).Get();

            Assert.Single(results);
            Assert.Equal(_outcome, results[0]);
        }

        [Fact]
        public void OneTacticsWithNoApplicableToken_OneOpenTactics()
        {
            var results = new TacticsApplication(_outcome, 1).Get();

            Assert.Single(results);
            Assert.Single(results[0].Tokens);
            Assert.Equal(RawTokenType.OpenTactics, results[0].Tokens[0].Type);
        }

        [Theory]
        [InlineData(RawTokenType.Blank, RawTokenType.OpenTactics, false)]
        [InlineData(RawTokenType.Damage, RawTokenType.Damage, true)]
        [InlineData(RawTokenType.Charge, RawTokenType.Charge, true)]
        [InlineData(RawTokenType.Block, RawTokenType.Block, true)]
        [InlineData(RawTokenType.Surge, RawTokenType.Surge, true)]
        [InlineData(RawTokenType.OpenWings, RawTokenType.OpenTactics, false)]
        [InlineData(RawTokenType.OpenTactics, RawTokenType.OpenTactics, false)]
        [InlineData(RawTokenType.OpenDouble, RawTokenType.OpenTactics, false)]
        public void TacticsWithOneOtherToken_CorrectType(RawTokenType existingTokenType, RawTokenType expectedTacticsType, bool wasTactics)
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = existingTokenType });

            var results = new TacticsApplication(_outcome, 1).Get();

            Assert.Single(results);
            Assert.Equal(2, results[0].Tokens.Count);
            Assert.Equal(expectedTacticsType, results[0].Tokens[1].Type);
            Assert.Equal(wasTactics, results[0].Tokens[1].WasTactics);
        }

        [Fact]
        public void TacticsCopiesTwoDamage_QuantityCopied()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage, Quantity = 2 });

            var results = new TacticsApplication(_outcome, 1).Get();

            Assert.Equal(2, results[0].Tokens[1].Quantity);
        }

        [Fact]
        public void OpenTacticsWithHigherQuantityOpenTactics_QuantityNotCopied()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.OpenTactics, Quantity = 2 });

            var results = new TacticsApplication(_outcome, 1).Get();

            Assert.Equal(1, results[0].Tokens[1].Quantity);
        }

        [Fact]
        public void TacticsWithMultipleValueTypes_EachPermutationProduced()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Block });

            var results = new TacticsApplication(_outcome, 1).Get();

            Assert.Contains(results, x => x.Tokens[2].Type == RawTokenType.Damage);
            Assert.Contains(results, x => x.Tokens[2].Type == RawTokenType.Block);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 4)]
        public void TwoTacticsWithTwoValueTypes_ThreeResultsProduced(int tactics, int expectedResults)
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Block });

            var results = new TacticsApplication(_outcome, tactics).Get();

            Assert.Equal(expectedResults, results.Count);
        }

        [Fact]
        public void TacicsAndToken_ArgsPassedInRemainUnmodified()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });

            new TacticsApplication(_outcome, 1).Get();

            Assert.Single(_outcome.Tokens);
        }
    }
}

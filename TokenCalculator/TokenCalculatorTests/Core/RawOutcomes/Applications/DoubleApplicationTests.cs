using System.Linq;
using TokenCalculator.Core.RawOutcomes.Applications;
using TokenCalculator.Core.RawOutcomes.Domain;
using Xunit;

namespace TokenCalculatorTests.Core.RawOutcomes.Applications
{
    public class DoubleApplicationTests
    {
        private readonly RawOutcome _outcome = new RawOutcome();

        [Fact]
        public void NoDoubles_ResultGivenBack()
        {
            var results = new DoubleApplication(_outcome, 0).Get();

            Assert.Single(results);
            Assert.Equal(_outcome, results[0]);
        }

        [Fact]
        public void OneDoubleWithNoApplicableToken_OneOpenDouble()
        {
            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Single(results);
            Assert.Single(results[0].Tokens);
            Assert.Equal(RawTokenType.OpenDouble, results[0].Tokens[0].Type);
        }

        [Theory]
        [InlineData(RawTokenType.Blank)]
        [InlineData(RawTokenType.OpenWings)]
        [InlineData(RawTokenType.OpenTactics)]
        [InlineData(RawTokenType.OpenDouble)]
        public void DoubleWithOneOtherNonApplicableToken_AddsOpenDouble(RawTokenType type)
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = type });

            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Single(results);
            Assert.Equal(2, results[0].Tokens.Count);
            Assert.Equal(RawTokenType.OpenDouble, results[0].Tokens[1].Type);
        }

        [Theory]
        [InlineData(RawTokenType.Damage)]
        [InlineData(RawTokenType.Charge)]
        [InlineData(RawTokenType.Block)]
        [InlineData(RawTokenType.Surge)]
        public void DoubleWithApplicableToken_DoublesUpTheQuantity(RawTokenType type)
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = type });

            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Single(results);
            Assert.Single(results[0].Tokens);
            Assert.Equal(2, results[0].Tokens[0].Quantity);
        }

        [Fact]
        public void DoubleWithAnAlreadyDoubleApplicableToken_AddsOpenDouble()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage, WasDoubled = true });

            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Equal(2, results[0].Tokens.Count);
            Assert.Equal(1, results[0].Tokens[0].Quantity);
            Assert.Equal(RawTokenType.OpenDouble, results[0].Tokens[1].Type);
        }

        [Fact]
        public void DoubleWithTwoDamage_GoesToFourQuantity()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage, Quantity = 2 });

            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Equal(4, results[0].Tokens[0].Quantity);
        }

        [Fact]
        public void DoubleWithMultipleValueTypes_EachPermutationProduced()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Block });

            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Contains(results, x => x.Tokens.First(token => token.WasDoubled).Type == RawTokenType.Damage);
            Assert.Contains(results, x => x.Tokens.First(token => token.WasDoubled).Type == RawTokenType.Block);
        }

        [Fact]
        public void TwoDoublesWithOneApplicableToken_TokenDoubledAndOpenDoubledAdded()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });

            var results = new DoubleApplication(_outcome, 2).Get();

            Assert.Single(results);
            Assert.Equal(2, results[0].Tokens.Count);
            Assert.Contains(results[0].Tokens, x => x.WasDoubled && x.Type == RawTokenType.Damage && x.Quantity == 2);
            Assert.Contains(results[0].Tokens, x => x.Type == RawTokenType.OpenDouble);
        }

        [Fact]
        public void TwoDoublesWithTwoApplicableTokens_BothTokensDoubled()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Block });

            var results = new DoubleApplication(_outcome, 2).Get();

            Assert.Single(results);
            Assert.Equal(2, results[0].Tokens.Count);
            Assert.Contains(results[0].Tokens, x => x.WasDoubled && x.Type == RawTokenType.Damage && x.Quantity == 2);
            Assert.Contains(results[0].Tokens, x => x.WasDoubled && x.Type == RawTokenType.Block && x.Quantity == 2);
        }

        [Fact]
        public void TwoDoublesWithThreeApplicableTokens_ThreeResultsProduced()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Block });
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Surge });

            var results = new DoubleApplication(_outcome, 2).Get();

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public void DoubleWithATacticsDamageToken_OpenDoubleAdded()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage, WasTactics = true });

            var results = new DoubleApplication(_outcome, 1).Get();

            Assert.Equal(2, results[0].Tokens.Count);
            Assert.Equal(RawTokenType.OpenDouble, results[0].Tokens[1].Type);
        }

        [Fact]
        public void DoublesAndToken_ArgsPassedInRemainUnmodified()
        {
            _outcome.Tokens.Add(new RawTokenResult { Type = RawTokenType.Damage });

            new DoubleApplication(_outcome, 1).Get();

            Assert.Single(_outcome.Tokens);
        }
    }
}

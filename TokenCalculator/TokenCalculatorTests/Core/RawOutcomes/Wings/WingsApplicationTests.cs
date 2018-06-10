using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Core.RawOutcomes.Wings;
using TokenCalculator.Core.Tokens;
using Xunit;

namespace TokenCalculatorTests.Core.RawOutcomes.Wings
{
    public class WingsApplicationTests
    {
        private readonly RawOutcome _outcome = new RawOutcome();
        private readonly List<Token> _tokens = new List<Token>();

        [Fact]
        public void NoWings_UnaffectedOutcome()
        {
            var results = new WingsApplication(_tokens, _outcome).Get();

            Assert.Single(results);
            Assert.True(_outcome.Tokens.Matches(results[0].Outcome.Tokens));
            Assert.Empty(results[0].Tokens);
        }

        [Fact]
        public void OneWings_OneOpenWings()
        {
            _tokens.Add(new Token { FaceUp = TokenSideType.Wings });

            var results = new WingsApplication(_tokens, _outcome).Get();

            Assert.Single(results);
            Assert.Empty(results[0].Tokens);
            Assert.Single(results[0].Outcome.Tokens);
            Assert.Equal(RawTokenType.OpenWings, results[0].Outcome.Tokens[0].Type);
        }

        [Fact]
        public void OneWingsAndOneOtherNonWings_WithAndWithoutWings()
        {
            _tokens.Add(new Token { FaceUp = TokenSideType.Wings });
            _tokens.Add(new Token { FaceUp = TokenSideType.Damage, FaceDown = TokenSideType.Block });

            var results = new WingsApplication(_tokens, _outcome).Get();

            Assert.Equal(2, results.Count);
            Assert.Contains(results, result => result.Outcome.Tokens.Any(x => x.Type == RawTokenType.OpenWings) 
                                               && result.Tokens[0].FaceUp.Type == TokenSideType.Damage
                                               && result.Tokens.Count == 1);
            Assert.Contains(results, result => !result.Outcome.Tokens.Any() 
                                               && result.Tokens[0].FaceUp.Type == TokenSideType.Block
                                               && result.Tokens.Count == 1);
        }

        [Fact]
        public void TwoWingsWithOneOtherToken_AllResultsProduced()
        {
            _tokens.Add(new Token { FaceUp = TokenSideType.Wings, FaceDown = TokenSideType.Damage });
            _tokens.Add(new Token { FaceUp = TokenSideType.Wings, FaceDown = TokenSideType.Block });
            _tokens.Add(new Token { FaceUp = TokenSideType.Surge, FaceDown = TokenSideType.Wings });

            var results = new WingsApplication(_tokens, _outcome).Get();

            Assert.Equal(7, results.Count);
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 2 
                                               && result.Outcome.Tokens.All(x => x.Type == RawTokenType.OpenWings)
                                               && result.Tokens.Count == 1
                                               && result.Tokens[0].FaceUp.Type == TokenSideType.Surge);
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 2
                                               && result.Outcome.Tokens.All(x => x.Type == RawTokenType.OpenWings)
                                               && result.Tokens.Count == 0);
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 0
                                               && result.Tokens.Count == 2
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Surge)
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Damage));
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 0
                                               && result.Tokens.Count == 2
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Surge)
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Block));
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 0
                                               && result.Tokens.Count == 1
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Damage));
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 0
                                               && result.Tokens.Count == 1
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Block));
            Assert.Contains(results, result => result.Outcome.Tokens.Count == 0
                                               && result.Tokens.Count == 1
                                               && result.Tokens.Any(x => x.FaceUp.Type == TokenSideType.Surge));
        }
    }
}

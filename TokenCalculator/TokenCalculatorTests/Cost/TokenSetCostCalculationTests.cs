using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Tokens;
using TokenCalculator.Cost;
using Xunit;

namespace TokenCalculatorTests.Cost
{
    public class TokenSetCostCalculationTests
    {
        [Fact]
        public void NoTokens_Zero()
        {
            var tokens = new List<Token>();

            var cost = new TokenSetCostCalculation(tokens).Get();

            Assert.Equal(0, cost);
        }

        [Theory]
        [InlineData(TokenSideType.Blank, 1, 0)]
        [InlineData(TokenSideType.Wings, 1, 2)]
        [InlineData(TokenSideType.Double, 1, 3)]
        [InlineData(TokenSideType.Tactics, 1, 4)]
        [InlineData(TokenSideType.Surge, 1, 3)]
        [InlineData(TokenSideType.Surge, 2, 5)]
        [InlineData(TokenSideType.Block, 1, 3)]
        [InlineData(TokenSideType.Block, 2, 7)]
        [InlineData(TokenSideType.Damage, 1, 3)]
        [InlineData(TokenSideType.Damage, 2, 7)]
        [InlineData(TokenSideType.Charge, 1, 4)]
        [InlineData(TokenSideType.Charge, 2, 9)]
        public void OneTokenWithOneSideBlank_CostIsExpected(TokenSideType type, int quantity, int tokenTypeCost)
        {
            var tokens = new List<Token> { new Token { FaceUp = new TokenSide { Type = type, Quantity = quantity } } };

            var cost = new TokenSetCostCalculation(tokens).Get();

            Assert.Equal(tokenTypeCost + 2, cost);
        }

        [Fact]
        public void FiveBlankTokens_TenCost()
        {
            var tokens = new List<Token>
            {
                new Token(),
                new Token(),
                new Token(),
                new Token(),
                new Token()
            };

            var cost = new TokenSetCostCalculation(tokens).Get();

            Assert.Equal(10, cost);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 10)]
        [InlineData(3, 18)]
        public void MultipleTactics_CostGoesUp(int tactics, int expectedTacticsCost)
        {
            var tokens = Enumerable.Range(0, tactics).Select(x => 
                new Token { FaceUp = TokenSideType.Tactics }).ToList();

            var cost = new TokenSetCostCalculation(tokens).Get();

            Assert.Equal(expectedTacticsCost + tactics * 2, cost);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 4)]
        [InlineData(4, 6)]
        [InlineData(5, 9)]
        public void OneSidedInitiate_CostGoesUp(int initiatives, int expectedInitiativeCost)
        {
            var tokens = Enumerable.Range(0, initiatives).Select(x =>
                new Token { FaceUp = new TokenSide { HasInitiative = true } }).ToList();

            var cost = new TokenSetCostCalculation(tokens).Get();

            Assert.Equal(expectedInitiativeCost + initiatives * 2, cost);
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 8)]
        [InlineData(3, 15)]
        public void TwoSidedInitiative_DoubleSidedPenalty(int doubleSidedInitiatives, int expectedInitiativeCost)
        {
            var tokens = Enumerable.Range(0, doubleSidedInitiatives).Select(x =>
                new Token
                {
                    FaceUp = new TokenSide { HasInitiative = true },
                    FaceDown = new TokenSide { HasInitiative = true }
                }).ToList();

            var cost = new TokenSetCostCalculation(tokens).Get();

            Assert.Equal(expectedInitiativeCost + doubleSidedInitiatives * 2, cost);
        }
    }
}

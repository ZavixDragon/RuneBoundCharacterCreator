using System.Collections.Generic;
using TokenCalculator.Core.Calculations;
using TokenCalculator.Core.Tokens;
using Xunit;

namespace TokenCalculatorTests.Core.Calculations
{
    public class InitiativeCalculationTests
    {
        [Fact]
        public void WithFaceUpAndFaceDownInitiative_CountsCorrectly()
        {
            var tokens = new List<Token>
            {
                new Token { FaceUp = new TokenSide { HasInitiative = false }, FaceDown = new TokenSide { HasInitiative = false } },
                new Token { FaceUp = new TokenSide { HasInitiative = false }, FaceDown = new TokenSide { HasInitiative = true } },
                new Token { FaceUp = new TokenSide { HasInitiative = true }, FaceDown = new TokenSide { HasInitiative = false } },
                new Token { FaceUp = new TokenSide { HasInitiative = true }, FaceDown = new TokenSide { HasInitiative = true } },
                new Token { FaceUp = new TokenSide { HasInitiative = false }, FaceDown = new TokenSide { HasInitiative = false } },
                new Token { FaceUp = new TokenSide { HasInitiative = false }, FaceDown = new TokenSide { HasInitiative = true } },
            };

            var initiative = new InitiativeCalculation(tokens).Get();

            Assert.Equal(2, initiative);
        }
    }
}

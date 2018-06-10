using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Applications;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Core.Tokens;
using Xunit;

namespace TokenCalculatorTests.Core.RawOutcomes.Applications
{
    public class SimpleApplicationTests
    {
        private readonly RawOutcome _outcome = new RawOutcome();

        [Theory]
        [InlineData(TokenSideType.Blank, true)]
        [InlineData(TokenSideType.Wings, false)]
        [InlineData(TokenSideType.Double, false)]
        [InlineData(TokenSideType.Tactics, false)]
        [InlineData(TokenSideType.Surge, true)]
        [InlineData(TokenSideType.Block, true)]
        [InlineData(TokenSideType.Damage, true)]
        [InlineData(TokenSideType.Charge, true)]
        public void SingleToken_SimpleOneIsTransferredOver(TokenSideType type, bool addedToOutcome)
        {
            var result = new SimpleApplication(new List<Token> { new Token { FaceUp = type } }, _outcome).Get();

            Assert.Equal(addedToOutcome, result.Tokens.Any());
        }

        [Fact]
        public void TokenAndOutcomeGiven_ArgumentsRemainUnmodified()
        {
            var tokens = new List<Token> { new Token { FaceUp = TokenSideType.Damage } };

            new SimpleApplication(tokens, _outcome).Get();

            Assert.Single(tokens);
            Assert.Empty(_outcome.Tokens);
        }
    }
}
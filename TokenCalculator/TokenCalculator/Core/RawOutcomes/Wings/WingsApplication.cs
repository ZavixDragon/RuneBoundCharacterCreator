using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Core.Tokens;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.RawOutcomes.Wings
{
    public class WingsApplication
    {
        private readonly List<Token> _tokens;
        private readonly RawOutcome _outcome;

        public WingsApplication(List<Token> tokens, RawOutcome outcome)
        {
            _tokens = tokens;
            _outcome = outcome;
        }

        public List<AppliedWingsOutcome> Get()
        {
            if (_tokens.All(x => x.FaceUp.Type != TokenSideType.Wings))
            {
                return new List<AppliedWingsOutcome> { new AppliedWingsOutcome
                {
                    Outcome = _outcome,
                    Tokens = _tokens
                }};
            }
            return _tokens
                .Where(token => token.FaceUp.Type == TokenSideType.Wings)
                .SelectMany(wings => _tokens
                    .SelectMany(token =>
                    {
                        if (token == wings)
                            return new WingsApplication(_tokens.Without(wings).ToList(),
                                _outcome.With(new RawTokenResult {Type = RawTokenType.OpenWings})).Get();
                        var clone = _tokens.Without(wings).ToList();
                        var index = clone.IndexOf(token);
                        clone = clone.Select(x => x.Clone()).ToList();
                        clone[index] = clone[index].Flip();
                        return new WingsApplication(clone, _outcome).Get();
                    }))
                .WithoutDuplicates()
                .ToList();
        }
    }
}

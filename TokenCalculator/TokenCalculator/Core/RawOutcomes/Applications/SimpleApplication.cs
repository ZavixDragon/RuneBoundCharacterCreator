using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Core.Tokens;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.RawOutcomes.Applications
{
    public class SimpleApplication
    {
        private readonly Dictionary<TokenSideType, RawTokenType> _typeMap = new Dictionary<TokenSideType, RawTokenType>
        {
            { TokenSideType.Blank, RawTokenType.Blank },
            { TokenSideType.Damage, RawTokenType.Damage },
            { TokenSideType.Charge, RawTokenType.Charge },
            { TokenSideType.Block, RawTokenType.Block },
            { TokenSideType.Surge, RawTokenType.Surge },
        };
        private readonly List<Token> _tokens;
        private readonly RawOutcome _outcome;

        public SimpleApplication(List<Token> tokens, RawOutcome outcome)
        {
            _tokens = tokens;
            _outcome = outcome;
        }

        public RawOutcome Get()
        {
            var result = _outcome.Clone();
            _tokens.Where(x => _typeMap.ContainsKey(x.FaceUp.Type))
                .ForEach(x => result = _outcome.With(new RawTokenResult
                {
                    Type = _typeMap[x.FaceUp.Type],
                    Quantity = x.FaceUp.Quantity
                }));
            return result;
        }
    }
}

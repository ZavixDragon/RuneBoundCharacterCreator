using System;
using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Tokens;

namespace TokenCalculator.Cost
{
    public class TokenSetCostCalculation
    {
        private readonly Dictionary<TokenSideType, int> _tokenTypeCost = new Dictionary<TokenSideType, int>
        {
            {TokenSideType.Blank, 0},
            {TokenSideType.Wings, 2},
            {TokenSideType.Double, 3},
            {TokenSideType.Tactics, 4},
            {TokenSideType.Surge, 3},
            {TokenSideType.Block, 3},
            {TokenSideType.Damage, 3},
            {TokenSideType.Charge, 4}
        };
        private readonly Dictionary<TokenSideType, int> _extraQuantityPenalty = new Dictionary<TokenSideType, int>
        {
            {TokenSideType.Blank, 0},
            {TokenSideType.Wings, 0},
            {TokenSideType.Double, 0},
            {TokenSideType.Tactics, 0},
            {TokenSideType.Surge, 2},
            {TokenSideType.Block, 4},
            {TokenSideType.Damage, 4},
            {TokenSideType.Charge, 5}
        };
        private readonly List<Token> _tokens;

        public TokenSetCostCalculation(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public int Get()
        {
            var tokenTypes = _tokens.Select(x => x.FaceUp).Concat(_tokens.Select(x => x.FaceDown)).ToList();
            return _tokens.Count * 2
                   + tokenTypes.Sum(x => _tokenTypeCost[x.Type])
                   + tokenTypes.Sum(x => _extraQuantityPenalty[x.Type] * (x.Quantity - 1))
                   + Enumerable.Range(0, tokenTypes.Count(x => x.Type == TokenSideType.Tactics)).Sum(x => x * 2)
                   + Enumerable.Range(1, tokenTypes.Count(x => x.HasInitiative)).Sum(x => (int)Math.Ceiling((double)x / 2))
                   + _tokens.Count(x => x.FaceUp.HasInitiative && x.FaceDown.HasInitiative);
        }
    }
}

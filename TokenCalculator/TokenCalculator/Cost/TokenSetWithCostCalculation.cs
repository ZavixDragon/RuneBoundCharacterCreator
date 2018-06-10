using System.Collections.Generic;
using TokenCalculator.Core;
using TokenCalculator.Core.Evaluators;
using TokenCalculator.Core.Tokens;

//TODO: Test
namespace TokenCalculator.Cost
{
    public class TokenSetWithCostCalculation
    {
        private readonly List<Token> _tokenSet;
        private readonly List<IEvaluator> _evaluators;

        public TokenSetWithCostCalculation(List<Token> tokenSet, List<IEvaluator> evaluators)
        {
            _tokenSet = tokenSet;
            _evaluators = evaluators;
        }

        public TokenSetWithCostResult Get()
        {
            return new TokenSetWithCostResult
            {
                Cost = new TokenSetCostCalculation(_tokenSet).Get(),
                TokenSet = new TokenSetCalculation(_tokenSet, _evaluators).CombineAllPossibleBestOutcomes()
            };
        }
    }
}

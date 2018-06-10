using System;
using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Calculations;
using TokenCalculator.Core.Evaluators;
using TokenCalculator.Core.RawOutcomes.Applications;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Core.RawOutcomes.Wings;
using TokenCalculator.Core.Tokens;


//TODO: Test
namespace TokenCalculator.Core
{
    public class TokenSetCalculation
    {
        private readonly List<Token> _tokens;
        private List<IEvaluator> _evaluators;

        public TokenSetCalculation(List<Token> tokens, List<IEvaluator> evaluators)
        {
            _tokens = tokens;
            _evaluators = evaluators;
        }

        public TokenSetResult CombineAllPossibleBestOutcomes()
        {
            var outcomes = new List<CalculatedOutcome>();
            for (var i = 0; i < Math.Pow(2, _tokens.Count); i++)
            {
                var _tokensCopy = _tokens.ToList();
                var temp = i;
                while (temp != 0)
                {
                    var temp2 = 1;
                    var index = 0;
                    while (temp2 * 2 <= temp)
                    {
                        temp2 *= 2;
                        index++;
                    }
                    _tokensCopy[index] = _tokensCopy[index].Flip();
                    temp -= temp2;
                }
                outcomes.Add(CalculateBestOutcome(_tokensCopy));
            }
            return new CalculatedOutcomesAggregate(outcomes).Get();
        }

        private CalculatedOutcome CalculateBestOutcome(List<Token> tokens)
        {
            var outcome = new RawOutcome { Initiative = new InitiativeCalculation(tokens).Get() };
            var wingOutcomes = new WingsApplication(tokens, outcome).Get();
            var outcomes = wingOutcomes.SelectMany(x =>
            {
                var simpleOutcome = new SimpleApplication(x.Tokens, x.Outcome).Get();
                var doubledOutcomes = new DoubleApplication(simpleOutcome, 
                    x.Tokens.Count(token => token.FaceUp.Type == TokenSideType.Double)).Get();
                var tacticsOutcomes = doubledOutcomes.SelectMany(doubledOutcome => new TacticsApplication(doubledOutcome,
                        x.Tokens.Count(token => token.FaceUp.Type == TokenSideType.Tactics)).Get()).ToList();
                return tacticsOutcomes;
            }).ToList();
            var calculatedOutcomes = outcomes.Select(x => new RawOutcomeCalculation(_evaluators, x).Get());
            return calculatedOutcomes.OrderByDescending(x => x.Value).First();
        }
    }
}

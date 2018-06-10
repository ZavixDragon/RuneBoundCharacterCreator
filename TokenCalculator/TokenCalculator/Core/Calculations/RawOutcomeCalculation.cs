using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Evaluators;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.Calculations
{
    public class RawOutcomeCalculation
    {
        private readonly List<IEvaluator> _evaluators;
        private readonly RawOutcome _outcome;

        public RawOutcomeCalculation(List<IEvaluator> evaluators, RawOutcome outcome)
        {
            _evaluators = evaluators;
            _outcome = outcome;
        }

        public CalculatedOutcome Get()
        {
            return new CalculatedOutcome
            {
                Value = _evaluators.Sum(x => x.CalculateValue(_outcome)),
                Initiative = _outcome.Initiative,
                Damage = _outcome.Tokens.Where(x => x.Type == RawTokenType.Damage || x.Type == RawTokenType.Charge).Sum(x => x.Quantity),
                Block = _outcome.Tokens.Where(x => x.Type == RawTokenType.Block).Sum(x => x.Quantity),
                Surge = _outcome.Tokens.Where(x => x.Type == RawTokenType.Surge).Sum(x => x.Quantity),
                Charge = _outcome.Tokens.Where(x => x.Type == RawTokenType.Charge).Sum(x => x.Quantity),
                OpenWings = _outcome.Tokens.Count(x => x.Type == RawTokenType.OpenWings),
                OpenTactics = _outcome.Tokens.Count(x => x.Type == RawTokenType.OpenTactics)
            };
        }
    }
}
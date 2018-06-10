using System;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.Evaluators
{
    public class UsedTacticsEvaluator : IEvaluator
    {
        public Decimal UsedTacticsValue { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            return outcome.Tokens.Where(x => x.WasTactics).Sum(x => UsedTacticsValue);
        }
    }
}

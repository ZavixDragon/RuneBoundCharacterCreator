using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.Evaluators
{
    public class ChargeEvaluator : IEvaluator
    {
        public decimal ChargeValue { get; set; }
        public decimal NoNormalDamageBonus { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            var result = outcome.Tokens.Where(x => x.Type == RawTokenType.Charge).Sum(x => x.Quantity * ChargeValue);
            if (outcome.Tokens.All(x => x.Type != RawTokenType.Damage))
                result += NoNormalDamageBonus;
            return result;
        }
    }
}

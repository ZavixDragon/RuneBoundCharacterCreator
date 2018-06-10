using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.Evaluators
{
    public class OpenDoubleEvaluator : IEvaluator
    {
        public decimal OpenDoubleValue { get; set; }
        public decimal DiminishingPercentageMultiplier { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            var multiplier = 1m;
            var result = 0m;
            outcome.Tokens.Where(x => x.Type == RawTokenType.OpenDouble).ForEach(x =>
            {
                result += OpenDoubleValue * multiplier;
                multiplier -= multiplier * (DiminishingPercentageMultiplier / 100);
            });
            return result;
        }
    }
}

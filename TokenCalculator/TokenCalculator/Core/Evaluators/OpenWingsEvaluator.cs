using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.Evaluators
{
    public class OpenWingsEvaluator : IEvaluator
    {
        public decimal OpenWingsValue { get; set; }
        public decimal DiminishingMultiplierPercentage { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            var multiplier = 1m;
            var result = 0m;
            outcome.Tokens.Where(x => x.Type == RawTokenType.OpenWings)
                .ForEach(x =>
                {
                    result += OpenWingsValue * multiplier;
                    multiplier -= multiplier * (DiminishingMultiplierPercentage / 100);
                });
            return result;
        }
    }
}

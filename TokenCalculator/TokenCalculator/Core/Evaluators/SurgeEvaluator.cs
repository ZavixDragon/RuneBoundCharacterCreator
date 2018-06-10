using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.Evaluators
{
    public class SurgeEvaluator : IEvaluator
    {
        public List<decimal> SurgeTokenValue { get; set; }
        public decimal AtLeastTwoSurgeOnTokenBonus { get; set; }
        public decimal AtLeastThreeSurgeOnTokenBonus { get; set; }
        public decimal EachQuantityPastThreeOnTokenBonus { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            var multiplier = 1m;
            var result = 0m;
            outcome.Tokens.Where(x => x.Type == RawTokenType.Surge).OrderBy(x => x.Quantity).ForEach(x =>
            {
                var tokenValue = SurgeTokenValue;
                if (x.Quantity >= 2)
                    tokenValue += AtLeastTwoSurgeOnTokenBonus;
                if (x.Quantity >= 3)
                    tokenValue += AtLeastThreeSurgeOnTokenBonus;
                if (x.Quantity >= 4)
                    tokenValue += EachQuantityPastThreeOnTokenBonus * (x.Quantity - 3);
                result += tokenValue * multiplier;
                multiplier -= multiplier * (PerTokenDiminishingPercentageMultiplier / 100);
            });
            return result;
        }
    }
}

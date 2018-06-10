using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.Evaluators
{
    public class DamageEvaluator : IEvaluator
    {
        public decimal DamageValue { get; set; }
        public decimal FirstDamagePenalty { get; set; }
        public decimal SecondDamagePenalty { get; set; }
        public decimal AtLeastTwoDamageTokenOneTimePenalty { get; set; }
        public decimal AtLeastThreeDamageHighestTokenOneTimePenalty { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            if (outcome.Tokens.All(x => x.Type != RawTokenType.Damage))
                return 0;
            var result = outcome.Tokens.Where(x => x.Type == RawTokenType.Damage).Sum(x => x.Quantity) * DamageValue;
            result -= FirstDamagePenalty;
            if (outcome.Tokens.Where(x => x.Type == RawTokenType.Damage).Sum(x => x.Quantity) >= 2)
                result -= SecondDamagePenalty;
            var highestToken = outcome.Tokens.Where(x => x.Type == RawTokenType.Damage).OrderByDescending(x => x.Quantity).First();
            if (highestToken.Quantity >= 2)
                result -= AtLeastTwoDamageTokenOneTimePenalty;
            if (highestToken.Quantity >= 3)
                result -= AtLeastThreeDamageHighestTokenOneTimePenalty;
            return result;
        }
    }
}

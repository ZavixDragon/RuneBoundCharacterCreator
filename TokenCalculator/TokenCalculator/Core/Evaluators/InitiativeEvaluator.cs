using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.Evaluators
{
    public class InitiativeEvaluator : IEvaluator
    {
        public decimal FirstInitiativeValue { get; set; }
        public decimal SecondInitiativeValue { get; set; }
        public decimal ThirdInitiativeValue { get; set; }
        public decimal FourthInitiativeValue { get; set; }
        public decimal FifthInitiativeValue { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            var result = 0m;
            if (outcome.Initiative >= 1)
                result += FirstInitiativeValue;
            if (outcome.Initiative >= 2)
                result += SecondInitiativeValue;
            if (outcome.Initiative >= 3)
                result += ThirdInitiativeValue;
            if (outcome.Initiative >= 4)
                result += FourthInitiativeValue;
            if (outcome.Initiative >= 5)
                result += FifthInitiativeValue;
            return result;
        }
    }
}

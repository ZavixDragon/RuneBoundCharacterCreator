using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.Evaluators
{
    public interface IEvaluator
    {
        decimal CalculateValue(RawOutcome outcome);
    }
}

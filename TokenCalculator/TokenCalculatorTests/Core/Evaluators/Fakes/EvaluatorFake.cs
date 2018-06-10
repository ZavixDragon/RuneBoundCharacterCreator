using System;
using TokenCalculator.Core.Evaluators;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculatorTests.Core.Evaluators.Fakes
{
    public class EvaluatorFake : IEvaluator
    {
        private readonly Func<RawOutcome, decimal> _onEvaluate;

        public EvaluatorFake(Func<RawOutcome, decimal> onEvaluate)
        {
            _onEvaluate = onEvaluate;
        }

        public decimal CalculateValue(RawOutcome outcome)
        {
            return _onEvaluate(outcome);
        }
    }
}

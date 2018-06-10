using System.Collections.Generic;
using System.Linq;

namespace TokenCalculator.Core.Calculations
{
    public class CalculatedOutcomesAggregate
    {
        private readonly List<CalculatedOutcome> _outcomes;

        public CalculatedOutcomesAggregate(List<CalculatedOutcome> outcomes)
        {
            _outcomes = outcomes;
        }

        public TokenSetResult Get()
        {
            return new TokenSetResult
            {
                Value = Spawn(_outcomes.Select(x => x.Value).ToList()),
                Damage = Spawn(_outcomes.Select(x => x.Damage)),
                Block = Spawn(_outcomes.Select(x => x.Block)),
                Surge = Spawn(_outcomes.Select(x => x.Surge)),
                OpenWings = Spawn(_outcomes.Select(x => x.OpenWings)),
                Initiative = Spawn(_outcomes.Select(x => x.Initiative)),
                OpenTactics = Spawn(_outcomes.Select(x => x.OpenTactics)),
                Charge = Spawn(_outcomes.Select(x => x.Charge))
            };
        }

        private TypeDetails Spawn(IEnumerable<int> values)
        {
            return Spawn(values.Select(x => new decimal(x)).ToList());
        }

        private TypeDetails Spawn(List<decimal> values)
        {
            return values.Any() 
                ? new TypeDetails
                {
                    Avg = values.Sum(x => x) / values.Count,
                    Min = values.Min(),
                    Max = values.Max()
                }
                : new TypeDetails();
        }
    }
}
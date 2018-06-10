using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.RawOutcomes.Domain
{
    public static class RawOutcomeExtensions
    {
        public static List<RawOutcome> WithoutDuplicates(this IEnumerable<RawOutcome> outcomes)
        {
            var distinctList = new List<RawOutcome>();
            outcomes.ForEach(outcome =>
            {
                if (!distinctList.Any(x => x.Matches(outcome)))
                    distinctList.Add(outcome);
            });
            return distinctList;
        }
    }
}

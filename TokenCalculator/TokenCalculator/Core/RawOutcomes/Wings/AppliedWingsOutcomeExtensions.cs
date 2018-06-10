using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.RawOutcomes.Wings
{
    public static class AppliedWingsOutcomeExtensions
    {
        public static List<AppliedWingsOutcome> WithoutDuplicates(this IEnumerable<AppliedWingsOutcome> outcomes)
        {
            var distinctList = new List<AppliedWingsOutcome>();
            outcomes.ForEach(outcome =>
            {
                if (!distinctList.Any(x => x.Matches(outcome)))
                    distinctList.Add(outcome);
            });
            return distinctList;
        }
    }
}

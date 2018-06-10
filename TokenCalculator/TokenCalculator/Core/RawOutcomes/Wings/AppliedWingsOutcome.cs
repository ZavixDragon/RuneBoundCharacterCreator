using System.Collections.Generic;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Core.Tokens;

namespace TokenCalculator.Core.RawOutcomes.Wings
{
    public class AppliedWingsOutcome
    {
        public List<Token> Tokens { get; set; }
        public RawOutcome Outcome { get; set; }

        public bool Matches(AppliedWingsOutcome other)
        {
            return Outcome.Matches(other.Outcome)
                   && Tokens.Matches(other.Tokens);
        }
    }
}

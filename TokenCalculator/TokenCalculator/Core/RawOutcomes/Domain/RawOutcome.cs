using System.Collections.Generic;
using System.Linq;

namespace TokenCalculator.Core.RawOutcomes.Domain
{
    public class RawOutcome
    {
        public int Initiative { get; set; } = 0;
        public List<RawTokenResult> Tokens { get; set; } = new List<RawTokenResult>();

        public RawOutcome With(RawTokenResult result)
        {
            var clone = Clone();
            clone.Tokens.Add(result);
            return clone;
        }

        public RawOutcome Without(RawTokenResult result)
        {
            var clone = Clone();
            clone.Tokens.RemoveAt(Tokens.IndexOf(result));
            return clone;
        }

        public bool Matches(RawOutcome other)
        {
            return Initiative == other.Initiative
                   && Tokens.Matches(other.Tokens);
        }

        public RawOutcome Clone()
        {
            return new RawOutcome
            {
                Initiative = Initiative,
                Tokens = Tokens.Select(x => x.Clone()).ToList()
            };
        }
    }
}

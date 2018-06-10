using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.RawOutcomes.Applications
{
    public class DoubleApplication
    {
        private readonly List<RawTokenType> _doubleApplicableType = new List<RawTokenType>
        {
            RawTokenType.Damage,
            RawTokenType.Charge,
            RawTokenType.Block,
            RawTokenType.Surge
        };
        private readonly RawOutcome _outcome;
        private readonly int _doubles;

        public DoubleApplication(RawOutcome outcome, int doubles)
        {
            _outcome = outcome;
            _doubles = doubles;
        }

        public List<RawOutcome> Get()
        {
            if (_doubles == 0)
                return new List<RawOutcome> { _outcome };
            if (!_outcome.Tokens.Any(x => _doubleApplicableType.Contains(x.Type) && !x.WasDoubled && !x.WasTactics))
                return new DoubleApplication(_outcome.With(new RawTokenResult { Type = RawTokenType.OpenDouble }), _doubles - 1).Get();
            return _outcome.Tokens
                .Where(x => _doubleApplicableType.Contains(x.Type) && !x.WasDoubled && !x.WasTactics)
                .SelectMany(x => new DoubleApplication(_outcome.Without(x).With(x.DoubleClone()), _doubles - 1).Get())
                .WithoutDuplicates()
                .ToList();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;

namespace TokenCalculator.Core.RawOutcomes.Applications
{
    public class TacticsApplication
    {
        private readonly List<RawTokenType> _tacticsApplicableType = new List<RawTokenType>
        {
            RawTokenType.Damage,
            RawTokenType.Charge,
            RawTokenType.Block,
            RawTokenType.Surge
        };
        private readonly RawOutcome _outcome;
        private readonly int _tactics;

        public TacticsApplication(RawOutcome outcome, int tactics)
        {
            _outcome = outcome;
            _tactics = tactics;
        }

        public List<RawOutcome> Get()
        {
            if (_tactics == 0)
                return new List<RawOutcome> { _outcome };
            if (!_outcome.Tokens.Any(x => _tacticsApplicableType.Contains(x.Type)))
                return new TacticsApplication(_outcome.With(new RawTokenResult { Type = RawTokenType.OpenTactics }), _tactics - 1).Get();
            return _outcome.Tokens
                .Where(x => _tacticsApplicableType.Contains(x.Type))
                .SelectMany(x => new TacticsApplication(_outcome.With(x.TacticsClone()), _tactics - 1).Get())
                .WithoutDuplicates()
                .ToList();
        }
    }
}

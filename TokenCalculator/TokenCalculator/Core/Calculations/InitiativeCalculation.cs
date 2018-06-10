using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Tokens;

namespace TokenCalculator.Core.Calculations
{
    public class InitiativeCalculation
    {
        private readonly List<Token> _tokens;

        public InitiativeCalculation(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public int Get()
        {
            return _tokens.Count(x => x.FaceUp.HasInitiative);
        }
    }
}

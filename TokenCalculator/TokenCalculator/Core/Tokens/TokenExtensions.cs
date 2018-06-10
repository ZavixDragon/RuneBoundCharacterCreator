using System.Collections.Generic;
using System.Linq;

namespace TokenCalculator.Core.Tokens
{
    public static class TokenExtensions
    {
        public static bool Matches(this IEnumerable<Token> tokens, IEnumerable<Token> tokens2)
        {
            var list1 = tokens.ToList();
            var list2 = tokens2.ToList();
            if (list1.Count != list2.Count)
                return false;
            for (var i = list1.Count - 1; i >= 0; i--)
            {
                var optionalToken = list1.FirstOrDefault(x => x.Matches(list2[i]));
                if (optionalToken == null)
                    return false;
                list1.Remove(optionalToken);
                list2.RemoveAt(i);
            }
            return true;
        }
    }
}

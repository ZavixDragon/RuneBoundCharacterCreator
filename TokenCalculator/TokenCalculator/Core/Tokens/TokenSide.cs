namespace TokenCalculator.Core.Tokens
{
    public class TokenSide
    {
        public bool HasInitiative { get; set; } = false;
        public TokenSideType Type { get; set; } = TokenSideType.Blank;
        public int Quantity { get; set; } = 1;

        public bool Matches(TokenSide other)
        {
            return HasInitiative == other.HasInitiative
                   && Type == other.Type
                   && Quantity == other.Quantity;
        }

        public static implicit operator TokenSide(TokenSideType type)
        {
            return new TokenSide { Type = type };
        }
    }
}

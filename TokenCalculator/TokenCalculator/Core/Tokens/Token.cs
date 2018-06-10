namespace TokenCalculator.Core.Tokens
{
    public class Token
    {
        public TokenSide FaceUp { get; set; } = new TokenSide();
        public TokenSide FaceDown { get; set; } = new TokenSide();

        public Token Flip()
        {
            return new Token { FaceUp = FaceDown, FaceDown = FaceUp };
        }

        public bool Matches(Token other)
        {
            return FaceUp.Matches(other.FaceUp)
                   && FaceDown.Matches(other.FaceDown);
        }

        public Token Clone()
        {
            return new Token { FaceUp = FaceUp, FaceDown = FaceDown };
        }
    }
}

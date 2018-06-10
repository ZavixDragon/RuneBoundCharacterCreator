namespace TokenCalculator.Core.RawOutcomes.Domain
{
    public class RawTokenResult
    {
        public RawTokenType Type { get; set; }
        public int Quantity { get; set; } = 1;
        public bool WasTactics { get; set; } = false;
        public bool WasDoubled { get; set; } = false;

        public bool Matches(RawTokenResult other)
        {
            return Type == other.Type
                   && WasTactics == other.WasTactics
                   && Quantity == other.Quantity;
        }

        public RawTokenResult TacticsClone()
        {
            return new RawTokenResult
            {
                WasTactics = true,
                Type = Type,
                Quantity = Quantity,
                WasDoubled = WasDoubled
            };
        }

        public RawTokenResult DoubleClone()
        {
            return new RawTokenResult
            {
                Type = Type,
                Quantity = Quantity * 2,
                WasTactics = WasTactics,
                WasDoubled = true
            };
        }

        public RawTokenResult Clone()
        {
            return new RawTokenResult
            {
                Type = Type,
                Quantity = Quantity,
                WasTactics = WasTactics,
                WasDoubled = WasDoubled
            };
        }
    }
}

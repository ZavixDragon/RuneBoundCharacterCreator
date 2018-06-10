using System.Linq;
using TokenCalculator.Core.RawOutcomes.Domain;
using TokenCalculator.Useful;

namespace TokenCalculator.Core.Evaluators
{
    public class BlockEvaluator : IEvaluator
    {
        public decimal OneBlockValue { get; set; }
        public decimal TwoBlockValue { get; set; }
        public decimal ThreeBlockValue { get; set; }
        public decimal FourOrGreaterBlockValue { get; set; }
        public decimal PercentageOneBlockDrop { get; set; }
        public decimal PercentageTwoBlockDrop { get; set; }
        public decimal PercentageThreeBlockDrop { get; set; }
        public decimal PercentageFourBlockDrop { get; set; }

        public decimal PercentageDropForChargeDisynergy { get; set; }

        public decimal CalculateValue(RawOutcome outcome)
        {
            var blocks = outcome.Tokens.Where(x => x.Type == RawTokenType.Block).OrderBy(x => x.Quantity).ToList();
            if (!blocks.Any())
                return 0;
            var percentage = 1m;
            if (outcome.Tokens.Any(x => x.Type == RawTokenType.Charge))
                percentage -= percentage * (PercentageDropForChargeDisynergy / 100);
            blocks.Skip(1).Where(x => x.Quantity == 1).ForEach(x => percentage -= percentage * (PercentageOneBlockDrop / 100));
            blocks.Skip(1).Where(x => x.Quantity == 2).ForEach(x => percentage -= percentage * (PercentageTwoBlockDrop / 100));
            blocks.Skip(1).Where(x => x.Quantity == 3).ForEach(x => percentage -= percentage * (PercentageThreeBlockDrop / 100));
            blocks.Skip(1).Where(x => x.Quantity >= 4).ForEach(x => percentage -= percentage * (PercentageFourBlockDrop / 100));
            var result = 0m;
            blocks.Where(x => x.Quantity == 1).ForEach(x => result += OneBlockValue);
            blocks.Where(x => x.Quantity == 2).ForEach(x => result += TwoBlockValue);
            blocks.Where(x => x.Quantity == 3).ForEach(x => result += ThreeBlockValue);
            blocks.Where(x => x.Quantity >= 4).ForEach(x => result += FourOrGreaterBlockValue);
            return result * percentage;
        }
    }
}

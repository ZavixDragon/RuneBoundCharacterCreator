using System;
using System.Collections.Generic;
using System.Linq;
using TokenCalculator.Core.Calculations;
using TokenCalculator.Core.Evaluators;
using TokenCalculator.Core.Tokens;
using TokenCalculator.Cost;
using TokenCalculator.Useful;

namespace TokenCalculator.Generator
{
    public class TokenSetGenerator
    {
        public GeneratorResults Get()
        {
            var evaluators = new List<IEvaluator>
            {
                new BlockEvaluator
                {
                    OneBlockValue = 4.1m,
                    TwoBlockValue = 7.2m,
                    ThreeBlockValue = 9.3m,
                    FourOrGreaterBlockValue = 11.4m,
                    PercentageDropForChargeDisynergy = 20,
                    PercentageOneBlockDrop = 10,
                    PercentageTwoBlockDrop = 15,
                    PercentageThreeBlockDrop = 18,
                    PercentageFourBlockDrop = 21,
                },
                new ChargeEvaluator
                {
                    ChargeValue = 5.01m,
                    NoNormalDamageBonus = 3
                },
                new DamageEvaluator
                {
                    DamageValue = 5,
                    FirstDamagePenalty = 3,
                    SecondDamagePenalty = 1,
                    AtLeastTwoDamageTokenOneTimePenalty = 0.04m,
                    AtLeastThreeDamageHighestTokenOneTimePenalty = 0.03m,
                },
                new InitiativeEvaluator
                {
                    FirstInitiativeValue = 0,
                    SecondInitiativeValue = 0,
                    ThirdInitiativeValue = 0,
                    FourthInitiativeValue = 0,
                    FifthInitiativeValue = 0
                },
                new OpenDoubleEvaluator
                {
                    OpenDoubleValue = 0.01m,
                    DiminishingPercentageMultiplier = 90,
                },
                new OpenTacticsEvaluator
                {
                    OpenTacticsValue = 9,
                    DiminishingMultiplierPercentage = 35
                },
                new OpenWingsEvaluator
                {
                    OpenWingsValue = 3.3m,
                },
                new SurgeEvaluator
                {
                    SurgeTokenValue = 0
                },
                new UsedTacticsEvaluator
                {
                    UsedTacticsValue = 4.01m,
                }
            };

            var uniqueSides = new List<TokenSide>();
            Enum.GetValues(typeof(TokenSideType)).Cast<TokenSideType>()
                .ForEach(x =>
                {
                    uniqueSides.Add(new TokenSide { Type = x });
                    if (x == TokenSideType.Damage || x == TokenSideType.Block || x == TokenSideType.Charge || x == TokenSideType.Surge) 
                        uniqueSides.Add(new TokenSide { Type = x, Quantity = 2 });
                });

            var uniqueTokens = new List<Token>();
            for (var side1 = 0; side1 < uniqueSides.Count - 1; side1++)
                for (var side2 = side1 + 1; side2 < uniqueSides.Count; side2++)
                    uniqueTokens.Add(new Token { FaceUp = uniqueSides[side1], FaceDown = uniqueSides[side2] });
            uniqueTokens = uniqueTokens.Where(x => !(x.FaceUp.Quantity == 2 && x.FaceDown.Quantity == 2) 
                                                   && !(x.FaceUp.Type == TokenSideType.Tactics && x.FaceDown.Quantity == 2)
                                                   && !(x.FaceUp.Quantity == 2 && x.FaceDown.Type == TokenSideType.Tactics)
                                                   && !(x.FaceUp.Quantity == 2 && x.FaceUp.Type == TokenSideType.Charge && x.FaceDown.Type != TokenSideType.Blank)
                                                   && !(x.FaceDown.Quantity == 2 && x.FaceDown.Type == TokenSideType.Charge && x.FaceUp.Type != TokenSideType.Blank)).ToList();

            var results = new List<GeneratorResult>();
            for (var token1 = 0; token1 < uniqueTokens.Count; token1++)
            {
                Console.WriteLine(token1 + 1);
                for (var token2 = token1; token2 < uniqueTokens.Count; token2++)
                    for (var token3 = token2; token3 < uniqueTokens.Count; token3++)
                        for (var token4 = token3; token4 < uniqueTokens.Count; token4++)
                        {
                            var tokens = new List<Token>
                            {
                                uniqueTokens[token1],
                                uniqueTokens[token2],
                                uniqueTokens[token3],
                                uniqueTokens[token4]
                            };
                            var result = new TokenSetWithCostCalculation(tokens, evaluators).Get();
                            results.Add(new GeneratorResult
                            {
                                Cost = result.Cost,
                                Tokens = tokens,
                                Stats = result.TokenSet
                            });
                        }
            }
            Console.WriteLine(results.Count);

            var highestValue = results.OrderByDescending(x => x.Stats.Value.Avg).First();
            var highestDamage = results.OrderByDescending(x => x.Stats.Damage.Avg).ThenByDescending(x => x.Stats.Value.Avg).First();
            var minimumDamage = results.OrderByDescending(x => x.Stats.Damage.Min).ThenByDescending(x => x.Stats.Value.Avg).First();
            var highestEconomy = results.OrderBy(x =>
            {
                if (x.Stats.Value.Avg == 0)
                    return 99;
                return x.Cost / x.Stats.Value.Avg;
            }).First();

            return new GeneratorResults
            {
                HighestValue = highestValue,
                HighestDamage = highestDamage,
                HighestMinimumDamage = minimumDamage,
                HighestEconomy = highestEconomy
            };
        }
    }

    public class GeneratorResults
    {
        public GeneratorResult HighestValue { get; set; }
        public GeneratorResult HighestDamage { get; set; }
        public GeneratorResult HighestEconomy { get; set; }
        public GeneratorResult HighestMinimumDamage { get; set; }
    }

    public class GeneratorResult
    {
        public int Cost { get; set; }
        public List<Token> Tokens { get; set; }
        public TokenSetResult Stats { get; set; }
    }
}

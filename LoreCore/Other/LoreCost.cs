using ItemChanger;

namespace LoreCore.Other;

public record LoreCost : Cost
{
    public int NeededLore { get; set; }

    public override bool CanPay() => ElderbugPlacement.Instance == null || ElderbugPlacement.Instance.AcquiredLore >= NeededLore;

    public override string GetCostText() => NeededLore + " Lore";

    public override bool HasPayEffects() => false;

    public override void OnPay() { } 
}

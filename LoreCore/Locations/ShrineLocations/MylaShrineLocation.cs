namespace LoreCore.Locations.ShrineLocations;

public class MylaShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of killing an infected girl. Put her out of her misery please.";

    #endregion

    #region Event handler

    private void HealthManager_Die(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion)
    {
        orig(self, attackDirection, attackType, ignoreEvasion);
        if (self.name.Contains("Zombie Myla"))
            ConditionMet = true;
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HealthManager.Die += HealthManager_Die;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HealthManager.Die -= HealthManager_Die;
    }

    #endregion
}

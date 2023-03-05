namespace LoreCore.Locations.ShrineLocations;

public class MenderbugShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of killing the only happy bug in this entire game... :c";

    #endregion

    #region Event handler

    private void HealthManager_Die(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion)
    {
        orig(self, attackDirection, attackType, ignoreEvasion);
        if (self.gameObject.name.StartsWith("Mender"))
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

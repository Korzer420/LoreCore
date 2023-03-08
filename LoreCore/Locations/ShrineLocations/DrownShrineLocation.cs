namespace LoreCore.Locations.ShrineLocations;

public class DrownShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of letting nature take care of a foe.";

    #endregion

    #region Event handler

    private void HealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
    {
        orig(self, hitInstance);
        if (hitInstance.DamageDealt >= 999)
            ConditionMet = true;
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HealthManager.TakeDamage += HealthManager_TakeDamage;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HealthManager.TakeDamage -= HealthManager_TakeDamage;
    }

    #endregion
}

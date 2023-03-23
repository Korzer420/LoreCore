using KorzUtils.Helper;

namespace LoreCore.Locations.ShrineLocations;

public class ShadeKillShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of being killed by your own past.";

    #endregion

    #region Event handler

    private void HeroController_TakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, UnityEngine.GameObject go, GlobalEnums.CollisionSide damageSide, int damageAmount, int hazardType)
    {
        orig(self, go, damageSide, damageAmount, hazardType);
        if (PlayerData.instance.GetInt(nameof(PlayerData.instance.health)) <= 0 && (go?.name.StartsWith("Hollow Shade") == true
            || go?.name.StartsWith("Shadow Ball") == true || go?.transform.parent?.name.StartsWith("Hollow Shade") == true))
            ConditionMet = true;
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HeroController.TakeDamage += HeroController_TakeDamage;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HeroController.TakeDamage -= HeroController_TakeDamage;
    }

    #endregion
}

using KorzUtils.Helper;

namespace LoreCore.Locations.ShrineLocations;

internal class SlugInTubShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of SLUG IN TUB";

    #endregion

    #region Event handler

    private bool HeroController_TryAddMPChargeSpa(On.HeroController.orig_TryAddMPChargeSpa orig, HeroController self, int amount)
    {
        if (HeroController.instance.cState.focusing && CharmHelper.EquippedCharm(KorzUtils.Enums.CharmRef.ShapeOfUnn))
            ConditionMet = true;
        return orig(self, amount);
    }

    #endregion

    #region Controls

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HeroController.TryAddMPChargeSpa += HeroController_TryAddMPChargeSpa;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HeroController.TryAddMPChargeSpa -= HeroController_TryAddMPChargeSpa;
    }

    #endregion
}

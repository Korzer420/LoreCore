using System;

namespace LoreCore.Locations.ShrineLocations;

public class GeoShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of \"spending\" 10 thousand geo.";

    public int SpendGeo { get; set; }

    #endregion

    #region Event handler

    private void HeroController_TakeGeo(On.HeroController.orig_TakeGeo orig, HeroController self, int amount)
    {
        orig(self, amount);
        SpendGeo = Math.Min(SpendGeo + amount, 10000);
        ConditionMet = SpendGeo >= 10000;
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HeroController.TakeGeo += HeroController_TakeGeo;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HeroController.TakeGeo -= HeroController_TakeGeo;
    }

    #endregion
}

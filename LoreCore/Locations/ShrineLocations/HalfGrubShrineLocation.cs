using Modding;

namespace LoreCore.Locations.ShrineLocations;

public class HalfGrubShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of return half of the grubs.";

    #endregion

    #region Event handler

    private int ModHooks_SetPlayerIntHook(string name, int orig)
    {
        if (name == nameof(PlayerData.instance.grubsCollected))
            ConditionMet = orig >= 23;
        return orig;
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        ModHooks.SetPlayerIntHook += ModHooks_SetPlayerIntHook;
    }

    protected override void OnUnload()
    {
        ModHooks.SetPlayerIntHook -= ModHooks_SetPlayerIntHook;
        base.OnUnload();
    }

    #endregion
}

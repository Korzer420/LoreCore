using Modding;

namespace LoreCore.Locations.ShrineLocations;

public class AllGrubShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of returning all grubs.";

    #endregion

    #region Event handler

    private int ModHooks_SetPlayerIntHook(string name, int orig)
    {
        if (name == nameof(PlayerData.instance.grubsCollected))
            ConditionMet = orig >= 46;
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

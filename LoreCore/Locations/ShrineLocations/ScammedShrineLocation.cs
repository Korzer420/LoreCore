using Modding;

namespace LoreCore.Locations.ShrineLocations;

internal class ScammedShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of a wrong financial investment.";

    #endregion

    #region Event handler

    private int ModHooks_SetPlayerIntHook(string name, int orig)
    {
        if (name == nameof(PlayerData.instance.bankerTheft) && !ConditionMet)
            ConditionMet = orig != 0;
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

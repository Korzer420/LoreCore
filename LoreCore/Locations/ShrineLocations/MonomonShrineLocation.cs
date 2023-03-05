using Modding;

namespace LoreCore.Locations.ShrineLocations;

public class MonomonShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of Monomon."; 

    #endregion

    #region Event handler

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        if (name == nameof(PlayerData.instance.monomonDefeated))
            ConditionMet = orig;
        return orig;
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        ModHooks.SetPlayerBoolHook += ModHooks_SetPlayerBoolHook;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        ModHooks.SetPlayerBoolHook -= ModHooks_SetPlayerBoolHook;
    }

    #endregion
}

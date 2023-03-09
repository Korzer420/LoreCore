using Modding;

namespace LoreCore.Locations.ShrineLocations;

public class NailsmithShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of bringing the nailsmith to its destiny.";

    #endregion

    #region Event handler

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        if (orig && (name == nameof(PlayerData.instance.nailsmithKilled) || name == nameof(PlayerData.instance.nailsmithSpared)))
            ConditionMet = true;
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
        ModHooks.SetPlayerBoolHook -= ModHooks_SetPlayerBoolHook;
        base.OnUnload();
    }

    #endregion
}

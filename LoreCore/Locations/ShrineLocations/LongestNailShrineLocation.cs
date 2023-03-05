using KorzUtils.Enums;
using KorzUtils.Helper;
using Modding;

namespace LoreCore.Locations.ShrineLocations;

public class LongestNailShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of the longest nail.";

    #endregion

    #region Event handler

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        if (name == nameof(PlayerData.instance.equippedCharm_18))
            ConditionMet = orig && CharmHelper.EquippedCharm(CharmRef.MarkOfPride);
        else if (name == nameof(PlayerData.instance.equippedCharm_13))
            ConditionMet = orig && CharmHelper.EquippedCharm(CharmRef.Longnail);
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

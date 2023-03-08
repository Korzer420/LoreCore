using Modding;

namespace LoreCore.Locations.SpecialLocations;

internal class MillibelleLocation : DialogueLocation
{
    #region Event handler

    private int ModHooks_SetPlayerIntHook(string name, int orig)
    {
        if (name == "bankerBalance" && orig <= 0)
        {
            PlayerData.instance.SetInt(nameof(PlayerData.instance.bankerTheft), 0);
            PlayerData.instance.SetBool(nameof(PlayerData.instance.bankerTheftCheck), false);
        }
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
        base.OnUnload();
        ModHooks.SetPlayerIntHook -= ModHooks_SetPlayerIntHook;
    } 

    #endregion
}

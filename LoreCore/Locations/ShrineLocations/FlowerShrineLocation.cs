using Modding;
using System.Linq;

namespace LoreCore.Locations.ShrineLocations;

public class FlowerShrineLocation : ShrineLocation
{
    #region Members

    private readonly string[] _flowerKeys = new string[]
    {
        nameof(PlayerData.instance.elderbugGaveFlower),
        nameof(PlayerData.instance.givenEmilitiaFlower),
        nameof(PlayerData.instance.givenGodseekerFlower),
        nameof(PlayerData.instance.givenOroFlower),
        nameof(PlayerData.instance.givenWhiteLadyFlower)
    };

    #endregion

    #region Properties

    public override string Text => "It dreams of kindness, formed as a flower... twice.";

    public int GivenFlowers { get; set; }

    #endregion


    #region Event handler

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        if (_flowerKeys.Contains(name))
        {
            GivenFlowers++;
            ConditionMet = GivenFlowers >= 2;
        }
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

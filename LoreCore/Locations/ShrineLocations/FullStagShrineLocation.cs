using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoreCore.Locations.ShrineLocations;

public class FullStagShrineLocation : ShrineLocation
{
    #region Members

    private readonly string[] _stagKeys = new string[]
    {
        nameof(PlayerData.instance.openedCrossroads),
        nameof(PlayerData.instance.openedGreenpath),
        nameof(PlayerData.instance.openedHiddenStation),
        nameof(PlayerData.instance.openedRestingGrounds),
        nameof(PlayerData.instance.openedRoyalGardens),
        nameof(PlayerData.instance.openedRuins1),
        nameof(PlayerData.instance.openedRuins2),
        nameof(PlayerData.instance.openedDeepnest),

    };

    #endregion

    #region Properties

    public override string Text => "It dreams of travel on the bug everywhere.";

    public int StagCount { get; set; }

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

    private bool ModHooks_SetPlayerBoolHook(string name, bool orig)
    {
        if (orig && _stagKeys.Contains(name))
        {
            StagCount++;
            ConditionMet = StagCount >= _stagKeys.Length;
        }    
        return orig;
    }

    #endregion
}

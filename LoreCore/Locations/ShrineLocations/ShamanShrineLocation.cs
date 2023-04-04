using Modding;
using System.Linq;

namespace LoreCore.Locations.ShrineLocations;

public class ShamanShrineLocation : ShrineLocation
{
    #region Members

    private readonly string[] _shamanKeys = new string[]
    {
        "SHAMAN_FIREBALL2",
        "SHAMAN_SCREAM",
        "SHAMAN_SCREAM2",
        "SHAMAN_QUAKE",
        "SHAMAN_QUAKE2"
    };

    #endregion

    #region Properties

    public override string Text => "It dreams of having a small talk with the snail shaman.";

    public int DialogueCount { get; set; }

    #endregion

    #region Event handler

    private string ModHooks_LanguageGetHook(string key, string sheetTitle, string orig)
    {
        if (_shamanKeys.Contains(key))
        {
            DialogueCount++;
            ConditionMet = DialogueCount >= _shamanKeys.Length;
        }
        return orig;
    }

    #endregion

    #region Controls

    protected override void OnLoad()
    {
        base.OnLoad();
        ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        ModHooks.LanguageGetHook -= ModHooks_LanguageGetHook;
    }

    #endregion
}

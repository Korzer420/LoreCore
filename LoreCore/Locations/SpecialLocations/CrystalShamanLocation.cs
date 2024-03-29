using ItemChanger;
using KorzUtils.Helper;

namespace LoreCore.Locations.SpecialLocations;

/// <summary>
/// The dream dialogue location of the crystalized shaman
/// </summary>
internal class CrystalShamanLocation : DreamNailLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(sceneName, new("Crystal Shaman", "Control"), SpawnShiny);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Events.RemoveFsmEdit(sceneName, new("Crystal Shaman", "Control"), SpawnShiny);
    }

    private void SpawnShiny(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained())
            return;
        fsm.GetState("Broken").AddActions(() => ItemHelper.SpawnShiny(fsm.transform.position, Placement));
    }
}

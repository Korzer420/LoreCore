using ItemChanger;
using ItemChanger.FsmStateActions;
using KorzUtils.Helper;
using LoreCore.Enums;
using LoreCore.Modules;
using System.Linq;

namespace LoreCore.Locations.SpecialLocations;

/// <summary>
/// Again, a stitched location from a dream nail and traveller location.
/// </summary>
internal class TisoCorpseLocation : DreamNailLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(sceneName, new("tiso_corpse", "FSM"), ControlCorpseSpawn);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Events.RemoveFsmEdit(sceneName, new("tiso_corpse", "FSM"), ControlCorpseSpawn);
    }

    private void ControlCorpseSpawn(PlayMakerFSM fsm)
    {
        fsm.GetState("Check").RemoveAllActions();
        fsm.GetState("Check").AddActions(() =>
        {
            if (TravellerControlModule.CurrentModule.Stages[Traveller.Tiso] < 4 || Placement.Items.All(x => x.IsObtained()))
                fsm.SendEvent("DEACTIVATE");
        });
    }
}

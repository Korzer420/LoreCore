using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;

namespace LoreCore.Locations;

internal class GhostDialogueLocation : DialogueLocation
{
    public string GhostName { get; set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(new FsmID(ObjectName, "ghost_npc_death"), (a) => { });
    }

    private void SpawnShiny(PlayMakerFSM fsm)
    {
        fsm.GetState("Destroy").AddLastAction(new Lambda(() =>
        {

        }));
    }
}

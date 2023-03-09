using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using KorzUtils.Helper;
using LoreCore.Items;

namespace LoreCore.Locations;

internal class GhostDialogueLocation : DialogueLocation
{
    public string GhostName { get; set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(sceneName, new FsmID(ObjectName, "ghost_npc_death"), SpawnShiny);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(sceneName, new FsmID(ObjectName, "ghost_npc_death"), SpawnShiny);
        base.OnUnload();
    }

    private void SpawnShiny(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained() || !ListenItem.CanListen || fsm.gameObject.scene.name != sceneName)
            return;
        fsm.GetState("Destroy").AddLastAction(new Lambda(() =>
        {
            ItemHelper.SpawnShiny(fsm.transform.position, Placement);
        }));
    }
}

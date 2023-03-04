using ItemChanger;

namespace LoreCore.Locations;

internal class GhostDialogueLocation : DialogueLocation
{
    public string GhostName { get; set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        On.PlayMakerFSM.OnEnable += PreventGhostDeath;
        Events.AddFsmEdit(new FsmID(null, "ghost_npc_death"), (a) => { });
    }

    private void PreventGhostDeath(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
    {
        
        orig(self);
    }
}

using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Items;
using LoreCore.Modules;

namespace LoreCore.Locations.SpecialLocations;

internal class ClothGardenLocation : AutoLocation
{
    protected override void OnLoad()
    {
        Events.AddFsmEdit(new("Cloth Ghost NPC", "Conversation Control"), ModifyClothGhost);
        Events.AddFsmEdit(new("Cloth Ghost NPC", "ghost_npc_death"), SpawnShiny);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(new("Cloth Ghost NPC", "Conversation Control"), ModifyClothGhost);
        Events.RemoveFsmEdit(new("Cloth Ghost NPC", "ghost_npc_death"), SpawnShiny);
    }

    private void ModifyClothGhost(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained() || !ListenItem.CanListen || TravellerControlModule.CurrentModule.Stages[Enums.Traveller.Cloth] < 5)
            return;
        FsmState state = fsm.GetState("Hero Anim");
        state.AddActions(() => fsm.GetState("Idle").ClearTransitions());
        state.AddActions(new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
        {
            FlingType = flingType,
            Container = Container.Tablet,
            MessageType = MessageType.Any,
        }, callback), "CONVO_FINISH"));
        state.ClearTransitions();
        state.AddTransition("CONVO_FINISH", "Talk Finish");
    }

    private void SpawnShiny(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained() || !ListenItem.CanListen || TravellerControlModule.CurrentModule.Stages[Enums.Traveller.Cloth] < 5)
            return;
        fsm.GetState("Destroy").AddActions(() => ItemHelper.SpawnShiny(fsm.transform.position, Placement));
    }
}

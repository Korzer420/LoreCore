using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Items;

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
        if (Placement.AllObtained() || !ListenItem.CanListen || TravellerLocation.Stages[Enums.Traveller.Cloth] < 5)
            return;
        FsmState state = fsm.GetState("Hero Anim");
        state.AddLastAction(new Lambda(() =>
        {
            fsm.GetState("Idle").ClearTransitions();
        }));
        state.AddLastAction(new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
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
        if (Placement.AllObtained() || !ListenItem.CanListen || TravellerLocation.Stages[Enums.Traveller.Cloth] < 5)
            return;
        fsm.GetState("Destroy").AddLastAction(new Lambda(() => ItemHelper.SpawnShiny(fsm.transform.position, Placement)));
    }
}

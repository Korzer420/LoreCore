using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Items;

namespace LoreCore.Locations.SpecialLocations;

internal class BrettaLocation : AutoLocation
{
    protected override void OnLoad()
    {
        Events.AddFsmEdit(new("Bretta Dazed", "Conversation Control"), SkipBrettaFlag);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(new("Bretta Dazed", "Conversation Control"), SkipBrettaFlag);
    }

    private void SkipBrettaFlag(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained())
        {
            // Force Bretta to be disabled. Her flag will be set by the item, rather than her location.
            fsm.GetState("Active?").AdjustTransition("FINISHED", "Destroy");
            return;
        }
        if (!ListenItem.CanListen)
        {
            fsm.gameObject.LocateMyFSM("npc_control").GetState("Idle").ClearTransitions();
            return;
        }
        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Give Items",
            Actions = new HutongGames.PlayMaker.FsmStateAction[]
            {
                new Lambda(() => fsm.gameObject.LocateMyFSM("npc_control").GetState("Idle").ClearTransitions()),
                new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new()
                {
                     MessageType = MessageType.Any,
                     FlingType = FlingType.DirectDeposit,
                     Container = Container.Tablet
                }, callback),"CONVO_FINISH")
            }
        });

        fsm.GetState("Idle").AdjustTransition("CONVO START", "Give Items");
        fsm.GetState("Give Items").AddTransition("CONVO_FINISH", "Talk Finish");
    }
}

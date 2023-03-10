using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Items;
using System.Linq;

namespace LoreCore.Locations.SpecialLocations;

internal class HiddenMothLocation : AutoLocation
{
    protected override void OnLoad()
    {
        Events.AddFsmEdit(new("Inspect Region", "Convo"), ModifyHiddenMoth);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(new("Inspect Region", "Convo"), ModifyHiddenMoth);
    }

    private void ModifyHiddenMoth(PlayMakerFSM fsm)
    {
        if (!ListenItem.CanListen)
        {
            fsm.GetState("Idle").ClearTransitions()
            return;
        }
        if (fsm.gameObject.scene.name != "Dream_Backer_Shrine" || Placement.Items.All(x => x.IsObtained()))
            return;
        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Give Items",
            Actions = new HutongGames.PlayMaker.FsmStateAction[]
            {
                new Lambda(() => fsm.GetState("Idle").ClearTransitions()),
                new AsyncLambda(callback =>
                    ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                    {
                        FlingType = flingType,
                        Container = Container.Tablet,
                        MessageType = MessageType.Any,
                    }, callback), "CONVO_FINISH")
            }
        });
        fsm.GetState("Box Up").AdjustTransition("FINISHED", "Give Items");
        fsm.GetState("Give Items").AddTransition("CONVO_FINISH", "Look Up End?");
    }
}

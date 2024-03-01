using HutongGames.PlayMaker;
using ItemChanger;
using KorzUtils.Helper;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using System.Linq;

namespace LoreCore.Locations;

public class StagEggLocation : AutoLocation
{
    protected override void OnLoad()
    {
        Events.AddFsmEdit(sceneName, new("Eggs Inspect", "Conversation Control"), ModifyEgg);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(sceneName, new("Eggs Inspect", "Conversation Control"), ModifyEgg);
    }

    private void ModifyEgg(PlayMakerFSM fsm)
    {
        FsmState currentWorkingState = fsm.GetState("Idle");
        if (Placement.Items.All(x => x.IsObtained()))
        {
            currentWorkingState.ClearTransitions();
            return;
        }
        currentWorkingState = new FsmState(fsm.Fsm)
        {
            Name = "Skip",
            Actions = new FsmStateAction[]
            {
                new Lambda(() => PlayerData.instance.SetBool("stagEggInspected", true)),
                new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                {
                    FlingType = flingType,
                    Container = Container.Tablet,
                    MessageType = MessageType.Any,
                }, callback), "CONVO FINISHED")
            }
        };
        currentWorkingState.AddTransition("CONVO FINISHED", "Talk Finish");
        fsm.AddState(currentWorkingState);
        currentWorkingState = fsm.GetState("Hero Anim");
        currentWorkingState.ClearTransitions();
        currentWorkingState.AddTransition("FINISHED", "Skip");
    }
}

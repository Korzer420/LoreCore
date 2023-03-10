using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Items;
using System.Linq;

namespace LoreCore.Locations;

/// <summary>
/// A location which gives items in a normal conversation.
/// </summary>
public class DialogueLocation : AutoLocation
{
    #region Properties

    public string ObjectName { get; set; }

    public string FsmName { get; set; }

    #endregion

    protected override void OnLoad()
    {
        Events.AddFsmEdit(sceneName, new(ObjectName, FsmName), SkipDialog);
        if (name == LocationList.Dung_Defender)
            Events.AddFsmEdit(sceneName, new(ObjectName, "FSM"), x => x.GetState("Check").ClearTransitions());
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(sceneName, new(ObjectName, FsmName), SkipDialog);
        if (name == LocationList.Dung_Defender)
            Events.RemoveFsmEdit(sceneName, new(ObjectName, "FSM"), x => x.GetState("Check").ClearTransitions());
    }

    private void SkipDialog(PlayMakerFSM fsm)
    {
        try
        {
            if (Placement.Items.All(x => x.IsObtained()))
                return;
            if (!ListenItem.CanListen)
            {
                fsm.gameObject.LocateMyFSM("npc_control").GetState("Idle").ClearTransitions();
                    return;
            }
            if (fsm.GetState("Give Items") is null)
            {
                FsmState startState;
                string transitionEnd;
                if (string.Equals(ObjectName, "Queen", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    startState = fsm.GetState("NPC Anim");
                    transitionEnd = "Summon";
                }
                else
                {
                    startState = fsm.GetState("Hero Anim");
                    if (startState == null)
                        startState = fsm.GetState("Hero Look");
                    // Zote Greenpath/City
                    if (startState == null)
                        startState = fsm.GetState("Talk Back");
                    if (startState == null)
                        startState = fsm.GetState("Hero Look");

                    // Dung defender and the grave function differently
                    if (fsm.gameObject.name == "Dung Defender NPC")
                        transitionEnd = "Box Down Event";
                    else if (fsm.transform.parent?.name == "Mantis Grave"
                        && PlayerData.instance.GetBool(nameof(PlayerData.instance.hasXunFlower))
                        && !PlayerData.instance.GetBool(nameof(PlayerData.instance.xunFlowerBroken)))
                        transitionEnd = "Kneel";
                    else
                        transitionEnd = "Talk Finish";
                }

                fsm.AddState(new FsmState(fsm.Fsm)
                {
                    Name = "Give Items",
                    Actions = new FsmStateAction[]
                    {
                        new Lambda(() =>
                        {
                            PlayMakerFSM control = fsm.gameObject.LocateMyFSM("npc_control");
                            control.GetState("Idle").ClearTransitions();
                        }),
                        new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                        {
                            FlingType = flingType,
                            Container = Container.Tablet,
                            MessageType = name == LocationList.Dreamer_Tablet ? MessageType.Big : MessageType.Any,
                        }, callback), "CONVO_FINISH")
                    }
                });

                // Zote Dirtmouth 2....
                if (startState == null)
                {
                    startState = fsm.GetState("Idle");
                    startState.AdjustTransition(startState.Transitions[0].EventName, "Give Items");
                }
                else
                    startState.AdjustTransition(startState.Transitions[0].EventName, "Give Items");
                // As if Zote wasn't annoying enough... (Dung Defender as well)
                if (fsm.GetState("Talk R") is FsmState state)
                    state.AdjustTransition("FINISHED", "Give Items");
                if (fsm.GetState("Check Active") is FsmState state2)
                    state2.Actions = new FsmStateAction[1]
                    {
                        new Lambda(() =>
                        {
                            fsm.SendEvent(fsm.gameObject.name != "Dung Defender NPC" || PlayerData.instance.GetBool(nameof(PlayerData.instance.killedDungDefender))
                             ? "FINISHED"
                             : "DESTROY");
                        })
                    };
                fsm.GetState("Give Items").AddTransition("CONVO_FINISH", transitionEnd);
            }
        }
        catch (System.Exception exception)
        {
            LogHelper.Write<LoreCore>("Failed to modify dialogue location.", exception);
        }
    }
}

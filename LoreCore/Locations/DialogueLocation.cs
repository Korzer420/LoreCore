using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Data;
using LoreCore.Enums;
using LoreCore.Items;
using LoreCore.Modules;
using System;
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
        Events.AddFsmEdit(sceneName, new(ObjectName, FsmName), SkipDialogue);
        if (name == LocationList.Dung_Defender)
            Events.AddFsmEdit(sceneName, new(ObjectName, "FSM"), x => x.GetState("Check").ClearTransitions());
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(sceneName, new(ObjectName, FsmName), SkipDialogue);
        if (name == LocationList.Dung_Defender)
            Events.RemoveFsmEdit(sceneName, new(ObjectName, "FSM"), x => x.GetState("Check").ClearTransitions());
    }

    private void SkipDialogue(PlayMakerFSM fsm)
    {
        try
        {
            if (Placement.Items.All(x => x.IsObtained()))
                return;
            if (name == LocationList.City_Fountain || name == LocationList.Traitor_Grave)
            {
                if (!ReadItem.CanRead)
                {
                    fsm.gameObject.LocateMyFSM("npc_control").GetState("Idle").ClearTransitions();
                    return;
                }
            }
            else
            {
                // Extra Hornet checks to make sure the appears.
                // To do: Move this in an extra class.
                if (name == LocationList.Hornet_Temple)
                {
                    if (TravellerControlModule.CurrentModule.Stages[Enums.Traveller.Hornet] >= 5)
                        fsm.GetState("Active?").AdjustTransition("ABSENT", "Idle");
                    else
                    {
                        fsm.GetState("Active?").AdjustTransition("FINISHED", "Absent");
                        fsm.GetState("Active?").AdjustTransition("INACTIVE", "Absent");
                    }
                }
                else if (name == LocationList.Hornet_Deepnest)
                { 
                    if (TravellerControlModule.CurrentModule.Stages[Traveller.Hornet] >= 4)
                        fsm.GetState("Defeated in Outskirts?").AdjustTransition("ABSENT", "Idle");
                    else
                        fsm.GetState("Defeated in Outskirts?").AdjustTransition("FINISHED", "Never Meet");
                }

                if (!ListenItem.CanListen)
                {
                    fsm.gameObject.LocateMyFSM("npc_control").GetState("Idle").ClearTransitions();
                    return;
                }
            }
            ModifyDialogue(fsm, Placement);
        }
        catch (System.Exception exception)
        {
            LogHelper.Write<LoreCore>("Failed to modify dialogue location.", exception);
        }
    }

    internal static void ModifyDialogue(PlayMakerFSM fsm, AbstractPlacement placement)
    {
        try
        {
            if (fsm.GetState("Give Items") is null)
            {
                FsmState startState;
                string transitionEnd;
                if (string.Equals(fsm.gameObject.name, "Queen", System.StringComparison.CurrentCultureIgnoreCase))
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
                        new AsyncLambda(callback => ItemUtility.GiveSequentially(placement.Items, placement, new GiveInfo
                        {
                            FlingType = FlingType.DirectDeposit,
                            Container = Container.Tablet,
                            MessageType = placement.Name == LocationList.Dreamer_Tablet ? MessageType.Big : MessageType.Any,
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
        catch (Exception)
        {
            LogHelper.Write<LoreCore>($"Failed to modify dialogue for placement {placement.Name}");
            throw;
        }
    }
}

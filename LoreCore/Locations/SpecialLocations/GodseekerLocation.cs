using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Util;
using KorzUtils.Helper;
using Modding;
using MonoMod.Cil;
using System;

namespace LoreCore.Locations.SpecialLocations;

internal class GodseekerLocation : DialogueLocation
{
    #region Event handler

    private bool ForceFlukeHermitBack(string name, bool orig)
    {
        if (name == nameof(PlayerData.instance.scaredFlukeHermitReturned)
            || name == nameof(PlayerData.instance.scaredFlukeHermitEncountered))
            return true;
        return orig;
    }

    #endregion
    
    protected override void OnLoad()
    {
        base.OnLoad();
        IL.BossDoorCompletionStates.Start += BossDoorCompletionStates_Start;
        ModHooks.GetPlayerBoolHook += ForceFlukeHermitBack;
        Events.AddFsmEdit(new("Godseeker Dazed", "Conversation Control"), ModifyAdditionalState);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        IL.BossDoorCompletionStates.Start -= BossDoorCompletionStates_Start;
        ModHooks.GetPlayerBoolHook -= ForceFlukeHermitBack;
        Events.RemoveFsmEdit(new("Godseeker Dazed", "Conversation Control"), ModifyAdditionalState);
    }

    private void BossDoorCompletionStates_Start(ILContext il)
    {
        ILCursor iLCursor = new(il);
        iLCursor.Goto(0);
        iLCursor.GotoNext(MoveType.After, x => x.MatchLdfld<BossSequenceDoor.Completion>("completed"));
        iLCursor.EmitDelegate<Func<bool, bool>>(x => PlayerData.instance.GetBool("godseekerUnlocked"));
    }

    private void ModifyAdditionalState(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained())
            return;
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
                            MessageType = MessageType.Any,
                        }, callback), "CONVO_FINISH")
                    }
        });
        fsm.GetState("Hero Anim").AdjustTransition("FINISHED", "Give Items");
        fsm.GetState("Give Items").AddTransition("CONVO_FINISH", "Talk Finish");
    }
}

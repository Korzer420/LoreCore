using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using KorzUtils.Helper;

namespace LoreCore.Items;

internal class ListenItem : AbstractItem
{
    #region Properties

    public static ListenItem Instance { get; set; }

    public static bool CanListen => Instance == null || Instance.IsObtained();

    public override void GiveImmediate(GiveInfo info) { }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        Instance = this;
        On.PlayMakerFSM.OnEnable += PlayMakerFSM_OnEnable;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.PlayMakerFSM.OnEnable -= PlayMakerFSM_OnEnable;
        Instance = null;
    }

    private void PlayMakerFSM_OnEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
    {
        if (!IsObtained())
        {
            if ((string.Equals(self.FsmName, "npc_control")
            && self.FsmVariables.FindFsmString("Prompt Name")?.Value == "Listen") || self.gameObject.name == "Stag" && self.FsmName == "Stag Control")
            {
                // There are a few exceptions with npc which we want to ignore.
                if (self.gameObject.name != "Moth NPC" && !self.gameObject.name.Contains("Shaman"))
                {
                    if (self.GetState("In Range") is FsmState state)
                    {
                        if (state.GetFirstActionOfType<ShowPromptMarker>() is ShowPromptMarker marker)
                            marker.labelName.Value = "???";
                        if (self.GetState("In Range Turns") is FsmState secondState)
                            secondState.GetFirstActionOfType<ShowPromptMarker>().labelName.Value = "???";
                        self.AddState(new FsmState(self.Fsm)
                        {
                            Name = "Block Interaction",
                            Actions = new FsmStateAction[]
                            {
                                new Lambda(() =>
                                {
                                    self.SendEvent(!IsObtained() ? "FINISHED" : "UP PRESSED");
                                })
                            }
                        });
                        state.AdjustTransition("UP PRESSED", "Block Interaction");
                        self.GetState("Block Interaction").AddTransition("UP PRESSED", "Can Talk?");
                        self.GetState("Block Interaction").AddTransition("FINISHED", "Cancel Frame");
                    }
                    else
                        self.GetState("Idle").ClearTransitions();
                }
            }
            else if (string.Equals(self.FsmName, "Shop Region"))
                self.GetState("Out Of Range").ClearTransitions();
        }
        orig(self);
    }

    #endregion
}

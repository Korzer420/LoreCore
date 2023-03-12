using HutongGames.PlayMaker;
using InControl;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using KorzUtils.Helper;
using System.Linq;

namespace LoreCore.Items;

internal class ReadItem : AbstractItem
{
    #region Members

    private readonly string[] _loreKeys = new string[]
    {
        "TUT_TAB_01",
        "TUT_TAB_02",
        "TUT_TAB_03",
        "PILGRIM_TAB_01",
        "PILGRIM_TAB_02",
        "COMPLETION_RATE_UNLOCKED",
        "MENDERBUG",
        "GREEN_TABLET_01",
        "GREEN_TABLET_02",
        "GREEN_TABLET_03",
        "GREEN_TABLET_05",
        "GREEN_TABLET_06",
        "GREEN_TABLET_07",
        "FUNG_TAB_01",
        "FUNG_TAB_02",
        "FUNG_TAB_03",
        "FUNG_TAB_04",
        "MANTIS_PLAQUE_01",
        "MANTIS_PLAQUE_02",
        "RUIN_TAB_01",
        "RUINS_MARISSA_POSTER",
        "MAGE_COMP_01",
        "MAGE_COMP_03",
        "LURIAN_JOURNAL",
        "DUNG_DEF_SIGN",
        "CLIFF_TAB_02",
        "ARCHIVE_01",
        "ARCHIVE_02",
        "ARCHIVE_03",
        "ABYSS_TUT_TAB_01",
        "MR_MUSH_RIDDLE_TAB_NORMAL",
        "WP_WORKSHOP_01",
        "WP_THRONE_01",
        "PLAQUE_WARN"
    };

    #endregion

    #region Properties

    public static ReadItem Instance { get; set; }

    public static bool CanRead => Instance == null || Instance.IsObtained();

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
        if (string.Equals(self.FsmName, "inspect_region"))
        {
            string key = self.FsmVariables.FindFsmString("Convo Name")?.Value ?? self.FsmVariables.FindFsmString("Game Text Convo")?.Value;
            LogHelper.Write<LoreCore>("Triggered on: " + self.Fsm.Name + "; Lore key is " + key);
        }
        if (!IsObtained() && (
                ((string.Equals(self.FsmName, "Inspection") || string.Equals(self.FsmName, "inspect_region"))
                && (_loreKeys.Contains(self.FsmVariables.FindFsmString("Convo Name")?.Value) || _loreKeys.Contains(self.FsmVariables.FindFsmString("Game Text Convo")?.Value)))
                // Special boards
                || (self.gameObject.name.EndsWith("Trial Board") && self.FsmName == "npc_control")))
        {
            try
            {
                // Try to display that the tablet is unreadable.
                self.AddState(new HutongGames.PlayMaker.FsmState(self.Fsm)
                {
                    Name = "Show unreadable prompt",
                    Actions = new HutongGames.PlayMaker.FsmStateAction[]
                    {
                        new Lambda(() => GameHelper.DisplayMessage("You can't read this."))
                    }
                });

                // Best try to make the tablets unreadable
                if (self.GetState("In Range") is FsmState)
                    self.GetState("In Range").AdjustTransition("UP PRESSED", "Show unreadable prompt");
                else
                    self.GetState("In Range?").AdjustTransition("UP PRESSED", "Show unreadable prompt");
                
                if (self.GetState("Idle") == null)
                    self.GetState("Show unreadable prompt").AddTransition("FINISHED", "Out Of Range");
                else
                    self.GetState("Show unreadable prompt").AddTransition("FINISHED", "Idle");
            }
            catch (System.Exception exception)
            {
                // Fail save
                self.GetState("Init").ClearTransitions();
            }
        }
        orig(self);
    }

    #endregion
}

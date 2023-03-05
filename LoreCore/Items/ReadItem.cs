using ItemChanger;
using ItemChanger.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoreCore.Items;

internal class ReadItem : AbstractItem
{
    #region Properties

    public static bool CanRead { get; set; }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        CanRead = false;
        On.PlayMakerFSM.OnEnable += PlayMakerFSM_OnEnable;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.PlayMakerFSM.OnEnable -= PlayMakerFSM_OnEnable;
        CanRead = true;
    }

    private void PlayMakerFSM_OnEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
    {
        if (!IsObtained() && (
                // Big Lore tablets
                (string.Equals(self.FsmName, "Inspection") && PowerManager.GetPowerByKey(self.FsmVariables.FindFsmString("Convo Name")?.Value, out Power power, false))
                ||
                // Small known lore tablets (like Record Abba) and dream warrior inspects
                (string.Equals(self.FsmName, "inspect_region") && (!RandomizerManager.PlayingRandomizer
                || !RandomizerRequestModifier.PointOfInterestLocations.Contains(self.gameObject.name))
                && (PowerManager.GetPowerByKey(self.FsmVariables.FindFsmString("Game Text Convo")?.Value, out power, false)
                || string.Equals(self.gameObject.name, "Inspect Region Ghost")))
                // Special boards
                || ((self.gameObject.name.EndsWith("Trial Board") || self.gameObject.name == "Dreamer Plaque Inspect"
                || self.gameObject.name == "Fountain Inspect" || self.transform.parent?.name == "Mantis Grave"
                || self.gameObject.name == "Antique Dealer Door" || self.gameObject.name == "Diary") && self.FsmName == "npc_control")))
            self.GetState("Init").ClearTransitions();)
        {

        }
        orig(self);
    }

    #endregion

    public override void GiveImmediate(GiveInfo info) => CanRead = true;
}

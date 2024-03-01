using HutongGames.PlayMaker.Actions;
using KorzUtils.Helper;
using System;

namespace LoreCore.Locations.SpecialLocations;

internal class JoniDreamLocation : GhostDialogueLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        On.PlayMakerFSM.OnEnable += PlayMakerFSM_OnEnable;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.PlayMakerFSM.OnEnable -= PlayMakerFSM_OnEnable;
    }

    private void PlayMakerFSM_OnEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
    {
        try
        {
            if (self.gameObject.name == "Ghost Activator" && self.transform.childCount > 0 
                && string.Equals("Ghost NPC Joni", self.transform.GetChild(0)?.name))
            {
                self.GetState("Idle").RemoveFirstAction<BoolTest>();
                self.GetState("Idle").InsertActions(0, () =>
                {
                    if (PlayerData.instance.GetBool(nameof(PlayerData.instance.hasDreamNail)))
                        self.SendEvent("SHINY PICKED UP");
                });
            }
        }
        catch (Exception exception)
        {
            LoreCore.Instance.LogError("Error while modifying joni spawn: "+exception.Message);
            LoreCore.Instance.LogError(exception.StackTrace);
        }
        orig(self);
    }
}

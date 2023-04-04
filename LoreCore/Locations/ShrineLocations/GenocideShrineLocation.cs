using KorzUtils.Helper;

namespace LoreCore.Locations.ShrineLocations;

public class GenocideShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of \"freeing\" the souls of the dead.";

    public int KillCount { get; set; }

    #endregion

    #region Event handler

    private void SetBoolValue_OnEnter(On.HutongGames.PlayMaker.Actions.SetBoolValue.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetBoolValue self)
    {
        orig(self);
        if (self.IsCorrectContext("ghost_npc_death", null, "Kill"))
        {
            KillCount++;
            ConditionMet = KillCount >= 24;
        }
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HutongGames.PlayMaker.Actions.SetBoolValue.OnEnter += SetBoolValue_OnEnter;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HutongGames.PlayMaker.Actions.SetBoolValue.OnEnter -= SetBoolValue_OnEnter;
    }

    #endregion
}

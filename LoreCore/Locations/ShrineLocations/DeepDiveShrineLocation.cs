using KorzUtils.Helper;

namespace LoreCore.Locations.ShrineLocations;

public class DeepDiveShrineLocation : ShrineLocation
{
    #region Members

    private bool _isDiving = false;

    #endregion

    #region Properties

    public override string Text => "It dreams of sky diving into a well.";

    #endregion

    #region Event handler

    private void SendMessage_OnEnter(On.HutongGames.PlayMaker.Actions.SendMessage.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SendMessage self)
    {
        if (self.IsCorrectContext("Spell Control", "Knight", "Quake Finish"))
            _isDiving = false;
        orig(self);
    }

    private void SetPlayerDataBool_OnEnter(On.HutongGames.PlayMaker.Actions.SetPlayerDataBool.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetPlayerDataBool self)
    {
        if (self.IsCorrectContext("Spell Control", "Knight", "Enter Quake"))
        {
            if (_isDiving)
                ConditionMet = true;
            _isDiving = false;
        }
        orig(self);
    }

    private void SetFloatValue_OnEnter(On.HutongGames.PlayMaker.Actions.SetFloatValue.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetFloatValue self)
    {
        if (self.IsCorrectContext("Spell Control", "Knight", "Q Off Ground") && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Town"
            && HeroController.instance.transform.localPosition.y > 40f)
            _isDiving = true;
        orig(self);
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        On.HutongGames.PlayMaker.Actions.SetFloatValue.OnEnter += SetFloatValue_OnEnter;
        On.HutongGames.PlayMaker.Actions.SetPlayerDataBool.OnEnter += SetPlayerDataBool_OnEnter;
        On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter += SendMessage_OnEnter;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        On.HutongGames.PlayMaker.Actions.SetFloatValue.OnEnter -= SetFloatValue_OnEnter;
        On.HutongGames.PlayMaker.Actions.SetPlayerDataBool.OnEnter -= SetPlayerDataBool_OnEnter;
        On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter -= SendMessage_OnEnter;
    }

    #endregion
}

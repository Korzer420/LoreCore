namespace LoreCore.Locations.ShrineLocations;

public class ZoteShrineLocation : ShrineLocation
{
    #region Properties

    public override string Text => "It dreams of quoting and following all 57 precepts of the mighty one.";

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        ConditionMet = true;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
    }

    #endregion
}

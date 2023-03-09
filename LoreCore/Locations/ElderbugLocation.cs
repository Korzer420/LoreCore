using ItemChanger;
using ItemChanger.Locations;
using LoreCore.Other;

namespace LoreCore.Locations;

internal class ElderbugLocation : AbstractLocation
{
    #region Properties

    public PlaceableLocation Location { get; set; }

    #endregion

    #region Methods

    public override AbstractPlacement Wrap() => new ElderbugPlacement(name)
    {
        Location = Location,
        tags = tags ?? new()
    };

    protected override void OnLoad() { }

    protected override void OnUnload() { }

    #endregion
}

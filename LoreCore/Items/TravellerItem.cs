using ItemChanger;
using ItemChanger.UIDefs;
using LoreCore.Enums;
using LoreCore.Locations;
using LoreCore.Modules;
using LoreCore.UIDefs;

namespace LoreCore.Items;

/// <summary>
/// An item which can play a sound and increases the stage of a traveller NPC.
/// </summary>
internal class TravellerItem : PowerLoreItem
{
    /// <summary>
    /// Gets or sets the traveller, which this item should add a stage to.
    /// </summary>
    public Traveller Traveller { get; set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        ItemChangerMod.Modules.GetOrAdd<TravellerProgressModule>();
    }

    public override void GiveImmediate(GiveInfo info)
    {
        base.GiveImmediate(info);
        TravellerLocation.Stages[Traveller]++;
        int currentState = TravellerLocation.Stages[Traveller] > 10 
            ? TravellerLocation.Stages[Traveller] - 10 
            : TravellerLocation.Stages[Traveller];
        int maxStage = Traveller switch
        {
            Traveller.Quirrel => 10,
            Traveller.Cloth => 5,
            Traveller.Tiso => 5,
            Traveller.Zote => 6,
            _ => 6
        };
        if (UIDef is LoreUIDef lore)
            lore.name = new BoxedString($"{lore.name.Value} ({currentState} / {maxStage})");
        else
            (UIDef as DreamLoreUIDef).name = new BoxedString($"{(UIDef as DreamLoreUIDef).name.Value} Level ({currentState} / {maxStage})");

        // Allow cloth to enter the traitor lord fight (not sure if this is necessary, but just in case)
        if (Traveller == Traveller.Cloth && TravellerLocation.Stages[Traveller.Cloth] >= 3)
            PlayerData.instance.SetBool(nameof(PlayerData.instance.savedCloth), true);
        else if (Traveller == Traveller.Zote && TravellerLocation.Stages[Traveller.Zote] >= 4)
            PlayerData.instance.SetBool(nameof(PlayerData.instance.zoteRescuedDeepnest), true);
    }
}

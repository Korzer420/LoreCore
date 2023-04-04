using ItemChanger;
using ItemChanger.Modules;
using LoreCore.Enums;
using LoreCore.Locations;
using System;
using System.Text;

namespace LoreCore.Modules;

/// <summary>
/// ItemChanger module which adds the traveller level to the inventory tracker.
/// </summary>
public class TravellerProgressModule : Module
{
    public override void Initialize()
    {
        if (ItemChangerMod.Modules?.Get<InventoryTracker>() is InventoryTracker inventoryTracker)
            inventoryTracker.OnGenerateFocusDesc += AppendTravellerProgress;
    }

    public override void Unload()
    {
        if (ItemChangerMod.Modules?.Get<InventoryTracker>() is InventoryTracker inventoryTracker)
            inventoryTracker.OnGenerateFocusDesc -= AppendTravellerProgress;
    }

    private void AppendTravellerProgress(StringBuilder stringBuilder)
    {
        // If one is above ten, the traveller order "none" is used, which renderes the tracker useless.
        if (TravellerLocation.Stages[Traveller.Zote] >= 10)
            return;
        foreach (Traveller item in Enum.GetValues(typeof(Traveller)))
            stringBuilder.AppendLine($"{item} Level: {TravellerLocation.Stages[item]}");
    }
}

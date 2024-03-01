using ItemChanger;
using ItemChanger.Modules;
using LoreCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoreCore.Modules;

/// <summary>
/// ItemChanger module which adds the traveller level to the inventory tracker.
/// </summary>
public class TravellerProgressModule : Module
{
    #region Eventhandler

    private void AppendTravellerProgress(StringBuilder stringBuilder)
    {
        // If one is above ten, the traveller order "none" is used, which renderes the tracker useless.
        if (TravellerControlModule.CurrentModule.Stages[Traveller.Zote] >= 10)
            return;
        foreach (Traveller item in Enum.GetValues(typeof(Traveller)))
            stringBuilder.AppendLine($"{item} Level: {TravellerControlModule.CurrentModule.Stages[item]}");
    }

    #endregion

    #region Methods

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

    #endregion
}

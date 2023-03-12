using ItemChanger;
using ItemChanger.Modules;
using LoreCore.Enums;
using LoreCore.Locations;
using LoreCore.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoreCore.Modules;

public class LoreProgressModule : Module
{
    public override void Initialize()
    {
        if (ItemChangerMod.Modules?.Get<InventoryTracker>() is InventoryTracker inventoryTracker)
            inventoryTracker.OnGenerateFocusDesc += AppendLoreProgress;
    }

    public override void Unload()
    {
        if (ItemChangerMod.Modules?.Get<InventoryTracker>() is InventoryTracker inventoryTracker)
            inventoryTracker.OnGenerateFocusDesc -= AppendLoreProgress;
    }

    private void AppendLoreProgress(StringBuilder stringBuilder)
    {
        if (ElderbugPlacement.Instance == null)
            return;
        stringBuilder.AppendLine("Acquired lore: " + ElderbugPlacement.Instance.AcquiredLore);
    }
}

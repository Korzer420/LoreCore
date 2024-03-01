using ItemChanger;
using ItemChanger.UIDefs;
using KorzUtils.Helper;
using LoreCore.Enums;
using LoreCore.Modules;
using LoreCore.UIDefs;
using System;

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
        try
        {
            TravellerControlModule.CurrentModule.Stages[Traveller]++;
            int currentState = TravellerControlModule.CurrentModule.Stages[Traveller] > 10
                ? TravellerControlModule.CurrentModule.Stages[Traveller] - 10
                : TravellerControlModule.CurrentModule.Stages[Traveller];
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
            else if (UIDef != null)
                (UIDef as DreamLoreUIDef).name = new BoxedString($"{(UIDef as DreamLoreUIDef)?.name?.Value} Level ({currentState} / {maxStage})");

            // Allow cloth to enter the traitor lord fight (not sure if this is necessary, but just in case)
            if (Traveller == Traveller.Cloth && TravellerControlModule.CurrentModule.Stages[Traveller.Cloth] >= 3)
                PlayerData.instance.SetBool(nameof(PlayerData.instance.savedCloth), true);
            else if (Traveller == Traveller.Zote && TravellerControlModule.CurrentModule.Stages[Traveller.Zote] >= 4)
                PlayerData.instance.SetBool(nameof(PlayerData.instance.zoteRescuedDeepnest), true);
        }
        catch (Exception exception)
        {
            LogHelper.Write<LoreCore>("Failed to give traveller item", exception);
        }
    }
}

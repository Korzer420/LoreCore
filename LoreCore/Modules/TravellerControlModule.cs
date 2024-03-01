using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Modules;
using KorzUtils.Helper;
using LoreCore.Enums;
using Modding;
using System.Collections.Generic;

namespace LoreCore.Modules;

/// <summary>
/// ItemChanger Module which controls whether the traveller npc can spawn in certain scenarios.
/// </summary>
public class TravellerControlModule : Module
{
    #region Members

    private Dictionary<Traveller, int> _stages;

    #endregion

    #region Properties

    public Dictionary<Traveller, int> Stages
    {
        get => _stages ??= new()
        {
            {Traveller.Quirrel , 0 },
            {Traveller.Cloth, 0 },
            {Traveller.Tiso, 0 },
            {Traveller.Zote, 0 },
            {Traveller.Hornet, 0 }
        };
        set => _stages = value;
    }

    public static TravellerControlModule CurrentModule => ItemChangerMod.Modules.GetOrAdd<TravellerControlModule>();

    #endregion

    #region Event handler

    private bool ModHooks_GetPlayerBoolHook(string name, bool orig)
    {
        if (name == nameof(PlayerData.zoteRescuedDeepnest))
            return Stages[Traveller.Zote] >= 5;
        return orig;
    }

    private void BlockQuirrel(PlayMakerFSM fsm)
    {
        if (Stages[Traveller.Quirrel] >= 8)
            return;
        fsm.GetState("Idle").GetFirstAction<FloatAdd>().add.Value = 0;
        fsm.GetState("Idle").AdjustTransition("QUIRREL", "Attack Antic");
        fsm.GetState("Quirrel?").AdjustTransition("QUIRREL", "Idle");
        // Prevent Quirrel from jumping around the room.
        fsm.GetState("Quirrel?").GetFirstAction<BoolTest>().isTrue = fsm.GetState("Quirrel?").GetFirstAction<BoolTest>().isFalse;
    }

    private void SkipHornet(PlayMakerFSM fsm)
    {
        if (Stages[Traveller.Hornet] >= 6)
            return;
        fsm.GetState("Set Phase 4").AdjustTransition("HORNET READY", "Idle 4");
    }

    #endregion

    public override void Initialize()
    {
        // For controlling if zote appears in colo.
        ModHooks.GetPlayerBoolHook += ModHooks_GetPlayerBoolHook;

        // Quirrel in archives.
        Events.AddFsmEdit(new("Mega Jellyfish", "Mega Jellyfish"), BlockQuirrel);

        // Hornet in THK fight.
        Events.AddFsmEdit(new("Hollow Knight Boss", "Phase Control"), SkipHornet);
    }

    public override void Unload()
    {
        ModHooks.GetPlayerBoolHook -= ModHooks_GetPlayerBoolHook;
        Events.RemoveFsmEdit(new("Mega Jellyfish", "Mega Jellyfish"), BlockQuirrel);
        Events.RemoveFsmEdit(new("Hollow Knight Boss", "Phase Control"), SkipHornet);
    }
}

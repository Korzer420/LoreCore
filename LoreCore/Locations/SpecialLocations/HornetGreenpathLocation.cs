﻿using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations.SpecialLocations;

internal class HornetGreenpathLocation : AutoLocation
{
    protected override void OnLoad()
    {
        Events.AddFsmEdit(new("Hornet Infected Knight Encounter", "Encounter"), SkipDialogue);
        Events.AddSceneChangeEdit("Fungus1_04", SpawnReoccurringItem);
    }

    protected override void OnUnload()
    {
        Events.RemoveSceneChangeEdit("Fungus1_04", SpawnReoccurringItem);
        Events.RemoveFsmEdit(new("Hornet Infected Knight Encounter", "Encounter"), SkipDialogue);
    }

    private void SkipDialogue(PlayMakerFSM fsm)
    {
        if (Placement.Items.All(x => x.IsObtained()))
            return;
        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Give items",
            Actions = new HutongGames.PlayMaker.FsmStateAction[]
            {
                new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                        {
                            FlingType = flingType,
                            Container = Container.Tablet,
                            MessageType = MessageType.Any,
                        }, callback), "CONVO_FINISH")
            }
        });
        fsm.GetState("Point").AdjustTransition("FINISHED", "Give items");
        fsm.GetState("Give items").AddTransition("CONVO_FINISH", "Set Hero Active");
        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Give items attack",
            Actions = new HutongGames.PlayMaker.FsmStateAction[]
            {
                new Lambda(() => Placement.GiveAll(new GiveInfo()
                {
                    Container = Container.Unknown,
                    FlingType = FlingType.DirectDeposit,
                    MessageType = MessageType.Corner
                }))
            }
        });
        fsm.GetState("Attacked").AdjustTransition("FINISHED", "Give items attack");
        fsm.GetState("Give items attack").AddTransition("FINISHED", "Leap Antic");
    }

    private void SpawnReoccurringItem(Scene scene)
    {
        if (Placement.AllObtained() || Placement.Items.All(x => !x.WasEverObtained()))
            return;
        ItemHelper.SpawnShiny(new(25.83f, 28.42f), Placement);
    }
}
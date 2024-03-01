using HutongGames.PlayMaker;
using ItemChanger;

using ItemChanger.FsmStateActions;
using ItemChanger.Util;
using KorzUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LoreCore.Locations.SpecialLocations;

/// <summary>
/// The dream dialogue location of the grimm summoner bug (The bug which needs to be dream nailed to summon the grimm troupe)
/// </summary>
internal class GrimmSummonerDreamLocation : DreamNailLocation
{
    protected override void OnLoad()
    {
        base.OnLoad();
        Events.AddFsmEdit(sceneName, new("Sycophant Dream", "Activate Lantern"), BlockLantern);
        Events.AddFsmEdit(sceneName, new(GameObjectName, "FSM"), SummonerSpawn);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        Events.RemoveFsmEdit(sceneName, new("Sycophant Dream", "Activate Lantern"), BlockLantern);
        Events.RemoveFsmEdit(sceneName, new(GameObjectName, "FSM"), SummonerSpawn);
    }

    private void BlockLantern(PlayMakerFSM fsm)
    {
        fsm.AddState(new FsmState(fsm.Fsm)
        {
            Name = "Block interaction",
            Actions = new FsmStateAction[]
            {
                new AsyncLambda(callback =>
                {
                    if (!Placement.AllObtained())
                    {
                        ItemUtility.GiveSequentially(Placement.Items, Placement, new()
                        {
                            FlingType = FlingType.DirectDeposit,
                            MessageType = MessageType.Lore,
                            Container = Container.Tablet
                        }, callback);
                    }
                    else
                        callback?.Invoke();
                }, "NAIL HIT")
            }
        });

        fsm.GetState("Idle").AdjustTransition("NAIL HIT", "Block interaction");
        fsm.GetState("Block interaction").AddTransition("NAIL HIT", "Idle");
    }

    /// <summary>
    /// Stores the reference of the summoner.
    /// </summary>
    private void SummonerSpawn(PlayMakerFSM fsm)
    {
        fsm.AddState(new FsmState(fsm.Fsm)
        {
            Name = "Clear Repeat",
            Actions = new FsmStateAction[]
            {
                new Lambda(() => 
                {
                    if (!PlayerData.instance.GetBool(nameof(PlayerData.instance.nightmareLanternLit)))
                        fsm.SendEvent("DEACTIVATE");
                })
            }
        });
        fsm.GetState("Check").AdjustTransition("DEACTIVATE", "Clear Repeat");
        fsm.GetState("Clear Repeat").AddTransition("DEACTIVATE", "Deactivate");
    }
}

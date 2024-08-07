using HutongGames.PlayMaker.Actions;
using ItemChanger;

using ItemChanger.FsmStateActions;
using ItemChanger.Locations;
using ItemChanger.Util;
using KorzUtils.Helper;
using LoreCore.Items;
using LoreCore.Modules;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoreCore.Locations.SpecialLocations;

internal class HornetFountainLocation : AutoLocation
{
    protected override void OnLoad()
    {
        Events.AddFsmEdit(new("Hornet Fountain Encounter", "Control"), ModifyCutscene);
        Events.AddSceneChangeEdit("Ruins1_27", SpawnShiny);
    }

    protected override void OnUnload()
    {
        Events.RemoveFsmEdit(new("Hornet Fountain Encounter", "Control"), ModifyCutscene);
        Events.RemoveSceneChangeEdit("Ruins1_27", SpawnShiny);
    }

    private void ModifyCutscene(PlayMakerFSM fsm)
    {
        if (Placement.AllObtained() || Placement.Items.All(x => x.WasEverObtained()) 
            || !ListenItem.CanListen || TravellerControlModule.CurrentModule.Stages[Enums.Traveller.Hornet] < 1)
        {
            Object.Destroy(fsm.gameObject);
            return;
        }
        // Remove destroy check.
        fsm.GetState("Init").RemoveFirstAction<PlayerDataBoolTest>();

        fsm.AddState(new HutongGames.PlayMaker.FsmState(fsm.Fsm)
        {
            Name = "Give items",
            Actions =
            [
                new AsyncLambda(callback => ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo
                        {
                            FlingType = flingType,
                            Container = Container.Tablet,
                            MessageType = MessageType.Any,
                        }, callback), "CONVO_FINISH")
            ]
        });
        fsm.GetState("Fade In").AdjustTransition("FINISHED", "Give items");
        fsm.GetState("Give items").AddTransition("CONVO_FINISH", "Jump");
    }

    private void SpawnShiny(Scene scene)
    {
        if (!Placement.AllObtained() && Placement.Items.All(x => x.WasEverObtained()))
            ItemHelper.SpawnShiny(new(37.7f, 5.42f), Placement);
    }
}
